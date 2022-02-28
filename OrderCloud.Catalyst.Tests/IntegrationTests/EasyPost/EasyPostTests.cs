using AutoFixture;
using Flurl.Http.Testing;
using NUnit.Framework;
using OrderCloud.Catalyst.Shipping.EasyPost;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Tests
{
	[TestFixture]
	public class EasyPostTests
	{
		private static Fixture _fixture = new Fixture();
		private HttpTest _httpTest;
		private static EasyPostConfig _config = new EasyPostConfig()
		{
			ApiKey = _fixture.Create<string>(),
			BaseUrl = "https://api.fake.com",
			CarrierAccountIDs = new List<string> { "fake"}
		};
		private EasyPostCommand _command = new EasyPostCommand(_config);
		private List<ShipPackage> _packages = _fixture.Create<List<ShipPackage>>();

		[SetUp]
		public void CreateHttpTest()
		{
			_httpTest = new HttpTest();
		}

		[TearDown]
		public void DisposeHttpTest()
		{
			_httpTest.Dispose();
		}

		[Test]
		public void ShouldThrowErrorIfMissingRequiredConfigs()
		{
			// Arrange
			var config = new EasyPostConfig();
			// Act 
			var ex = Assert.ThrowsAsync<IntegrationMissingConfigsException>(async () =>
				await _command.CalculateShipMethodsAsync(_packages, config)
			);
			// Assert
			var data = (IntegrationMissingConfigs)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "EasyPost");
			Assert.AreEqual(data.MissingFieldNames, new List<string> { "BaseUrl", "ApiKey" });
		}

		[Test]
		public void ShouldThrowErrorIfAuthorizationFails()
		{
			// Arrange
			_httpTest.RespondWith("{\"error\":{\"code\":\"APIKEY.INACTIVE\",\"message\":\"This api key is no longer active.Please use a different api key or reactivate this key.\",\"errors\":[]}}", 403); // real taxjar response

			var ex = Assert.ThrowsAsync<IntegrationAuthFailedException>(async () =>
				// Act 
				await _command.CalculateShipMethodsAsync(_packages)
			);

			var data = (IntegrationAuthFailedError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "EasyPost");
			Assert.AreEqual(data.ResponseStatus, 403);
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/shipments");
		}

		[Test]
		public void ShouldThrowErrorIfNoResponse()
		{
			// Arrange
			_httpTest.SimulateTimeout();

			var ex = Assert.ThrowsAsync<IntegrationNoResponseException>(async () =>
			// Act 
			await _command.CalculateShipMethodsAsync(_packages));

			var data = (IntegrationNoResponseError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "EasyPost");
		}

		[Test]
		public void ShouldThrowErrorForBadRequest()
		{
			// Arrange
			_httpTest
				// real vertex response
				.RespondWith("{\"error\":{\"code\":\"ADDRESS.COUNTRY.INVALID\",\"message\":\"Invalid 'country', please provide a 2 character ISO country code.\",\"errors\":[]}}", 422);

			var ex = Assert.ThrowsAsync<IntegrationErrorResponseException>(async () =>
				// Act 
				await _command.CalculateShipMethodsAsync(_packages)
			);

			var data = (IntegrationErrorResponseError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "EasyPost");
			Assert.AreEqual(data.ResponseStatus, 422);
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/shipments");
			Assert.AreEqual(((EasyPostError)data.ResponseBody).error.code, "ADDRESS.COUNTRY.INVALID");
		}
	}
}
