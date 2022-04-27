using AutoFixture;
using Flurl.Http.Testing;
using NUnit.Framework;
using OrderCloud.Integrations.Shipping.EasyPost;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests.IntegrationTests
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
			CarrierAccountIDs = new List<string> { "fake" }
		};
		private EasyPostService _command = new EasyPostService(_config);
		private List<ShippingPackage> _packages = _fixture.Create<List<ShippingPackage>>();

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
				await _command.CalculateShippingRatesAsync(_packages, config)
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
				await _command.CalculateShippingRatesAsync(_packages)
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
			await _command.CalculateShippingRatesAsync(_packages));

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
				await _command.CalculateShippingRatesAsync(_packages)
			);

			var data = (IntegrationErrorResponseError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "EasyPost");
			Assert.AreEqual(data.ResponseStatus, 422);
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/shipments");
			Assert.AreEqual(((EasyPostError)data.ResponseBody).error.code, "ADDRESS.COUNTRY.INVALID");
		}

		[Test]
		public async Task SuccessResponseShouldBeMappedCorrectly()
		{
			// Arrange
			var shipmentResponse = _fixture.Create<EasyPostShipment>();
			foreach (var rate in shipmentResponse.rates)
			{
				rate.rate = _fixture.Create<decimal>().ToString();
				rate.list_rate = _fixture.Create<decimal>().ToString();
			}

			_httpTest.RespondWithJson(shipmentResponse);
			// Act
			var result = await _command.CalculateShippingRatesAsync(_packages);

			// Assert
			Assert.AreEqual(_packages.Count, result.Count);

			Assert.AreEqual(shipmentResponse.rates.Count, result[0].Count);

			var mockRate = shipmentResponse.rates[0];
			var resultRate = result[0][0];
			Assert.AreEqual(mockRate.id, resultRate.ID);
			Assert.AreEqual(mockRate.service, resultRate.Name);
			Assert.AreEqual(mockRate.delivery_days, resultRate.EstimatedTransitDays);
			Assert.AreEqual(decimal.Parse(mockRate.rate), resultRate.Cost);
		}
	}
}
