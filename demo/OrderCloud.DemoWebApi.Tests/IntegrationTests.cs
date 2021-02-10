using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using OrderCloud.Catalyst;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using OrderCloud.SDK;
using OrderCloud.TestWebApi;
using FluentAssertions;
using System.Net;

namespace OrderCloud.DemoWebApi.Tests
{
	[TestFixture]
	public class IntegrationTests
	{
		[Test]
		public async Task can_allow_anonymous() {
			var result = await CreateServer()
				.CreateFlurlClient()
				.Request("test/anon")
				.GetStringAsync();

			result.Should().Be("hello anon!");
		}

		[Test]
		public async Task should_deny_access_without_oc_token() {
			var resp = await CreateServer()
				.CreateFlurlClient()
				.AllowAnyHttpStatus()
				.Request("test/shop")
				.GetAsync();

			resp.StatusCode.Should().Be((HttpStatusCode)401);
		}

		[Test]
		public async Task can_auth_with_oc_token() {
			var result = await CreateServer()
				.CreateFlurlClient()
				.WithFakeOrderCloudToken("mYcLiEnTiD") // check should be case insensitive
				.Request("test/shop")
				.GetStringAsync();

			result.Should().Be("hello shopper!");
		}

		[TestCase("test/shop", true)]
		[TestCase("test/admin", false)]
		[TestCase("test/either", true)]
		[TestCase("test/anybody", true)]
		[TestCase("test/anon", true)]
		public async Task can_authorize_by_role(string endpoint, bool success) {
			var resp = await CreateServer()
				.CreateFlurlClient()
				.AllowAnyHttpStatus()
				.WithFakeOrderCloudToken("myclientid")
				.Request(endpoint)
				.GetAsync();

			resp.StatusCode.Should().Be((HttpStatusCode)(success ? 200 : 403));
		}

		[Test]
		public async Task should_deny_access_with_incorrect_client_id() {
			var resp = await CreateServer()
				.CreateFlurlClient()
				.AllowAnyHttpStatus()
				.WithFakeOrderCloudToken("wrongid")
				.Request("test/shop")
				.GetAsync();

			resp.StatusCode.Should().Be((HttpStatusCode)401);
		}

		[Test]
		public async Task can_disambiguate_webhook() {
			var payload = new {
				Route = "v1/buyers/{buyerID}/addresses/{addressID}",
				Verb = "PUT",
				Request = new { Body = new { City = "Minneapolis" } },
				ConfigData = new { Foo = "blah" }
			};

			//var json = JsonConvert.SerializeObject(payload);
			//var keyBytes = Encoding.UTF8.GetBytes("myhashkey");
			//var dataBytes = Encoding.UTF8.GetBytes(json);
			//var hash = new HMACSHA256(keyBytes).ComputeHash(dataBytes);
			//var base64 = Convert.ToBase64String(hash);

			dynamic resp = await CreateServer()
				.CreateFlurlClient()
				.Request("test/webhook")
				.WithHeader("X-oc-hash", "4NPw1O9AviSOC1A3C+HqkDutRLNwyABneY/3M2OqByE=")
				.PostJsonAsync(payload)
				.ReceiveJson();

			Assert.AreEqual(resp.Action, "HandleAddressSave");
			Assert.AreEqual(resp.City, "Minneapolis");
			Assert.AreEqual(resp.Foo, "blah");
		}

		private TestServer CreateServer() {
			return new TestServer(Program.ConfigureWebHostBuilder<TestStartup>(new string[] {}));
		}

		private class TestStartup : Startup
		{
			public TestStartup(IConfiguration configuration) : base(configuration) { }

			public override void ConfigureServices(IServiceCollection services) {
				// first do real service registrations
				base.ConfigureServices(services);

				// then replace some of them with fakes
				var oc = Substitute.For<IOrderCloudClient>();
				oc.Me.GetAsync(Arg.Any<string>()).Returns(new MeUser { Username = "joe", AvailableRoles = new[] { "Shopper" } });
				services.AddSingleton(oc);
			}
		}
	}

	public static class TestServerExtensions
	{
		public static IFlurlClient CreateFlurlClient(this TestServer server) => new FlurlClient(server.CreateClient());
	}
}
