using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Flurl.Http;
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

	public class OrderCloudUserAuthHandler<TSettings> : AuthenticationHandler<OrderCloudUserAuthOptions>
	{
		private readonly IOrderCloudClient _oc;
		private readonly ISimpleCache _cache;

		public OrderCloudUserAuthHandler(
			IOptionsMonitor<OrderCloudUserAuthOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock,
			ISimpleCache cache,
			IOrderCloudClient ocClient)
			: base(options, logger, encoder, clock)
		{
			_oc = ocClient;
			_cache = cache;
		}

		private async Task<ClaimsPrincipal> VerifyToken(string token)
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
				catch (FlurlHttpException ex) when ((int?)ex.Call.Response?.StatusCode < 500)
				{
					return null;
				}
				catch (Exception)
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

			return new ClaimsPrincipal(cid);
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
			try {
				var token = GetTokenFromAuthHeader();
				var user = await VerifyToken(token);

				var ticket = new AuthenticationTicket(user, "OcUser");
				return AuthenticateResult.Success(ticket);
			}
			catch (Exception ex) {
				throw new UnAuthorizedException();
			}
		}

		protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
		{
			var token = GetTokenFromAuthHeader();
			var jwt = new JwtOrderCloud(token);
			throw new InsufficientRolesException(new InsufficientRolesError()
			{
				SufficientRoles = GetRouteAuthAttribute().Roles.Split(","),
				AssignedRoles = jwt.Roles,
			});
		}

		private OrderCloudUserAuthAttribute GetRouteAuthAttribute()
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
