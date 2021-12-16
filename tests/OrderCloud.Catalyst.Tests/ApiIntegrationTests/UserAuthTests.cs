using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Flurl.Http;
using NSubstitute;
using NUnit.Framework;
using OrderCloud.Catalyst;
using OrderCloud.Catalyst.Auth.UserAuth;
using OrderCloud.Catalyst.TestApi;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests
{
	[TestFixture]
	public class UserAuthTests
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

			await resp.ShouldHaveFirstApiError("InvalidToken", 401, "Access token is invalid or expired.");
		}

		[Test]
		public async Task can_auth_with_oc_token()
		{
			var token = FakeOrderCloudToken.Create("mYcLiEnTiD", new List<string> { "Shopper" }); // clientID check should be case insensitive
			var result = await TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/shop")
				.GetStringAsync();

			result.Should().Be("\"hello shopper!\"");
		}

		[Test]
		public async Task should_succeed_with_custom_role()
		{
			var token = FakeOrderCloudToken.Create("mYcLiEnTiD", new List<string> { "CustomRole" });
			var request = TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/custom");

			var result = await request.GetStringAsync();

			result.Should().Be("\"hello custom!\"");
		}

		[Test]
		public async Task should_succeed_with_with_full_access()
		{
			var token = FakeOrderCloudToken.Create("mYcLiEnTiD", new List<string> { "FullAccess" });
			var request = TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/admin");

			var result = await request.GetStringAsync();

			result.Should().Be("\"hello admin!\"");
		}

		[Test]
		public async Task should_error_without_custom_role()
		{
			var token = FakeOrderCloudToken.Create("mYcLiEnTiD");
			var result = await TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/custom")
				.GetAsync();

			Assert.AreEqual(403, result.StatusCode);
		}

		[Test]
		public async Task can_get_user_context_from_auth()
		{
			var fixture = new Fixture();
			var username = fixture.Create<string>();
			var clientID = fixture.Create<string>();
			var token = FakeOrderCloudToken.Create(clientID, new List<string> { "Shopper" }, username: username);

			var result = await TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/usercontext")
				.GetJsonAsync<SimplifiedUser>();

			Assert.AreEqual(username, result.Username);
			Assert.AreEqual(clientID, result.TokenClientID);
			Assert.AreEqual("Shopper", result.AvailableRoles[0]);
		}

		[Test]
		public async Task can_get_user_context_from_setting_it()
		{
			var fixture = new Fixture();
			var username = fixture.Create<string>();
			var clientID = fixture.Create<string>();
			var token = FakeOrderCloudToken.Create(clientID, new List<string> { "Shopper" }, username: username);

			var result = await TestFramework.Client
				.Request($"demo/usercontext/{token}")
				.PostAsync()
				.ReceiveJson<SimplifiedUser>();

			Assert.AreEqual(username, result.Username);
			Assert.AreEqual(clientID, result.TokenClientID);
			Assert.AreEqual("Shopper", result.AvailableRoles[0]);
		}

		[Test]
		public async Task should_succeed_if_now_is_between_expiry_and_nvb()
		{
			var fixture = new Fixture();

			var token = FakeOrderCloudToken.Create(
				clientID: fixture.Create<string>(),
				roles: new List<string> { "Shopper" },
				expiresUTC: DateTime.UtcNow + TimeSpan.FromHours(1),
				notValidBeforeUTC: DateTime.UtcNow - TimeSpan.FromHours(1)
			);

			var resp = await TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/shop")
				.GetStringAsync();

			resp.Should().Be("\"hello shopper!\"");
		}

		[Test]
		public async Task should_allow_access_if_no_client_id()
		{
			var token = FakeOrderCloudToken.Create(null, new List<string> { "Shopper" });

			var result = await TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/shop")
				.GetStringAsync();

			result.Should().Be("\"hello shopper!\"");
		}

		[Test]
		public async Task should_deny_access_if_nvb_is_wrong()
		{
			var fixture = new Fixture();

			var token = FakeOrderCloudToken.Create(
				clientID: fixture.Create<string>(),
				roles: new List<string> { "Shopper" },
				expiresUTC: DateTime.UtcNow + TimeSpan.FromHours(2),
				notValidBeforeUTC: DateTime.UtcNow + TimeSpan.FromHours(1)
			);

			var resp = await TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/shop")
				.GetAsync();

			await resp.ShouldHaveFirstApiError("InvalidToken", 401, "Access token is invalid or expired.");
		}

		[Test]
		public async Task should_have_specific_error_for_wrong_env()
		{
			var fixture = new Fixture();

			var token = FakeOrderCloudToken.Create(
				clientID: fixture.Create<string>(),
				apiUrl: "https://api.ordercloud.io"
			);

			var request =  TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/anybody");

			// Auth validation fails
			TestStartup.oc.Me.GetAsync(Arg.Any<string>()).Returns<MeUser>(x => { throw new Exception(); });

			var response = await request.GetAsync();

			await response.ShouldHaveFirstApiError("InvalidToken", 401, "Environment mismatch. Token gives access to https://api.ordercloud.io while this API expects ");
		}

		[Test]
		public async Task should_succeed_based_on_token_not_me_get()
		{
			var fixture = new Fixture();
			var clientID = fixture.Create<string>();
			var token = FakeOrderCloudToken.Create(clientID, new List<string> { "OrderAdmin" }); // token has the role

			var request = TestFramework.Client
				.WithOAuthBearerToken(token)
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
		public async Task should_deny_access_if_past_expiry()
		{
			var fixture = new Fixture();

			var token = FakeOrderCloudToken.Create(
				clientID: fixture.Create<string>(),
				roles: new List<string> { "Shopper" },
				expiresUTC: DateTime.UtcNow - TimeSpan.FromHours(1)
			);

			var resp = await TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/shop")
				.GetAsync();

			await resp.ShouldHaveFirstApiError("InvalidToken", 401, "Access token is invalid or expired.");
		}

		[TestCase(true, 401)]
		[TestCase(true, 403)]
		[TestCase(true, 500)]
		[TestCase(true, 404)]
		[TestCase(false, 401)]
		[TestCase(false, 403)]
		[TestCase(false, 500)]
		public async Task ordercloud_error_should_be_forwarded(bool useKid, int statusCode)
		{
			var fixture = new Fixture();
			var errorCode = fixture.Create<string>();
			var message = fixture.Create<string>();

			var keyID = useKid ? "something" : null;
			var token = FakeOrderCloudToken.Create("mYcLiEnTiD", new List<string> { "Shopper" }, keyID: keyID); // token has the role

			var request = TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/shop");

			var error = new OrderCloudException(message)
			{
				HttpStatus = (HttpStatusCode)statusCode,
				Errors = new ApiError[] {
					new ApiError() {
						Message = message,
						ErrorCode = errorCode,
					}
				}
			};

			TestStartup.oc.Me.GetAsync(Arg.Any<string>()).Returns<MeUser>(x => { throw error; });
			TestStartup.oc.Certs.GetPublicKeyAsync(Arg.Any<string>()).Returns<PublicKey>(x => { throw error; });

			var result = await request.GetAsync();

			await result.ShouldHaveFirstApiError(errorCode, statusCode, message);
		}

		[TestCase("demo/shop", true)]
		[TestCase("demo/admin", false)]
		[TestCase("demo/either", true)]
		[TestCase("demo/anybody", true)]
		[TestCase("demo/anon", true)]
		public async Task can_authorize_by_role(string endpoint, bool success)
		{
			var token = FakeOrderCloudToken.Create("mYcLiEnTiD", new List<string> { "Shopper" });

			var request = TestFramework.Client
					.WithOAuthBearerToken(token)
					.Request(endpoint);

			if (success)
			{
				var response = await request.GetAsync();
				Assert.AreEqual(200, response.StatusCode);
			}
			else
			{
				var response = await request.GetAsync();
				var json = await request.GetJsonAsync();
				await response.ShouldHaveFirstApiError("InsufficientRoles", 403, "User does not have role(s) required to perform this action.");
				Assert.AreEqual("OrderAdmin", json.Errors[0].Data.SufficientRoles[0]);
			}
		}

		[TestCase(true)]
		[TestCase(false)]
		public async Task user_authorization_is_cached(bool useKid)
		{
			var keyID = useKid ? "something" : null;
			var token = FakeOrderCloudToken.Create("mYcLiEnTiD", new List<string> { "Shopper" }, keyID: keyID);

			var request = TestFramework.Client.WithOAuthBearerToken(token).Request("demo/shop");

			TestStartup.oc.ClearReceivedCalls();

			// Two back-to-back requests 
			await request.GetAsync();
			await request.GetAsync();

			// But exactly one request to OrderCloud
			if (useKid)
			{
				await TestStartup.oc.Received(1).Certs.GetPublicKeyAsync(Arg.Any<string>());
			}
			else
			{
				await TestStartup.oc.Received(1).Me.GetAsync(Arg.Any<string>());
			}
		}

		[Test]
		public async Task user_auth_provider_handles_mulitple_concurrent_requests()
		{
			var requestCount = 10;
			var fixture = new Fixture();
			var clientIDs = new List<string>();
			var requests = new List<Task<string>>();

			foreach (var i in Enumerable.Range(0, requestCount))
			{
				var clientID = fixture.Create<string>();
				clientIDs.Add(clientID);
				var token = FakeOrderCloudToken.Create(clientID);
				var request = TestFramework.Client.WithOAuthBearerToken(token).Request("demo/clientid").GetStringAsync();
				requests.Add(request);
			}

			var results = await Task.WhenAll(requests);

			foreach (var i in Enumerable.Range(0, requestCount))
			{
				Assert.AreEqual("\"" + clientIDs[i] + "\"", results[i]);
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
