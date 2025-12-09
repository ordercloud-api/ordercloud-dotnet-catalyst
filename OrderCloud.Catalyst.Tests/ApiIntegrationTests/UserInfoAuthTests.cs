using AutoFixture;
using AutoFixture.NUnit3;
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
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests
{
    [TestFixture]
    public class UserInfoAuthTests
    {

        [Test]
        public async Task should_deny_access_without_oc_token()
        {
            var resp = await TestFramework.Client
                .Request("demo/simpleuserinfo")
                .GetAsync();

            await resp.ShouldHaveFirstApiError("InvalidToken", 401, "Access token is invalid or expired.");
        }

        [Test]
        public async Task can_auth_with_oc_token()
        {
            var token = FakeUserInfoToken.Create();
            var result = await TestFramework.Client
                .WithOAuthBearerToken(token)
                .Request("demo/simpleuserinfo")
                .GetStringAsync();

            result.Should().Be("\"hello userinfo!\"");
        }

        [Test]
        public async Task should_succeed_with_custom_role()
        {
            var token = FakeUserInfoToken.Create(new List<string> { "CustomRole" });
            var request = TestFramework.Client
                .WithOAuthBearerToken(token)
                .Request("demo/customuserinfo");

            var result = await request.GetStringAsync();

            result.Should().Be("\"hello custom userinfo!\"");
        }

        [Test]
        public async Task should_succeed_with_with_full_access()
        {
            var token = FakeUserInfoToken.Create(new List<string> { "FullAccess" });
            var request = TestFramework.Client
                .WithOAuthBearerToken(token)
                .Request("demo/customuserinfo");

            var result = await request.GetStringAsync();

            result.Should().Be("\"hello custom userinfo!\"");
        }

        [Test]
        public async Task should_error_without_custom_role()
        {
            var token = FakeUserInfoToken.Create();
            var result = await TestFramework.Client
                .WithOAuthBearerToken(token)
                .Request("demo/customuserinfo")
                .GetAsync();

            Assert.AreEqual(403, result.StatusCode);
        }

        [Test]
        public async Task can_get_user_info_context_from_auth()
        {
            var fixture = new Fixture();
            var username = fixture.Create<string>();
            var token = FakeUserInfoToken.Create(new List<string> { "Shopper" }, username: username);

            var result = await TestFramework.Client
                .WithOAuthBearerToken(token)
                .Request("demo/userinfocontext")
                .GetJsonAsync<SimplifiedUser>();

            Assert.AreEqual(username, result.Username);
            Assert.AreEqual("Shopper", result.AvailableRoles[0]);
        }

        [Test]
        public async Task can_get_user_context_from_setting_it()
        {
            var fixture = new Fixture();
            var username = fixture.Create<string>();
            var token = FakeUserInfoToken.Create(new List<string> { "Shopper" }, username: username);

            var result = await TestFramework.Client
                .Request($"demo/userinfocontext/{token}")
                .PostAsync()
                .ReceiveJson<SimplifiedUser>();

            Assert.AreEqual(username, result.Username);
            Assert.AreEqual("Shopper", result.AvailableRoles[0]);
        }

        [Test]
        public async Task should_succeed_if_now_is_between_expiry_and_nvb()
        {
            var token = FakeUserInfoToken.Create(
                roles: new List<string> { "Shopper" },
                expiresUTC: DateTime.UtcNow + TimeSpan.FromHours(1),
                notValidBeforeUTC: DateTime.UtcNow - TimeSpan.FromHours(1)
            );

            var resp = await TestFramework.Client
                .WithOAuthBearerToken(token)
                .Request("demo/simpleuserinfo")
                .GetStringAsync();

            resp.Should().Be("\"hello userinfo!\"");
        }

        [Test]
        public async Task should_deny_access_if_nvb_is_wrong()
        {
            var fixture = new Fixture();

            var token = FakeUserInfoToken.Create(
                roles: new List<string> { "Shopper" },
                expiresUTC: DateTime.UtcNow + TimeSpan.FromHours(2),
                notValidBeforeUTC: DateTime.UtcNow + TimeSpan.FromHours(1)
            );

            var resp = await TestFramework.Client
                .WithOAuthBearerToken(token)
                .Request("demo/simpleuserinfo")
                .GetAsync();

            await resp.ShouldHaveFirstApiError("InvalidToken", 401, "Access token is invalid or expired.");
        }

        [Test]
        public async Task should_deny_access_if_past_expiry()
        {
            var fixture = new Fixture();

            var token = FakeUserInfoToken.Create(
                roles: new List<string> { "Shopper" },
                expiresUTC: DateTime.UtcNow,
                notValidBeforeUTC: DateTime.UtcNow - TimeSpan.FromHours(1)
            );

            var resp = await TestFramework.Client
                .WithOAuthBearerToken(token)
                .Request("demo/simpleuserinfo")
                .GetAsync();

            await resp.ShouldHaveFirstApiError("InvalidToken", 401, "Access token is invalid or expired.");
        }

        [Test]
        public async Task two_requests_with_the_same_kid_should_verify_both_tokens()
        {
            var fixture = new Fixture();
            var keyID = fixture.Create<string>();

            var token1 = FakeUserInfoToken.Create(new List<string> { "Shopper" }, keyID: keyID);
            var token2 = token1 + "makethisinvalid";

            var response1 = await TestFramework.Client.WithOAuthBearerToken(token1).Request("demo/simpleuserinfo").GetAsync();
            var response2 = await TestFramework.Client.WithOAuthBearerToken(token2).Request("demo/simpleuserinfo").GetAsync();

            await response2.ShouldHaveFirstApiError("InvalidToken", 401, "Access token is invalid or expired.");
        }



        [Test]
        public async Task user_auth_provider_handles_mulitple_concurrent_requests()
        {
            var fixture = new Fixture();
            var requestCount = 10;
            var usernames = new List<string>();
            var requests = new List<Task<string>>();

            foreach (var i in Enumerable.Range(0, requestCount))
            {
                var username = fixture.Create<string>();
                usernames.Add(username);
                var token = FakeUserInfoToken.Create(username: username);
                var request = TestFramework.Client.WithOAuthBearerToken(token).Request("demo/userinfousername").GetStringAsync();
                requests.Add(request);
            }

            var results = await Task.WhenAll(requests);

            foreach (var i in Enumerable.Range(0, requestCount))
            {
                Assert.AreEqual("\"" + usernames[i] + "\"", results[i]);
            }
        }
    }
}
