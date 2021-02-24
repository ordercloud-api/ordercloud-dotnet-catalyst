using System.Threading.Tasks;
using Flurl.Http;
using NUnit.Framework;
using FluentAssertions;
using System.Net;
using OrderCloud.SDK;
using System.Text;
using Newtonsoft.Json;
using OrderCloud.DemoWebApi;
using OrderCloud.DemoWebApi.Tests;
using System.Security.Cryptography;
using System;
using System.Net.Http;
using AutoFixture;

namespace OrderCloud.DemoWebApi.Tests
{
	public class WebhookAuthTests
	{
		[Test]
		public async Task hash_is_authenticated()
		{
			Fixture fixture = new Fixture();
			var payload = fixture.Create<WebhookPayloads.Addresses.Save>();
			payload.ConfigData = new { Foo = "blah" };

			dynamic resp = await SendWebhookReq(payload).ReceiveJson(); //SendWebhookReq(payload).ReceiveJson();

			Assert.AreEqual(resp.Action, "HandleAddressSave");
			Assert.AreEqual(resp.City, payload.Request.Body.City);
			Assert.AreEqual(resp.Foo, "blah");
		}

		[Test]
		public async Task hash_does_not_match()
		{
			Fixture fixture = new Fixture();
			var payload = fixture.Create<WebhookPayloads.Addresses.Save>();

			var resp = await SendWebhookReq(payload, "dfadasfd");  //SendWebhookReq(payload, "dfadasfd");
			resp.ShouldHaveStatusCode(401);
		}

		private async Task<HttpResponseMessage> SendWebhookReq(object payload, string hashKey = null)
		{
			var _settings = new AppSettings();
			var json = JsonConvert.SerializeObject(payload);
			var keyBytes = Encoding.UTF8.GetBytes(hashKey ?? _settings.WebhookHashKey);
			var dataBytes = Encoding.UTF8.GetBytes(json);
			var hash = new HMACSHA256(keyBytes).ComputeHash(dataBytes);
			var base64 = Convert.ToBase64String(hash);

			return await TestFramework.Client
				.Request("webhook/saveaddress")
				.WithHeader("X-oc-hash", base64)
				.PostJsonAsync(payload);
		}
	}
}
