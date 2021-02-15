using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
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
		public OrderCloudUserAuthAttribute(params ApiRole[] roles)
		{
			AuthenticationSchemes = "OrderCloudUser";
			if (roles.Any())
				Roles = string.Join(",", roles);
		}
	}

	public class OrderCloudUserAuthHandler<TSettings> : AuthenticationHandler<OrderCloudUserAuthOptions>
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

				var jwt = new JwtOrderCloud(token);
				if (jwt.ClientID == null)
					return AuthenticateResult.Fail("The provided bearer token does not contain a 'cid' (Client ID) claim.");

				// we've validated the token as much as we can on this end, go make sure it's ok on OC
				var user = await _ocClient.Me.GetAsync(token);
				if (!user.Active)
                    return AuthenticateResult.Fail("Authentication failure");
				var cid = new ClaimsIdentity("OcUser");
				cid.AddClaim(new Claim("clientid", jwt.ClientID));
				cid.AddClaim(new Claim("accesstoken", token));
				cid.AddClaim(new Claim("username", user.Username));
				cid.AddClaim(new Claim("userid", user.ID));
				cid.AddClaim(new Claim("email", user.Email ?? ""));
				cid.AddClaim(new Claim("buyer", user.Buyer?.ID ?? ""));
				cid.AddClaim(new Claim("supplier", user.Supplier?.ID ?? ""));
				cid.AddClaim(new Claim("seller", user?.Seller?.ID ?? ""));
				cid.AddClaims(user.AvailableRoles.Select(r => new Claim(ClaimTypes.Role, r)));

				if (jwt.IsAnon)
					cid.AddClaim(new Claim("anonorderid", jwt.OrderID));

				var ticket = new AuthenticationTicket(new ClaimsPrincipal(cid), "OcUser");
				return AuthenticateResult.Success(ticket);
			}
			catch (Exception ex) {
				return AuthenticateResult.Fail(ex.Message);
			}
		}

		protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
		{
			var token = GetTokenFromAuthHeader();
			var jwt = new JwtOrderCloud(token);
			throw new InsufficientRolesException(new InsufficientRolesError()
			{
				SufficientRoles = GetUserAuthAttribute().Roles.Select(r => r.ToString()).ToList(),
				AssignedRoles = jwt.Roles,
			});
		}

		private OrderCloudUserAuthAttribute GetUserAuthAttribute()
		{
			var controllerName = Request.RouteValues["controller"].ToString();
			var actionName = Request.RouteValues["action"].ToString();
			var assembly = Assembly.GetAssembly(typeof(TSettings));
			var controlType = assembly.GetTypes().First(t => t.Name == $"{controllerName}Controller");
			return controlType
				.GetMethod(actionName)
				.GetCustomAttribute(typeof(OrderCloudUserAuthAttribute)) as OrderCloudUserAuthAttribute;
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

	public class OrderCloudUserAuthOptions : AuthenticationSchemeOptions { }
}
