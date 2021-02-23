using System.Threading.Tasks;
using Flurl.Http;
using OrderCloud.Catalyst;
using NUnit.Framework;
using OrderCloud.TestWebApi;
using FluentAssertions;
using System.Net;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using System;

namespace OrderCloud.DemoWebApi.Tests
{
	[TestFixture]
	public class IntegrationTests
	{
		[Test]
		public async Task can_allow_anonymous() {
			var result = await CreateServer()
				.CreateFlurlClient()
				.Request("demo/anon")
				.GetStringAsync();

			result.Should().Be("hello anon!");
		}

		[Test]
		public async Task should_deny_access_without_oc_token() {
			var resp = await CreateServer()
				.CreateFlurlClient()
				.AllowAnyHttpStatus()
				.Request("demo/shop")
				.GetAsync();

			resp.StatusCode.Should().Be((HttpStatusCode)401);
		}

		[Test]
		public async Task can_auth_with_oc_token() {
			var result = await CreateServer()
				.CreateFlurlClient()
				.WithFakeOrderCloudToken("mYcLiEnTiD") // check should be case insensitive
				.Request("demo/shop")
				.GetStringAsync();

			result.Should().Be("hello shopper!");
		}

		[Test]
		public async Task can_get_username_from_verified_user()
		{
			var result = await CreateServer()
				.CreateFlurlClient()
				.WithFakeOrderCloudToken("mYcLiEnTiD") // check should be case insensitive
				.Request("demo/username")
				.GetStringAsync();

			result.Should().Be("hello joe!");
		}

		[TestCase("demo/shop", true)]
		[TestCase("demo/admin", false)]
		[TestCase("demo/either", true)]
		[TestCase("demo/anybody", true)]
		[TestCase("demo/anon", true)]
		public async Task can_authorize_by_role(string endpoint, bool success)
		{
			var resp = await CreateServer()
				.CreateFlurlClient()
				.AllowAnyHttpStatus()
				.WithFakeOrderCloudToken("myclientid")
				.Request(endpoint)
				.GetAsync();

			resp.StatusCode.Should().Be((HttpStatusCode)(success ? 200 : 403));
		}

        [Test]
        public async Task can_disambiguate_webhook()
        {
			var payload = new
            {
                Route = "v1/buyers/{buyerID}/addresses/{addressID}",
                Verb = "PUT",
                Request = new { Body = new { City = "Minneapolis" } },
                ConfigData = new { Foo = "blah" }
            };

            var json = JsonConvert.SerializeObject(payload);
            var keyBytes = Encoding.UTF8.GetBytes("myhashkey");
            var dataBytes = Encoding.UTF8.GetBytes(json);
            var hash = new HMACSHA256(keyBytes).ComputeHash(dataBytes);
            var base64 = Convert.ToBase64String(hash);

            dynamic resp = await CreateServer()
                .CreateFlurlClient()
                .Request("demo/webhook")
                .WithHeader("X-oc-hash", base64)
                .PostJsonAsync(payload)
                .ReceiveJson();

            Assert.AreEqual(resp.Action, "HandleAddressSave");
            Assert.AreEqual(resp.City, "Minneapolis");
            Assert.AreEqual(resp.Foo, "blah");
        }

        private TestServer CreateServer() {
			return new TestServer(CatalystWebHostBuilder.CreateWebHostBuilder<TestStartup>(new string[] { }));
		}
	}

	public static class TestServerExtensions
	{
		public static IFlurlClient CreateFlurlClient(this TestServer server) 
		{
			server.AllowSynchronousIO = true;
			return new FlurlClient(server.CreateClient());
		}
	}
}
