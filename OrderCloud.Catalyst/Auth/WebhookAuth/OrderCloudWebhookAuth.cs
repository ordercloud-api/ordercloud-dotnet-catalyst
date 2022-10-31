using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// Apply to controllers or actions to authenticate the webhook by validating the hash key passed in the X-oc-hash header.
	/// </summary>
	public class OrderCloudWebhookAuthAttribute : AuthorizeAttribute
	{
		public OrderCloudWebhookAuthAttribute() {
			AuthenticationSchemes = "OrderCloudWebhook";
		}
	}

	public class OrderCloudWebhookAuthHandler : AuthenticationHandler<OrderCloudWebhookAuthOptions>
	{
		private static RequestAuthenticationService _requestAuthenticationService;

		public OrderCloudWebhookAuthHandler(
			IOptionsMonitor<OrderCloudWebhookAuthOptions> options, 
			ILoggerFactory logger, 
			UrlEncoder encoder, 
			ISystemClock clock,
			RequestAuthenticationService requestAuthenticationService
			) : base(options, logger, encoder, clock) 
		{
			_requestAuthenticationService = requestAuthenticationService;
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
			await _requestAuthenticationService.VerifyWebhookHashAsync(Request, Options); // Will throw error if fails
			var cid = new ClaimsIdentity("OcWebhook");
			var ticket = new AuthenticationTicket(new ClaimsPrincipal(cid), "OcWebhook");
			return AuthenticateResult.Success(ticket);
		}
	}

	public class OrderCloudWebhookAuthOptions : AuthenticationSchemeOptions
	{
		public string HashKey { get; set; }
	}
}
