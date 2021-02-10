using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
	/// Apply to controllers or actions to require that a valid OrderCloud access token is provided in the Authorization heaader.
	/// </summary>
	public class OrderCloudUserAuthAttribute : AuthorizeAttribute
	{
		/// <param name="roles">Optional list of roles. If provided, user must have just one of them, otherwise authorization fails.</param>
		public OrderCloudUserAuthAttribute(params ApiRole[] roles) {
			AuthenticationSchemes = "OrderCloudUser";
			if (roles.Any())
				Roles = string.Join(",", roles);
		}
	}

	public class OrderCloudUserAuthHandler : AuthenticationHandler<OrderCloudUserAuthOptions>
	{
		private readonly IOrderCloudClient _ocClient;

		public OrderCloudUserAuthHandler(IOptionsMonitor<OrderCloudUserAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IOrderCloudClient ocClient)
			: base(options, logger, encoder, clock)
		{
			_ocClient = ocClient;
		}

		// todo: add caching?
		protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
			try {
				var token = GetTokenFromAuthHeader();

				if (string.IsNullOrEmpty(token))
					return AuthenticateResult.Fail("The OrderCloud bearer token was not provided in the Authorization header.");

				var jwt = new JwtSecurityToken(token);
				var clientID = jwt.Claims.FirstOrDefault(x => x.Type == "cid")?.Value;
				if (clientID == null)
					return AuthenticateResult.Fail("The provided bearer token does not contain a 'cid' (Client ID) claim.");
				if (!Options.ValidClientIDs.Contains(clientID, StringComparer.InvariantCultureIgnoreCase))
					return AuthenticateResult.Fail("Client ID from token is not valid for this integration.");

				// we've validated the token as much as we can on this end, go make sure it's ok on OC
				var user = await _ocClient.Me.GetAsync(token);

				var cid = new ClaimsIdentity("OcUser");
				cid.AddClaim(new Claim("clientid", clientID));
				cid.AddClaim(new Claim("accesstoken", token));
				cid.AddClaim(new Claim("username", user.Username));
				cid.AddClaims(user.AvailableRoles.Select(r => new Claim(ClaimTypes.Role, r)));

				var anon = jwt.Claims.FirstOrDefault(x => x.Type == "orderid");
				if (anon != null)
					cid.AddClaim(new Claim("anonorderid", anon.Value));

				var ticket = new AuthenticationTicket(new ClaimsPrincipal(cid), "OcUser");
				return AuthenticateResult.Success(ticket);
			}
			catch (Exception ex) {
				return AuthenticateResult.Fail(ex.Message);
			}
		}

		private string GetTokenFromAuthHeader() {
			if (!Request.Headers.TryGetValue("Authorization", out var header))
				return null;

			var parts = header.FirstOrDefault()?.Split(new[] { ' ' }, 2);
			if (parts?.Length != 2)
				return null;

			if (parts[0] != "Bearer")
				return null;

			return parts[1].Trim();
		}
	}

	public class OrderCloudUserAuthOptions : AuthenticationSchemeOptions
	{
		public List<string> ValidClientIDs { get; set; } = new List<string>();

		/// <summary>
		/// Enforce that only tokens associated with specific OrderCloud client ID(s) are allowed to access endpoints marked with [OrderCloudUserAuth].
		/// </summary>
		public OrderCloudUserAuthOptions AddValidClientIDs(params string[] clientIDs) {
			ValidClientIDs.AddRange(clientIDs);
			return this;
		}
	}
}
