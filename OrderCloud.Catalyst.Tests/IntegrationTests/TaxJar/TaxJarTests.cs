using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Flurl.Http.Testing;
using NUnit.Framework;
using OrderCloud.Catalyst.Tax.TaxJar;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst.Tests
{
	[TestFixture]
	public class TaxJarTests
	{
		private static Fixture _fixture = new Fixture();
		private HttpTest _httpTest;
		private static TaxJarConfig _config = new TaxJarConfig()
		{
			APIToken = _fixture.Create<string>(),
			BaseUrl = "https://api.fake.com"
		};
		private TaxJarCommand _command = new TaxJarCommand(_config);
		private OrderSummaryForTax _order = _fixture.Create<OrderSummaryForTax>();

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
			var config = new TaxJarConfig();
			// Act 
			var ex = Assert.ThrowsAsync<IntegrationMissingConfigsException>(async () =>
				await _command.CalculateEstimateAsync(_order, config)
			);
			// Assert
			var data = (IntegrationMissingConfigs)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "TaxJar");
			Assert.AreEqual(data.MissingFieldNames, new List<string> { "BaseUrl", "APIToken" });
		}

		[Test]
		public void ShouldThrowErrorIfAuthorizationFails()
		{
			// Arrange
			_httpTest.RespondWith("{\"error\":\"Unauthorized\",\"detail\":\"Not authorized for route 'POST /v2/taxes'\",\"status\":401}", 401); // real taxjar response

			var ex = Assert.ThrowsAsync<IntegrationAuthFailedException>(async () =>
				// Act 
				await _command.CalculateEstimateAsync(_order)
			);

			var data = (IntegrationAuthFailedError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "TaxJar");
			Assert.AreEqual(data.ResponseStatus, 401);
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/v2/taxes");
		}

		[Test]
		public void ShouldThrowErrorIfNoResponse()
		{
			// Arrange
			_httpTest.SimulateTimeout();

			var ex = Assert.ThrowsAsync<IntegrationNoResponseException>(async () =>
				// Act 
				await _command.CalculateEstimateAsync(_order)
			);

			var data = (IntegrationNoResponseError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "TaxJar");
		}

		[Test]
		public void ShouldThrowErrorForBadRequest()
		{
			// Arrange
			_httpTest
				// real vertex response
				.RespondWith("{\"error\":\"Bad Request\",\"detail\":\"No to zip, required when country is US\",\"status\":400}", 400);

			var ex = Assert.ThrowsAsync<IntegrationErrorResponseException>(async () =>
				// Act 
				await _command.CalculateEstimateAsync(_order)
			);

			var data = (IntegrationErrorResponseError) ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "TaxJar");
			Assert.AreEqual(data.ResponseStatus, 400);
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/v2/taxes");
			Assert.AreEqual(((TaxJarError) data.ResponseBody).detail, "No to zip, required when country is US");
		}

		[Test]
		public async Task ShouldCalculateTotalTaxAsSum()
		{
			// Arrange
			var lineItems = _order.LineItems;
			var shipEstimates = _order.ShipEstimates;
			var response = _fixture.Create<TaxJarCalcResponse>();
			_httpTest.RespondWithJson(response);
			// Act
			var result = await _command.CalculateEstimateAsync(_order);
			// Assert
			Assert.AreEqual(((lineItems.Count + shipEstimates.Count) * response.tax.amount_to_collect), result.TotalTax);
		}

		[Test]
		public async Task EstimateShouldMakeCalculateRequests()
		{
			// Arrange
			var lineItems = _order.LineItems;
			var shipEstimates = _order.ShipEstimates;
			var response = _fixture.Create<TaxJarCalcResponse>();
			_httpTest.RespondWithJson(response);
			// Act
			var result = await _command.CalculateEstimateAsync(_order);
			// Assert
			_httpTest.ShouldHaveCalled($"{_config.BaseUrl}/v2/taxes").Times((lineItems.Count + shipEstimates.Count));
			_httpTest.ShouldNotHaveCalled($"{_config.BaseUrl}/v2/taxes/orders");
		}

		[Test]
		public async Task CommitShouldMakeBothRequests()
		{
			// Arrange
			var lineItems = _order.LineItems;
			var shipEstimates = _order.ShipEstimates;
			var response = _fixture.Create<TaxJarCalcResponse>();
			_httpTest.RespondWithJson(response);
			// Act
			var result = await _command.CommitTransactionAsync(_order);
			// Assert
			_httpTest.ShouldHaveCalled($"{_config.BaseUrl}/v2/taxes").Times((lineItems.Count + shipEstimates.Count));
			_httpTest.ShouldHaveCalled($"{_config.BaseUrl}/v2/taxes/orders").Times((lineItems.Count + shipEstimates.Count));
		}
	}
}
