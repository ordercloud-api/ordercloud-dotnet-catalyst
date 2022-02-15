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
		public OrderCloudWebhookAuthHandler(IOptionsMonitor<OrderCloudWebhookAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock) { }

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
			Require.That(!string.IsNullOrEmpty(Options.HashKey),
				new InvalidOperationException("OrderCloudWebhookAuthOptions.HashKey was not configured."));

			Require.That(Context.Request.Headers.ContainsKey("X-oc-hash"), new WebhookUnauthorizedException());

			var sent = Context.Request.Headers["X-oc-hash"].FirstOrDefault();
			Require.That(!string.IsNullOrEmpty(sent), new WebhookUnauthorizedException());

			Context.Request.EnableBuffering();
			// Just choose something reasonable - https://stackoverflow.com/questions/3033771/file-i-o-with-streams-best-memory-buffer-size
			var bufferSize = 4096;

			try {
				using (var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8, false, bufferSize, true))
				{
					var bodyBytes = Encoding.UTF8.GetBytes(await reader.ReadToEndAsync());

					var keyBytes = Encoding.UTF8.GetBytes(Options.HashKey);
					var hash = new HMACSHA256(keyBytes).ComputeHash(bodyBytes);
					var computed = Convert.ToBase64String(hash);

					Require.That(sent == computed, new WebhookUnauthorizedException());

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
