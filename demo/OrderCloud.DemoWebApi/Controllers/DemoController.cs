using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderCloud.Catalyst;
using OrderCloud.SDK;

namespace SampleApp.WebApi.Controllers
{
	[Route("demo")]
	public class DemoController : Controller
	{
		[HttpGet("shop"), OrderCloudUserAuth(ApiRole.Shopper)]
		public object Shop() => "hello shopper!";

		[HttpGet("admin"), OrderCloudUserAuth(ApiRole.OrderAdmin)]
		public object Admin() => "hello admin!";

		[HttpGet("either"), OrderCloudUserAuth(ApiRole.Shopper, ApiRole.OrderAdmin)]
		public object Either() => "hello either!";

		[HttpGet("anybody"), OrderCloudUserAuth]
		public object Anybody() => "hello anybody!";

		[HttpGet("anon")]
		public object Anon() => "hello anon!";

		//[Route("webhook"), OrderCloudWebhookAuth]
		//public object HandleAddressSave([FromBody] WebhookPayloads.Addresses.Save<MyConfigData> payload) {
		//	return new {
		//		Action = "HandleAddressSave",
		//		City = payload.Request.Body.City,
		//		Foo = payload.ConfigData.Foo
		//	};
		//}

		//[Route("webhook"), OrderCloudWebhookAuth]
		//public object HandleGenericWebhook([FromBody] WebhookPayload payload) {
		//	return new {
		//		Action = "HandleGenericWebhook",
		//		City = payload.Request.Body.City,
		//		Foo = payload.ConfigData.Foo
		//	};
		//}
	}

	public class MyConfigData
	{
		public string Foo { get; set; }
		public int Bar { get; set; }
	}
}
