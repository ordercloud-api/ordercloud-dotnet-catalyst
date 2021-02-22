using System.Threading.Tasks;
using Flurl.Http;
using OrderCloud.Catalyst.Tests.TestingHelpers;
using NUnit.Framework;
using FluentAssertions;
using System.Net;
using Microsoft.AspNetCore.TestHost;

namespace OrderCloud.Catalyst.Tests
{
	[TestFixture]
	public class OCUserAuthTests
	{
		private TestService _service;

		[SetUp]
		public void Setup()
        {
			_service = new TestService();
        }

        [Test]
		public async Task can_allow_anonymous() {
			var result = await _service.CreateServer()
				.CreateFlurlClient()
				.Request("demo/anon")
				.GetStringAsync();

			result.Should().Be("hello anon!");
		}

		[Test]
		public async Task should_deny_access_without_oc_token() {
			var resp = await _service.CreateServer()
				.CreateFlurlClient()
				.AllowAnyHttpStatus()
				.Request("demo/shop")
				.GetAsync();

			resp.StatusCode.Should().Be((HttpStatusCode)401);
		}

		[Test]
		public async Task can_auth_with_oc_token() {
			var result = await _service.CreateRequestWithToken("demo/shop", "mYcLiEnTiD").GetStringAsync();
			result.Should().Be("hello shopper!");
		}

		[Test]
		public async Task can_get_username_from_verified_user()
		{
			var result = await _service.CreateRequestWithToken("demo/username", "mYcLiEnTiD").GetStringAsync();
			result.Should().Be("hello joe!");
		}

		[TestCase("demo/shop", true)]
		[TestCase("demo/admin", false)]
		[TestCase("demo/either", true)]
		[TestCase("demo/anybody", true)]
		[TestCase("demo/anon", true)]
		public async Task can_authorize_by_role(string endpoint, bool success)
		{
			var resp = await _service.CreateServer()
				.CreateFlurlClient()
				.AllowAnyHttpStatus()
				.WithFakeOrderCloudToken("myclientid")
				.Request(endpoint)
				.GetAsync();

			resp.StatusCode.Should().Be((HttpStatusCode)(success ? 200 : 403));
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
