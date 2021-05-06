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
using Newtonsoft.Json;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// Apply to controllers or actions to require that a valid OrderCloud access token is provided in the Authorization header.
	/// </summary>
	public class OrderCloudUserAuthAttribute : AuthorizeAttribute
	{
		public List<string> OrderCloudRoles => Roles?.Split(",")?.ToList() ?? new List<string> { };

		public OrderCloudUserAuthAttribute()
		{
			AuthenticationSchemes = "OrderCloudUser";
		}

		/// <param name="roles">Optional list of roles. If provided, user must have just one of them, otherwise authorization fails.</param>
		public OrderCloudUserAuthAttribute(params ApiRole[] roles)
		{
			AuthenticationSchemes = "OrderCloudUser";
			if (roles.Any())
				Roles = string.Join(",", roles);
		}

		/// <param name="roles">Optional list of roles. If provided, user must have just one of them, otherwise authorization fails.</param>
		public OrderCloudUserAuthAttribute(params string[] roles)
		{
			AuthenticationSchemes = "OrderCloudUser";
			if (roles.Any())
				Roles = string.Join(",", roles);
		}
	}

	public class OrderCloudUserAuthHandler : AuthenticationHandler<OrderCloudUserAuthOptions>
	{
		private static UserContextProvider _tokenProvider;

		public OrderCloudUserAuthHandler(
			IOptionsMonitor<OrderCloudUserAuthOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock,
			UserContextProvider tokenProvider
			)
			: base(options, logger, encoder, clock)
		{
			_tokenProvider = tokenProvider;
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
			try {
				var requiredRoles = Context.GetRequiredOrderCloudRoles();
				var token = await _tokenProvider.VerifyTokenAsync(Request, requiredRoles);
				var cid = new ClaimsIdentity("OcUser");
				cid.AddClaims(token.Roles.Select(r => new Claim(ClaimTypes.Role, r)));
				cid.AddClaim(new Claim("AccessToken", token.AccessToken));

				var ticket = new AuthenticationTicket(new ClaimsPrincipal(cid), "OcUser");
				return AuthenticateResult.Success(ticket);
			}
			catch (CatalystBaseException ex) when (ex.HttpStatus == 403)
			{
				throw ex;
			}
			catch (OrderCloudException ex)
			{
				throw ex;
			}
			catch (Exception ex) {
				throw new UnAuthorizedException();
			}
		}
	}

	public class OrderCloudUserAuthOptions : AuthenticationSchemeOptions { }
}
