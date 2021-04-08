using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public interface IUserContextProvider
	{
		Task<VerifiedUserContext> VerifyAsync(string token, List<string> roles = null);
		Task<VerifiedUserContext> VerifyAsync(HttpRequest request, List<string> roles = null);
	}

	public class UserContextProvider : IUserContextProvider
	{
		private readonly IOrderCloudClient _oc;
		private readonly ISimpleCache _cache;

		public UserContextProvider(ISimpleCache cache, IOrderCloudClient oc)
		{
			_oc = oc;
			_cache = cache;
		}

        public async Task<VerifiedUserContext> VerifyAsync(HttpRequest request, List<string> roles = null)
        {
            var token = request.GetOrderCloudToken();
            return await VerifyAsync(token, roles);
        }

        public async Task<VerifiedUserContext> VerifyAsync(string token, List<string> roles = null)
		{     
            if (string.IsNullOrEmpty(token))
                throw new UnAuthorizedException();

            var jwt = new JwtOrderCloud(token);
            if (jwt.ClientID == null)
                throw new UnAuthorizedException();

            // we've validated the token as much as we can on this end, go make sure it's ok on OC	
            var allowFetchUserRetry = false;
            var user = await _cache.GetOrAddAsync(token, TimeSpan.FromMinutes(5), () =>
            {
                try
                {
                    return _oc.Me.GetAsync(token);
                }
                catch (OrderCloudException ex) 
                {
                    throw ex;
			    }
                catch (FlurlHttpException ex) when ((int?)ex.Call.Response?.StatusCode < 500)
                {
                    return null;
                }
                catch (Exception ex)
                {
                    allowFetchUserRetry = true;
                    return null;
                }
            });

            if (allowFetchUserRetry)
                await _cache.RemoveAsync(token); // not their fault, don't make them wait 5 min

            if (user == null || !user.Active)
                throw new UnAuthorizedException();
            var cid = new ClaimsIdentity("OcUser");
            cid.AddClaim(new Claim("accesstoken", token));
            cid.AddClaim(new Claim("userrecordjson", JsonConvert.SerializeObject(user)));
            cid.AddClaims(user.AvailableRoles.Select(r => new Claim(ClaimTypes.Role, r)));

            if (roles != null && roles.Count > 0 && !roles.Any(role => user.AvailableRoles.Contains(role)))
            {
                throw new InsufficientRolesException(new InsufficientRolesError()
                {
                    SufficientRoles = roles,
                    AssignedRoles = user.AvailableRoles.ToList()
                });
            }

            return new VerifiedUserContext(new ClaimsPrincipal(cid));
        }
	}
}
