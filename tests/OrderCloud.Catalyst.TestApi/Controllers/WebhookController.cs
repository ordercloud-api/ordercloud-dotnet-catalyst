using Microsoft.AspNetCore.Mvc;
using OrderCloud.Catalyst;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst.TestApi
{
	public class WebhookController : CatalystController
	{
		[HttpPost("webhook/saveaddress"), OrderCloudWebhookAuth]
		public object HandleAddressSave([FromBody] WebhookPayloads.Addresses.Save payload)
		{
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
