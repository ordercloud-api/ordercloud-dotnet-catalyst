using Microsoft.AspNetCore.Mvc;
using OrderCloud.Catalyst;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst.TestApi
{
	public class WebhookController : BaseController
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
	}
}
