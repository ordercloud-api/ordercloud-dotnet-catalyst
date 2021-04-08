using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using OrderCloud.Catalyst.Auth.UserAuth;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{	
	public class VerifiedUserContext
	{	
		public JwtOrderCloud Token => this.token ?? throw new NoUserContextException();
		public IOrderCloudClient OcClient
		{
			get
			{
				if (this.ocClient == null)
				{
					this.ocClient = JWT.BuildOrderCloudClient(Token);
				}
				return this.ocClient;
			}
		}

		private JwtOrderCloud token;
		private IOrderCloudClient ocClient;
		private readonly ISimpleCache _cache;
		private readonly IOrderCloudClient _oc;

		public VerifiedUserContext(ISimpleCache cache, IOrderCloudClient oc)
		{
			_cache = cache;
			_oc = oc;
		}

		public async Task SetAsync(HttpRequest request, List<string> requiredRoles = null)
		{
			var token = request.GetOrderCloudToken();
			await SetAsync(token, requiredRoles);
		}

		public async Task SetAsync(string token, List<string> requiredRoles = null)
		{
			if (string.IsNullOrEmpty(token))
				throw new UnAuthorizedException();

			var parsedToken = new JwtOrderCloud(token);

			if (parsedToken.ClientID == null || parsedToken.NotValidBeforeUTC < DateTime.UtcNow || parsedToken.ExpiresUTC > DateTime.UtcNow)
				throw new UnAuthorizedException();

			// we've validated the token as much as we can on this end, go make sure it's ok on OC	
			var allowValidateTokenRetry = false;
			var isValid = await _cache.GetOrAddAsync(token, TimeSpan.FromMinutes(5), async () =>
			{
				try
				{
					// some valid tokens - e.g. those from the portal - do not have a "kid"
					if (parsedToken.KeyID == null)
					{
						var user = await _oc.Me.GetAsync(token);
						return user != null && user.Active;
					}
					else
					{
						var publicKey = await _oc.Certs.GetPublicKeyAsync(parsedToken.KeyID);
						return JWT.IsTokenCryptoValid(token, publicKey);
					}
				}
				catch (FlurlHttpException ex) when (ex.Call.Response?.StatusCode < 500)
				{
					return false;
				}
				catch (Exception)
				{
					allowValidateTokenRetry = true;
					return false;
				}
			});
			if (allowValidateTokenRetry)
				await _cache.RemoveAsync(token); // not their fault, don't make them wait 5 min      

			if (!isValid)
				throw new UnAuthorizedException();

			if (requiredRoles != null && requiredRoles.Count > 0 && !requiredRoles.Any(role => parsedToken.AvailableRoles.Contains(role)))
			{
				throw new InsufficientRolesException(new InsufficientRolesError()
				{
					SufficientRoles = requiredRoles,
					AssignedRoles = parsedToken.AvailableRoles.ToList()
				});
			}
			this.token = parsedToken;
		}
	}
}
