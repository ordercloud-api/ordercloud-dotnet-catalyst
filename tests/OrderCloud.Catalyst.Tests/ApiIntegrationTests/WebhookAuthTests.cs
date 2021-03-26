using System.Threading.Tasks;
using Flurl.Http;
using NUnit.Framework;
using FluentAssertions;
using System.Net;
using OrderCloud.SDK;
using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System;
using System.Net.Http;
using AutoFixture;
using OrderCloud.Catalyst.TestApi;

namespace OrderCloud.Catalyst.Tests
{
	public class WebhookAuthTests
	{
		[Test]
		public async Task hash_is_authenticated()
		{
			Fixture fixture = new Fixture();
			var payload = fixture.Create<WebhookPayloads.Addresses.Save>();
			payload.ConfigData = new { Foo = "blah" };

			dynamic resp = await SendWebhookReq("webhook/saveaddress", payload).ReceiveJson();

			Assert.AreEqual(resp.Action, "HandleAddressSave");
			Assert.AreEqual(resp.City, payload.Request.Body.City);
			Assert.AreEqual(resp.Foo, "blah");
		}

		[Test]
		public async Task hash_does_not_match()
		{
			Fixture fixture = new Fixture();
			var payload = fixture.Create<WebhookPayloads.Addresses.Save>();

			var resp = await SendWebhookReq("webhook/saveaddress", payload, "dfadasfd");
			Assert.AreEqual(401, resp.StatusCode);
		}

		[TestCase(true)]
		[TestCase(false)]
		public async Task prewebhook_is_blocked_or_not(bool proceed)
		{
			var resp = await SendWebhookReq($"webhook/response-testing/{proceed}", null);
			var json = await resp.GetJsonAsync<PreWebhookResponse>();
			Assert.AreEqual(false, json.proceed);
			Assert.AreEqual(null, json.body);
		}

		[TestCase("something")]
		public async Task prewebhook_returns_body(object body)
		{
			var resp = await SendWebhookReq($"webhook/response-testing/false", body);
			var json = await resp.GetJsonAsync<PreWebhookResponse>();
			Assert.AreEqual(body, json.body);
		}

		private async Task<IFlurlResponse> SendWebhookReq(string route, object payload, string hashKey = null)
		{
			var _settings = new TestSettings();
			var json = JsonConvert.SerializeObject(payload);
			var keyBytes = Encoding.UTF8.GetBytes(hashKey ?? _settings.OrderCloudSettings.WebhookHashKey);
			var dataBytes = Encoding.UTF8.GetBytes(json);
			var hash = new HMACSHA256(keyBytes).ComputeHash(dataBytes);
			var base64 = Convert.ToBase64String(hash);

			return await TestFramework.Client
				.Request(route)
				.WithHeader("X-oc-hash", base64)
				.PostJsonAsync(payload);
		}
	}
}
