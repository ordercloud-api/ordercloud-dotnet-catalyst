using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
    /// <summary>
    /// Apply to controllers or actions to require that a valid OrderCloud access token is provided in the Authorization header.
    /// </summary>
    public class OrderCloudUserInfoAuthAttribute : AuthorizeAttribute
    {
        public List<string> OrderCloudRoles => Roles?.Split(',')?.ToList() ?? new List<string> { };

        public OrderCloudUserInfoAuthAttribute()
        {
            AuthenticationSchemes = "OrderCloudUserInfo";
        }

        /// <param name="roles">Optional list of roles. If provided, user must have just one of them, otherwise authorization fails.</param>
        public OrderCloudUserInfoAuthAttribute(params ApiRole[] roles)
        {
            AuthenticationSchemes = "OrderCloudUserInfo";
            var rolesList = roles.ToList();
            rolesList.Add(ApiRole.FullAccess);
            Roles = string.Join(",", rolesList);
        }

        /// <param name="roles">Optional list of roles. If provided, user must have just one of them, otherwise authorization fails.</param>
        public OrderCloudUserInfoAuthAttribute(params string[] roles)
        {
            AuthenticationSchemes = "OrderCloudUserInfo";
            var rolesList = roles.ToList();
            rolesList.Add("FullAccess");
            Roles = string.Join(",", rolesList);
        }
    }

    public class OrderCloudUserInfoAuthHandler : AuthenticationHandler<OrderCloudUserInfoAuthOptions>
    {
        private readonly IRequestAuthenticationService _auth;

        public OrderCloudUserInfoAuthHandler(
            IOptionsMonitor<OrderCloudUserInfoAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IRequestAuthenticationService auth
            )
            : base(options, logger, encoder, clock)
        {
            _auth = auth;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var requiredRoles = Context.GetRequiredOrderCloudRoles();
                var token = await _auth.VerifyUserInfoTokenAsync(Request, requiredRoles);
                var cid = new ClaimsIdentity("OcUserInfo");
                cid.AddClaims(token.Roles.Select(r => new Claim(ClaimTypes.Role, r)));
                cid.AddClaim(new Claim("UserInfoToken", token.UserInfoToken));

                var ticket = new AuthenticationTicket(new ClaimsPrincipal(cid), "OcUserInfo");
                return AuthenticateResult.Success(ticket);
            }
            catch (CatalystBaseException ex)
            {
                throw ex;
            }
            catch (OrderCloudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new UnAuthorizedException();
            }
        }
    }

    public class OrderCloudUserInfoAuthOptions : AuthenticationSchemeOptions
    {

    }
}
