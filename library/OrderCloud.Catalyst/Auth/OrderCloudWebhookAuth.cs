using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Internal;
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
		public OrderCloudWebhookAuthHandler(IOptionsMonitor<OrderCloudWebhookAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock) { }

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
			if (string.IsNullOrEmpty(Options.HashKey)) {
				throw new InvalidOperationException("OrderCloudWebhookAuthOptions.HashKey was not configured.");
			}

			if (!Context.Request.Headers.ContainsKey("X-oc-hash")) {
				return AuthenticateResult.Fail("X-oc-hash header was not sent. Endpoint can only be hit from a valid OrderCloud webhook.");
			}

			var sent = Context.Request.Headers["X-oc-hash"].FirstOrDefault();
			if (string.IsNullOrEmpty(sent)) {
				return AuthenticateResult.Fail("X-oc-hash header was not sent. Endpoint can only be hit from a valid OrderCloud webhook.");
			}

			Context.Request.EnableRewind();

			try {
				var keyBytes = Encoding.UTF8.GetBytes(Options.HashKey);
				var hash = new HMACSHA256(keyBytes).ComputeHash(Context.Request.Body);
				var computed = Convert.ToBase64String(hash);

				if (sent != computed) {
					return AuthenticateResult.Fail("X-oc-hash header does not match. Endpoint can only be hit from a valid OrderCloud webhook.");
				}
				else {
					var cid = new ClaimsIdentity("OcWebhook");
					var ticket = new AuthenticationTicket(new ClaimsPrincipal(cid), "OcWebhook");
					return AuthenticateResult.Success(ticket);
				}
			}
			finally {
				Context.Request.Body.Position = 0;
			}
		}
	}

	public class OrderCloudWebhookAuthOptions : AuthenticationSchemeOptions
	{
		public string HashKey { get; set; }
	}
}
