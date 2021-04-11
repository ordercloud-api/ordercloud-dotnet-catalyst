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

			var expectedJwt = fixture.Create<JwtOrderCloud>();

			// these must be tested separately
			expectedJwt.ExpiresUTC = null;
			expectedJwt.NotValidBeforeUTC = null;
			expectedJwt.IssuedAtUTC = null;

			var raw = expectedJwt.CreateFake();
			var jwt = new JwtOrderCloud(raw);

			Assert.AreEqual(raw, jwt.AccessToken);
			Assert.AreEqual(expectedJwt.KeyID, jwt.KeyID);
			Assert.AreEqual(expectedJwt.AnonOrderID, jwt.AnonOrderID);
			Assert.AreEqual(expectedJwt.Username, jwt.Username);
			Assert.AreEqual(expectedJwt.Roles, jwt.Roles);
			Assert.AreEqual(expectedJwt.AuthUrl, jwt.AuthUrl);
			Assert.AreEqual(expectedJwt.ApiUrl, jwt.ApiUrl);
			Assert.AreEqual(expectedJwt.UserType, jwt.UserType);
			Assert.AreEqual(expectedJwt.ClientID, jwt.ClientID);
			Assert.AreEqual(expectedJwt.UserDatabaseID, jwt.UserDatabaseID);
			Assert.AreEqual(expectedJwt.CompanyDatabaseID, jwt.CompanyDatabaseID);
			Assert.AreEqual(expectedJwt.SellerDatabaseID, jwt.SellerDatabaseID);
			Assert.AreEqual(expectedJwt.CompanyInteropID, jwt.CompanyInteropID);
			Assert.AreEqual(expectedJwt.SellerInteropID, jwt.SellerInteropID);
			Assert.AreEqual(expectedJwt.IssuedAtUTC, jwt.IssuedAtUTC);
			Assert.AreEqual(expectedJwt.MarketplaceID, jwt.MarketplaceID);
			Assert.AreEqual(expectedJwt.ImpersonatingUserDatabaseID, jwt.ImpersonatingUserDatabaseID);
		}

		[Test]
		public void dates_should_deserialize_correctly()
		{
			var exp = new DateTime(2010, 4, 14, 0, 0, 0, DateTimeKind.Utc);
			var nvb = new DateTime(2000, 6, 1, 0, 0, 0, DateTimeKind.Utc);

			var expectedJwt = new JwtOrderCloud() { ExpiresUTC = exp, NotValidBeforeUTC = nvb } ;
			var raw = expectedJwt.CreateFake();
			var jwt = new JwtOrderCloud(raw);
		
			Assert.AreEqual(exp, jwt.ExpiresUTC);
			Assert.AreEqual(nvb, jwt.NotValidBeforeUTC);

		}

		[Test]
		public void defaults_should_deserialize_correctly()
		{
			var expectedJwt = new JwtOrderCloud();
			var raw = expectedJwt.CreateFake();
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
			return dateTime?.AddTicks(-((dateTime?.Ticks ?? 0) % TimeSpan.FromSeconds(1).Ticks));
		}
	}
}
