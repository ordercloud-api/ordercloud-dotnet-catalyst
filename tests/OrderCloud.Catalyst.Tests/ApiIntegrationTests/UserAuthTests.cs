using FluentAssertions;
using Flurl.Http;
using NSubstitute;
using NUnit.Framework;
using OrderCloud.Catalyst;
using OrderCloud.Catalyst.TestApi;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests
{
	[TestFixture]
	public class UserAuthTests
	{
		[SetUp]
		protected void SetUp()
		{
			var client = TestFramework.Client;
			TestStartup.oc.ClearReceivedCalls();
			TestStartup.oc.Me.GetAsync(Arg.Any<string>()).Returns(new MeUser { Username = "joe", Active = true, AvailableRoles = new[] { "Shopper" } });
		}

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
				.WithFakeOrderCloudToken("mYcLiEnTiD", new List<string> { "Shopper" }) // clientID check should be case insensitive
				.Request("demo/shop")
				.GetStringAsync();

			result.Should().Be("\"hello shopper!\"");
		}

		[Test]
		public async Task should_succeed_with_custom_role()
		{
			var request = TestFramework.Client
				.WithFakeOrderCloudToken("mYcLiEnTiD", new List<string> { "CustomRole" })
				.Request("demo/custom");

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

		[TestCase("awvcxdd", "sgdfhfgh")]
		public async Task can_get_user_context_from_auth(string userName, string clientID)
		{
			var jwt = new JwtOrderCloud() { Username = userName, ClientID = clientID, Roles = new List<string> { "Shopper" } };
			var result = await TestFramework.Client
				.WithFakeOrderCloudToken(jwt) // check should be case insensitive
				.Request("demo/usercontext")
				.GetJsonAsync<SimplifiedUser>();

			Assert.AreEqual(userName, result.Username);
			Assert.AreEqual(clientID, result.TokenClientID);
			Assert.AreEqual("Shopper", result.AvailableRoles[0]);

		}

		[TestCase("aqxcbiof", "yucnkqs")]
		public async Task can_get_user_context_from_setting_it(string userName, string clientID)
		{
			var jwt = new JwtOrderCloud() { Username = userName, ClientID = clientID, Roles = new List<string> { "Shopper" } };

			var result = await TestFramework.Client
				.Request($"demo/usercontext/{jwt.CreateFake()}")
				.PostAsync()
				.ReceiveJson<SimplifiedUser>();

			Assert.AreEqual(userName, result.Username);
			Assert.AreEqual(clientID, result.TokenClientID);
			Assert.AreEqual("Shopper", result.AvailableRoles[0]);
		}

		[TestCase("Auth.InvalidUsernameOrPassword", 401, "Invalid username or password")]
		[TestCase("InvalidToken", 401, "Access token is invalid or expired.")]
		public async Task ordercloud_error_should_be_forwarded(string errorCode, int statusCode, string message)
		{
			var request = TestFramework.Client
				.WithFakeOrderCloudToken("mYcLiEnTiD", new List<string> { "Shopper" })
				.Request("demo/shop");
			var error = OrderCloudExceptionFactory.Create((HttpStatusCode)statusCode, "", new ApiError[] { new ApiError() { 
				Message = message,
				ErrorCode = errorCode,
			}});

			TestStartup.oc.Me.GetAsync(Arg.Any<string>()).Returns<MeUser>(x => { throw error; });

			var result = await request.GetAsync();

			result.ShouldBeApiError(errorCode, statusCode, message);
		}

		public async Task auth_should_succeed_based_on_token_not_roles()
		{
			var request = TestFramework.Client
				.WithFakeOrderCloudToken("mYcLiEnTiD", new List<string>() { "OrderAdmin" }) // token has the role
				.Request("demo/admin");

			TestStartup.oc.Me.GetAsync(Arg.Any<string>()).Returns(new MeUser
			{
				Username = "joe",
				ID = "",
				Active = true,
				AvailableRoles = new[] { "MeAdmin", "MeXpAdmin" } // me get does not have the role
			});

			var result = await request.GetStringAsync();

			result.Should().Be("\"hello admin!\""); // auth should work
		}

		[Test]
		public async Task user_authorization_is_cached()
		{
			var request = TestFramework.Client
				.WithFakeOrderCloudToken("mYcLiEnTiD")
				.Request("demo/shop");

			// Two back-to-back requests 
			await request.GetAsync();
			await request.GetAsync();

			// But exactly one request to Ordercloud
			await TestStartup.oc.Received(1).Me.GetAsync(Arg.Any<string>());
		}

		[TestCase("demo/shop", true)]
		[TestCase("demo/admin", false)]
		[TestCase("demo/either", true)]
		[TestCase("demo/anybody", true)]
		[TestCase("demo/anon", true)]
		public async Task can_authorize_by_role(string endpoint, bool success)
		{
			var request = TestFramework.Client
					.WithFakeOrderCloudToken("mYcLiEnTiD", new List<string> { "Shopper" })
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
