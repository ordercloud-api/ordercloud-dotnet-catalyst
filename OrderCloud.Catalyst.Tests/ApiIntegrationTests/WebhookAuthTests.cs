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
using Newtonsoft.Json.Linq;

namespace OrderCloud.Catalyst.Tests
{
	public class WebhookAuthTests
	{
		[Test]
		[TestCase("webhook/saveaddress/protected-by-service")]
		[TestCase("webhook/saveaddress/protected-by-attribute")]
		public async Task hash_is_authenticated(string route)
		{
			Fixture fixture = new Fixture();
			var payload = fixture.Create<WebhookPayloads.Addresses.Save>();
			//var payload = new WebhookPayloads.Addresses.Save();

			payload.ConfigData = new { Foo = "blah" };

			dynamic resp = await SendWebhookReq(route, payload).ReceiveJson();

			Assert.AreEqual(resp.Action, "HandleAddressSave");
			Assert.AreEqual(resp.City, payload.Request.Body.City);
			Assert.AreEqual(resp.Foo, "blah");
		}

		[Test]
		[TestCase("webhook/saveaddress/protected-by-service")]
		[TestCase("webhook/saveaddress/protected-by-attribute")]
		public async Task hash_does_not_match(string route)
		{
			Fixture fixture = new Fixture();
			var payload = fixture.Create<WebhookPayloads.Addresses.Save>();

			var resp = await SendWebhookReq(route, payload, "dfadasfd");
			await resp.ShouldHaveFirstApiError("Unauthorized", 401, "X-oc-hash header does not match. Endpoint can only be hit from a valid OrderCloud webhook.");
		}

		[TestCase(true)]
		[TestCase(false)]
		public async Task prewebhook_is_blocked_or_not(bool proceed)
		{
			Fixture fixture = new Fixture();
			var payload = fixture.Create<ExampleModel>();

			var resp = await SendWebhookReq($"webhook/response-testing/{proceed}", payload);
			var json = await resp.GetJsonAsync<PreWebhookResponse>();
			Assert.AreEqual(proceed, json.proceed);
			if (proceed)
			{
				Assert.AreEqual(null, json.body);
			}
			else
			{
				Assert.AreEqual(payload.BoundedString, ((JObject) json.body).GetValue("BoundedString").ToObject<string>());
				Assert.AreEqual(payload.RequiredField, ((JObject)json.body).GetValue("RequiredField").ToObject<string>());
			}
		}

		[TestCase("something")]
		public async Task prewebhook_returns_body(object body)
		{
			var resp = await SendWebhookReq($"webhook/response-testing/false", body);
			var json = await resp.GetJsonAsync<PreWebhookResponse>();
			Assert.AreEqual(body, json.body);
		}

		[Test]
		public async Task fails_without_any_header()
		{
			Fixture fixture = new Fixture();
			var payload = fixture.Create<WebhookPayloads.Addresses.Save>();

			var resp = await TestFramework.Client
				.Request("webhook/saveaddress/protected-by-attribute")
				.PostJsonAsync(payload);

			await resp.ShouldHaveFirstApiError("Unauthorized", 401, "X-oc-hash header does not match. Endpoint can only be hit from a valid OrderCloud webhook.");
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
