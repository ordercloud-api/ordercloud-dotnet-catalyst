using FluentAssertions;
using Flurl.Http;
using NSubstitute;
using NUnit.Framework;
using OrderCloud.Catalyst;
using OrderCloud.Catalyst.TestApi;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests
{
	[TestFixture]
	class UserAuthTests
	{
		[Test]
		public async Task can_allow_anonymous()
		{
			var result = await TestFramework.Client
				.Request("demo/anon")
				.GetStringAsync();

			result.Should().Be("\"hello anon!\"");
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

			result.Should().Be("\"hello shopper!\"");
		}

		[Test]
		public async Task should_succeed_with_custom_role()
		{
			var token = FakeOrderCloudToken.Create("some_client_id");
			var request = TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/custom");

			TestStartup.oc.Me.GetAsync(token).Returns(new MeUser { Username = "joe", ID = "", Active = true, AvailableRoles = new[] { "CustomRole" } });

			var result = await request.GetStringAsync();

			result.Should().Be("\"hello custom!\"");
		}

		[Test]
		public async Task should_error_without_custom_role()
		{
			var result = await TestFramework.Client
				.WithFakeOrderCloudToken("mYcLiEnTiD") // check should be case insensitive
				.Request("demo/custom")
				.GetAsync();

			Assert.AreEqual(403, result.StatusCode);
		}

		[Test]
		public async Task can_get_username_from_verified_user()
		{
			var result = await TestFramework.Client
				.WithFakeOrderCloudToken("mYcLiEnTiD") // check should be case insensitive
				.Request("demo/username")
				.GetStringAsync();

			result.Should().Be("\"hello joe!\"");
		}

		[Test]
		public async Task should_succeed_with_order_admin()
		{
			var token = FakeOrderCloudToken.Create("some_new_client_id");
			var request = TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/admin");

			TestStartup.oc.Me.GetAsync(token).Returns(new MeUser
			{
				Username = "joe",
				ID = "",
				Active = true,
				AvailableRoles = new[] {
				"MeAdmin",
				"MeXpAdmin",
				"ProductAdmin",
				"PriceScheduleAdmin",
				"SupplierReader",
				"OrderAdmin",
				"SupplierAdmin",
				"SupplierUserAdmin",
				"MPMeSupplierUserAdmin",
				"MPSupplierUserGroupAdmin"
				}
			});

			var result = await request.GetStringAsync();

			result.Should().Be("\"hello admin!\"");
		}

		[Test]
		public async Task user_authorization_is_cached()
		{
			var token = FakeOrderCloudToken.Create("a_fake_client_id");
			var request = TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/shop");
			TestStartup.oc.ClearReceivedCalls();
			// Two back-to-back requests 
			await request.GetAsync();
			await request.GetAsync();

			// But exactly one request to Ordercloud
			TestStartup.oc.Received(1).Me.GetAsync(token);
		}

		[TestCase("demo/shop", true)]
		[TestCase("demo/admin", false)]
		[TestCase("demo/either", true)]
		[TestCase("demo/anybody", true)]
		[TestCase("demo/anon", true)]
		public async Task can_authorize_by_role(string endpoint, bool success)
		{
			var request = TestFramework.Client
					.WithFakeOrderCloudToken("myclientid")
					.Request(endpoint);

			if (success)
			{
				var response = await request.GetAsync();
				Assert.AreEqual(200, response.StatusCode);
			} else
			{
				var response = await request.GetAsync();
				var json = await request.GetJsonAsync<ApiError<InsufficientRolesError>>();
				response.ShouldBeApiError("InsufficientRoles", 403, "User does not have role(s) required to perform this action.");
				Assert.AreEqual("OrderAdmin", json.Data.SufficientRoles[0]);
			}	
		}

		[Test]
		public async Task can_get_verified_user_context_from_token()
		{
			var token = FakeOrderCloudToken.Create("sOmEcLiEnTiD");

			var result = await TestFramework.Client
				.Request($"demo/token/{token}")
				.PostAsync()
				.ReceiveJson<SimplifiedUser>();

			result.TokenClientID.Should().Be("sOmEcLiEnTiD");
			result.AvailableRoles[0].Should().Be("Shopper");
			result.Username.Should().Be("joe");
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
