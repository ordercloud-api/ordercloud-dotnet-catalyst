using AutoFixture;
using Flurl.Http.Testing;
using NUnit.Framework;
using OrderCloud.Catalyst.Shipping.UPS;
using OrderCloud.Catalyst.Tax.Avalara;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests.IntegrationTests
{ 
	[TestFixture]
	public class UPSTests
	{
		private static Fixture _fixture = new Fixture();
		private HttpTest _httpTest;
		private static UPSConfig _config = new UPSConfig()
		{
			ApiKey = _fixture.Create<string>(),
			BaseUrl = "https://api.fake.com"
		};
		private UPSService _command = new UPSService(_config);
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
			var config = new UPSConfig();
			// Act 
			var ex = Assert.ThrowsAsync<IntegrationMissingConfigsException>(async () =>
				await _command.CalculateShippingRatesAsync(_packages, config)
			); ;
			// Assert
			var data = (IntegrationMissingConfigs)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "UPS");
			Assert.AreEqual(data.MissingFieldNames, new List<string> { "BaseUrl", "ApiKey", });
		}

		[Test]
		public void ShouldThrowErrorIfAuthorizationFails()
		{
			// Arrange
			_httpTest.RespondWith("{\"response\":{\"errors\":[{\"code\":\"250003\",\"message\":\"Invalid Access License number\"}]}}", 401); // real avalara response

			var ex = Assert.ThrowsAsync<IntegrationAuthFailedException>(async () =>
				// Act 
				await _command.CalculateShippingRatesAsync(_packages)
			);

			var data = (IntegrationAuthFailedError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "UPS");
			Assert.AreEqual(data.ResponseStatus, 401);
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/rating/Shop");
		}

		[Test]
		public void ShouldThrowErrorIfNoResponse()
		{
			// Arrange
			_httpTest.SimulateTimeout();

			var ex = Assert.ThrowsAsync<IntegrationNoResponseException>(async () =>
				// Act 
				await _command.CalculateShippingRatesAsync(_packages)
			);

			var data = (IntegrationNoResponseError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "UPS");
		}

		[Test]
		public void ShouldThrowErrorForBadRequest()
		{
			// Arrange
			_httpTest
				// real avalara response
				.RespondWith("{\"response\":{\"errors\":[{\"code\":\"110208\",\"message\":\"Missing / Illegal ShipTo / Address / CountryCode\"}]}}", 400);

			var ex = Assert.ThrowsAsync<IntegrationErrorResponseException>(async () =>
				// Act 
				await _command.CalculateShippingRatesAsync(_packages)
			);

			var data = (IntegrationErrorResponseError) ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "UPS");
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/rating/Shop");
			Assert.AreEqual(data.ResponseStatus, 400);
			Assert.AreEqual(((UPSErrorBody)data.ResponseBody).response.errors[0].code, "110208");
			Assert.AreEqual(((UPSErrorBody)data.ResponseBody).response.errors[0].message, "Missing / Illegal ShipTo / Address / CountryCode");
		}

		[Test]
		public async Task SuccessResponseShouldBeMappedCorrectly()
		{
			// Arrange
			var shipmentResponse = _fixture.Create<UPSRestResponseBody>();
			foreach (var rate in shipmentResponse.RateResponse.RatedShipment)
			{
				rate.TotalCharges.MonetaryValue = _fixture.Create<decimal>().ToString();
				rate.GuaranteedDelivery.BusinessDaysInTransit = _fixture.Create<int>().ToString();
				rate.Service.Code = _fixture.Create<UPSServiceCodes>().ToString();
			}

			_httpTest.RespondWithJson(shipmentResponse);
			// Act
			var result = await _command.CalculateShippingRatesAsync(_packages);

			// Assert
			Assert.AreEqual(_packages.Count, result.Count);

			Assert.AreEqual(shipmentResponse.RateResponse.RatedShipment.Count, result[0].Count);

			var mockRate = shipmentResponse.RateResponse.RatedShipment[0];
			var resultRate = result[0][0];
			Assert.AreEqual(mockRate.Service.Code, resultRate.ID);
			Assert.AreEqual(int.Parse(mockRate.GuaranteedDelivery.BusinessDaysInTransit), resultRate.EstimatedTransitDays);
			Assert.AreEqual(decimal.Parse(mockRate.TotalCharges.MonetaryValue), resultRate.Cost);
		}

		[Test]
		public async Task ShippingOptionsWithoutGuaranteedDeliveryShouldBeSkipped()
		{
			// Arrange
			var shipmentResponse = _fixture.Create<UPSRestResponseBody>();
			foreach (var rate in shipmentResponse.RateResponse.RatedShipment)
			{
				rate.TotalCharges.MonetaryValue = _fixture.Create<decimal>().ToString();
				rate.GuaranteedDelivery = null;
				rate.Service.Code = _fixture.Create<UPSServiceCodes>().ToString();
			}

			_httpTest.RespondWithJson(shipmentResponse);
			// Act
			var result = await _command.CalculateShippingRatesAsync(_packages);

			// Assert
			foreach (var shipMethods in result)
			{
				Assert.AreEqual(0, shipMethods.Count);
			}
		}

		[Test]
		[TestCase("59", "UPS 2nd Day Air AM")]
		[TestCase("11", "UPS Standard")]
		[TestCase("12", "UPS 3 Day Select")]
		[TestCase("", "Invald UPS service Type")]
		public void ServiceNamesShouldMapCorrectly(string code, string expectedName)
		{
			var computedName = UPSRatesMapper.ToUPSServiceName(code);
			Assert.AreEqual(expectedName, computedName);
		}
	}
}
