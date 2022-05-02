using FluentAssertions;
using Flurl.Http;
using NUnit.Framework;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests
{
	[TestFixture]
	public class UserTypeRestrictedTests
	{
		[Test]
		public async Task should_have_no_effect_without_UserAuth()
		{
			var request = TestFramework.Client
				.Request("demo/unrestricted");

			var result = await request.GetStringAsync();

			result.Should().Be("\"hello anyone!\"");
		}

		[TestCase(CommerceRole.Seller, false)]
		[TestCase(CommerceRole.Supplier, true)]
		[TestCase(CommerceRole.Buyer, false)]
		public async Task should_allow_only_suppliers(CommerceRole role, bool allowed)
		{
			var token = FakeOrderCloudToken.Create("mYcLiEnTiD", new List<string> { "Shopper" }, userType: role);
			var request = TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/supplier");

			if (allowed)
			{
				var response = await request.GetAsync();
				Assert.AreEqual(200, response.StatusCode);
			}
			else
			{
				var response = await request.GetAsync();
				var json = await request.GetJsonAsync();
				await response.ShouldHaveFirstApiError("InvalidUserType", 403, $"Users of type {role} do not have permission to perform this action.");
				Assert.AreEqual(role.ToString(), json.Errors[0].Data.ThisUserType);
				Assert.AreEqual("Supplier", json.Errors[0].Data.UserTypesThatCanAccess[0]);
			}
		}

		[TestCase(CommerceRole.Seller, true)]
		[TestCase(CommerceRole.Supplier, true)]
		[TestCase(CommerceRole.Buyer, false)]
		public async Task should_allow_seller_or_suppliers(CommerceRole role, bool allowed)
		{
			var token = FakeOrderCloudToken.Create("mYcLiEnTiD", new List<string> { "Shopper" }, userType: role);
			var request = TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/supplierorseller");

			if (allowed)
			{
				var response = await request.GetAsync();
				Assert.AreEqual(200, response.StatusCode);
			}
			else
			{
				var response = await request.GetAsync();
				var json = await request.GetJsonAsync();
				await response.ShouldHaveFirstApiError("InvalidUserType", 403, $"Users of type {role} do not have permission to perform this action.");
				Assert.AreEqual(role.ToString(), json.Errors[0].Data.ThisUserType);
				Assert.AreEqual("Supplier", json.Errors[0].Data.UserTypesThatCanAccess[0]);
				Assert.AreEqual("Seller", json.Errors[0].Data.UserTypesThatCanAccess[1]);

			}
		}

		public async Task first_error_should_be_about_type_not_role()
		{
			var token = FakeOrderCloudToken.Create("mYcLiEnTiD", userType: CommerceRole.Buyer);
			var request = TestFramework.Client
				.WithOAuthBearerToken(token)
				.Request("demo/supplieradmin");

				var response = await request.GetAsync();
				var json = await request.GetJsonAsync();
				await response.ShouldHaveFirstApiError("InvalidUserType", 403, $"Users of type Buyer do not have permission to perform this action.");
		}
	}
}
