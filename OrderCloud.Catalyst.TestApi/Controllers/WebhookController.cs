using Microsoft.AspNetCore.Mvc;
using OrderCloud.Catalyst;
using OrderCloud.SDK;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.TestApi
{
	public class WebhookController : CatalystController
	{
		private RequestAuthenticationService _service;
		private TestSettings _settings;

		public WebhookController(RequestAuthenticationService service, TestSettings settings)
		{
			_service = service;
			_settings = settings;
		}

		[HttpPost("webhook/saveaddress/protected-by-attribute"), OrderCloudWebhookAuth]
		public async Task<object> ProtectedByAttribute([FromBody] WebhookPayloads.Addresses.Save payload)
		{
			return new
			{
				Action = "HandleAddressSave",
				City = payload.Request.Body.City,
				Foo = payload.ConfigData.Foo
			};
		}

		[HttpPost("webhook/saveaddress/protected-by-service")]
		public async Task<object> ProtectedByService([FromBody] WebhookPayloads.Addresses.Save payload)
		{
			var options = new OrderCloudWebhookAuthOptions()
			{
				HashKey = _settings.OrderCloudSettings.WebhookHashKey
			};
			await _service.VerifyWebhookHashAsync(options);
			return new
			{
				Action = "HandleAddressSave",
				City = payload.Request.Body.City,
				Foo = payload.ConfigData.Foo
			};
		}

		[HttpPost("webhook/response-testing/{allowed}"), OrderCloudWebhookAuth]
		public PreWebhookResponse WebhookRepsonseTesting([FromRoute] bool allowed, [FromBody] object payload = null)
		{
			if (allowed == true)
			{
				return PreWebhookResponse.Proceed();
			}
			if (payload == null)
			{
				return PreWebhookResponse.Block();
			}
			return PreWebhookResponse.Block(payload);
		}
	}
}
