using AutoFixture;
using AutoFixture.NUnit3;
using NUnit.Framework;
using OrderCloud.Catalyst.Auth.UserAuth;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Tests
{
	[TestFixture]
	public class TokenTests
	{
		[Test]
		public void should_be_able_to_serialze_and_deserialize_tokens()
		{
			var fixture = new Fixture();

			var keyID = fixture.Create<string>();
			var clientID = fixture.Create<string>();
			var roles = fixture.Create<List<string>>();
			var username = fixture.Create<string>();
			var anonOrderID = fixture.Create<string>();
			var authUrl = fixture.Create<string>();
			var apiUrl = fixture.Create<string>();
			var userType = "admin";
			var userDatabaseID = fixture.Create<string>();
			var impersonatingUserDatabaseID = fixture.Create<string>();
			var exp = new DateTime(2010, 4, 14, 0, 0, 0, DateTimeKind.Utc);
			var nvb = new DateTime(2000, 6, 1, 0, 0, 0, DateTimeKind.Utc);

			var raw = FakeOrderCloudToken.Create(clientID, roles, exp, nvb, username, keyID, anonOrderID, authUrl, apiUrl, userType, userDatabaseID, impersonatingUserDatabaseID);
			var jwt = new UserContext(raw);

			Assert.AreEqual(raw, jwt.AccessToken);
			Assert.AreEqual(keyID, jwt.KeyID);
			Assert.AreEqual(anonOrderID, jwt.AnonOrderID);
			Assert.AreEqual(username, jwt.Username);
			Assert.AreEqual(roles, jwt.Roles);
			Assert.AreEqual(authUrl, jwt.AuthUrl);
			Assert.AreEqual(apiUrl, jwt.ApiUrl);
			Assert.AreEqual(CommerceRole.Seller, jwt.CommerceRole);
			Assert.AreEqual(clientID, jwt.ClientID);
			Assert.AreEqual(userDatabaseID, jwt.UserDatabaseID);
			Assert.AreEqual(impersonatingUserDatabaseID, jwt.ImpersonatingUserDatabaseID);
			Assert.AreEqual(Truncate(exp), Truncate(jwt.ExpiresUTC));
			Assert.AreEqual(Truncate(nvb), Truncate(jwt.NotValidBeforeUTC));
		}

		[Test]
		[AutoData]
		public void create_fake_should_have_correct_defaults(string clientID)
		{
			var raw = FakeOrderCloudToken.Create(clientID);
			var jwt = new UserContext(raw);

			Assert.AreEqual(raw, jwt.AccessToken);
			Assert.AreEqual(new List<string>(), jwt.Roles);
			Assert.AreEqual("mockdomain.com", jwt.AuthUrl);
			Assert.AreEqual("mockdomain.com", jwt.ApiUrl);
			Assert.AreEqual(Truncate(DateTime.UtcNow), Truncate(jwt.NotValidBeforeUTC));
			Assert.AreEqual(Truncate(DateTime.UtcNow.AddMinutes(30)), Truncate(jwt.ExpiresUTC));

			Assert.AreEqual(null, jwt.KeyID);
			Assert.AreEqual(null, jwt.AnonOrderID);
			Assert.AreEqual(null, jwt.Username);
			Assert.AreEqual(CommerceRole.Seller, jwt.CommerceRole);
			Assert.AreEqual(clientID, jwt.ClientID);
			Assert.AreEqual(null, jwt.UserDatabaseID);
			Assert.AreEqual(null, jwt.ImpersonatingUserDatabaseID);
		}

		public static DateTime? Truncate(DateTime? dateTime)
		{
			if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue) return dateTime; // do not modify "guard" values
			return dateTime?.AddTicks(-((dateTime?.Ticks ?? 0) % TimeSpan.FromSeconds(10).Ticks));
		}
	}
}
