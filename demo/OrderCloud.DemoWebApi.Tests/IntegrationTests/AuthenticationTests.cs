using FluentAssertions;
using Flurl.Http;
using NSubstitute;
using NUnit.Framework;
using OrderCloud.Catalyst;
using OrderCloud.SDK;
using OrderCloud.TestWebApi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.DemoWebApi.Tests
{
	[TestFixture]
	class AuthenticationTests
	{
		[Test]
		public async Task can_allow_anonymous()
		{
			var result = await TestFramework.Client
				.Request("demo/anon")
				.GetStringAsync();

			result.Should().Be("hello anon!");
		}

		[Test]
		public async Task should_deny_access_without_oc_token()
		{
			var resp = await TestFramework.Client
				.Request("demo/shop")
				.GetAsync();

			resp.ShouldBeApiError("InvalidToken", 401, "Access token is invalid or expired.");
		}

		[Test]
		public async Task can_auth_with_oc_token()
		{
			var result = await TestFramework.Client
				.WithFakeOrderCloudToken("mYcLiEnTiD") // check should be case insensitive
				.Request("demo/shop")
				.GetStringAsync();

			result.Should().Be("hello shopper!");
		}

		[Test]
		public async Task can_get_username_from_verified_user()
		{
			var result = await TestFramework.Client
				.WithFakeOrderCloudToken("mYcLiEnTiD") // check should be case insensitive
				.Request("demo/username")
				.GetStringAsync();

			result.Should().Be("hello joe!");
		}

		[Test]
		public async Task user_authorization_is_cached()
		{
			var token = FakeOrderCloudToken.Create("a_fake_client_id");
			var request = TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/shop");
			TestStartup.OC.ClearReceivedCalls();
			// Two back-to-back requests 
			await request.GetAsync();
			await request.GetAsync();

			// But exactly one request to Ordercloud
			TestStartup.OC.Received(1).Me.GetAsync(token);
		}

		[TestCase("demo/shop", true)]
		[TestCase("demo/admin", false)]
		[TestCase("demo/either", true)]
		[TestCase("demo/anybody", true)]
		[TestCase("demo/anon", true)]
		public async Task can_authorize_by_role(string endpoint, bool success)
		{
			var resp = await TestFramework.Client
				.WithFakeOrderCloudToken("myclientid")
				.Request(endpoint)
				.GetAsync();

			if (success)
			{
				resp.ShouldHaveStatusCode(200);
			} else
			{
				resp.ShouldBeApiError("InsufficientRoles", 403, "User does not have role(s) required to perform this action.");
				var roles = (await resp.DeserializeAsync<ApiError<InsufficientRolesError>>()).Data.SufficientRoles;
				Assert.AreEqual("OrderAdmin", roles[0]);
			}	
		}

		//[Test]
		//public async Task can_disambiguate_webhook() {
		//	var payload = new {
		//		Route = "v1/buyers/{buyerID}/addresses/{addressID}",
		//		Verb = "PUT",
		//		Request = new { Body = new { City = "Minneapolis" } },
		//		ConfigData = new { Foo = "blah" }
		//	};

		//	//var json = JsonConvert.SerializeObject(payload);
		//	//var keyBytes = Encoding.UTF8.GetBytes("myhashkey");
		//	//var dataBytes = Encoding.UTF8.GetBytes(json);
		//	//var hash = new HMACSHA256(keyBytes).ComputeHash(dataBytes);
		//	//var base64 = Convert.ToBase64String(hash);

		//	dynamic resp = await CreateServer()
		//		.CreateFlurlClient()
		//		.Request("demo/webhook")
		//		.WithHeader("X-oc-hash", "4NPw1O9AviSOC1A3C+HqkDutRLNwyABneY/3M2OqByE=")
		//		.PostJsonAsync(payload)
		//		.ReceiveJson();

		//	Assert.AreEqual(resp.Action, "HandleAddressSave");
		//	Assert.AreEqual(resp.City, "Minneapolis");
		//	Assert.AreEqual(resp.Foo, "blah");
		//}
	}
}
