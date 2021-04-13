using AutoFixture;
using NUnit.Framework;
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
			var userType = fixture.Create<string>();
			var userDatabaseID = fixture.Create<string>();
			var companyDatabaseID = fixture.Create<string>();
			var sellerDatabaseID = fixture.Create<string>();
			var companyInteropID = fixture.Create<string>();
			var sellerInteropID = fixture.Create<string>();
			var marketplaceID = fixture.Create<string>();
			var issuedAtUTC = fixture.Create<DateTime>();
			var impersonatingUserDatabaseID = fixture.Create<string>();
			var exp = new DateTime(2010, 4, 14, 0, 0, 0, DateTimeKind.Utc);
			var nvb = new DateTime(2000, 6, 1, 0, 0, 0, DateTimeKind.Utc);

			var raw = JwtOrderCloud.CreateFake(clientID, roles, exp, nvb, username, keyID, anonOrderID, authUrl, apiUrl, userType, userDatabaseID, companyDatabaseID, sellerDatabaseID, companyInteropID, issuedAtUTC, sellerInteropID, marketplaceID, impersonatingUserDatabaseID);
			var jwt = new JwtOrderCloud(raw);

			Assert.AreEqual(raw, jwt.AccessToken);
			Assert.AreEqual(keyID, jwt.KeyID);
			Assert.AreEqual(anonOrderID, jwt.AnonOrderID);
			Assert.AreEqual(username, jwt.Username);
			Assert.AreEqual(roles, jwt.Roles);
			Assert.AreEqual(authUrl, jwt.AuthUrl);
			Assert.AreEqual(apiUrl, jwt.ApiUrl);
			Assert.AreEqual(userType, jwt.UserType);
			Assert.AreEqual(clientID, jwt.ClientID);
			Assert.AreEqual(userDatabaseID, jwt.UserDatabaseID);
			Assert.AreEqual(companyDatabaseID, jwt.CompanyDatabaseID);
			Assert.AreEqual(sellerDatabaseID, jwt.SellerDatabaseID);
			Assert.AreEqual(companyInteropID, jwt.CompanyInteropID);
			Assert.AreEqual(sellerInteropID, jwt.SellerInteropID);
			Assert.AreEqual(Truncate(issuedAtUTC), Truncate(jwt.IssuedAtUTC));
			Assert.AreEqual(marketplaceID, jwt.MarketplaceID);
			Assert.AreEqual(impersonatingUserDatabaseID, jwt.ImpersonatingUserDatabaseID);
			Assert.AreEqual(Truncate(exp), Truncate(jwt.ExpiresUTC));
			Assert.AreEqual(Truncate(nvb), Truncate(jwt.NotValidBeforeUTC));
		}

		[Test]
		public void create_fake_should_have_correct_defaults()
		{
			var raw = JwtOrderCloud.CreateFake();
			var jwt = new JwtOrderCloud(raw);

			Assert.AreEqual(raw, jwt.AccessToken);
			Assert.AreEqual(new List<string>(), jwt.Roles);
			Assert.AreEqual("mockdomain.com", jwt.AuthUrl);
			Assert.AreEqual("mockdomain.com", jwt.ApiUrl);
			Assert.AreEqual(Truncate(DateTime.UtcNow), Truncate(jwt.NotValidBeforeUTC));
			Assert.AreEqual(Truncate(DateTime.UtcNow.AddMinutes(30)), Truncate(jwt.ExpiresUTC));

			Assert.AreEqual(null, jwt.KeyID);
			Assert.AreEqual(null, jwt.AnonOrderID);
			Assert.AreEqual(null, jwt.Username);
			Assert.AreEqual(null, jwt.UserType);
			Assert.AreEqual(null, jwt.ClientID);
			Assert.AreEqual(null, jwt.UserDatabaseID);
			Assert.AreEqual(null, jwt.CompanyDatabaseID);
			Assert.AreEqual(null, jwt.SellerDatabaseID);
			Assert.AreEqual(null, jwt.CompanyInteropID);
			Assert.AreEqual(null, jwt.SellerInteropID);
			Assert.AreEqual(null, jwt.IssuedAtUTC);
			Assert.AreEqual(null, jwt.MarketplaceID);
			Assert.AreEqual(null, jwt.ImpersonatingUserDatabaseID);
		}

		public static DateTime? Truncate(DateTime? dateTime)
		{
			if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue) return dateTime; // do not modify "guard" values
			return dateTime?.AddTicks(-((dateTime?.Ticks ?? 0) % TimeSpan.FromSeconds(10).Ticks));
		}
	}
}
