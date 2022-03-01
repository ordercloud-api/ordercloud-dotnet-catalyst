using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Flurl.Http.Testing;
using NUnit.Framework;
using OrderCloud.Catalyst.Tax.Vertex;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst.Tests.IntegrationTests
{
    [TestFixture]
    public class VertexTests
    {
        private static Fixture _fixture = new Fixture();
        private HttpTest _httpTest;
        private static VertexConfig _config = _fixture.Create<VertexConfig>();
        private VertexCommand _command = new VertexCommand(_config);
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
			var config = new VertexConfig();
			// Act 
			var ex = Assert.ThrowsAsync<IntegrationMissingConfigsException>(async () =>
				await _command.CalculateEstimateAsync(_order, config)
			);
			// Assert
			var data = (IntegrationMissingConfigs) ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Vertex");
			Assert.AreEqual(data.MissingFieldNames, new List<string> { "CompanyName", "ClientID", "ClientSecret", "Username", "Password" });
		}

		[Test]
		public void ShouldThrowErrorIfAuthorizationFails()
		{
			// Arrange
			_httpTest.RespondWith("{\"error\":\"invalid_client\"}", 400); // real vertex response
			
			var ex = Assert.ThrowsAsync<IntegrationAuthFailedException>(async () =>
				// Act 
				await _command.CalculateEstimateAsync(_order)
			);

			var data = (IntegrationAuthFailedError) ex.Errors[0].Data;
			Assert.AreEqual(data.ResponseStatus, 400);
			Assert.AreEqual(data.ServiceName, "Vertex");
			Assert.AreEqual(data.RequestUrl, "https://auth.vertexsmb.com/identity/connect/token");
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

			var data = (IntegrationNoResponseError) ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Vertex");
		}

		[Test]
		public void ShouldThrowErrorForBadRequest()
		{
			// Arrange
			_httpTest
				// real vertex response
				.RespondWith("{\"access_token\":\"fakee1cf7e06bc50f1a884dd0fffake\",\"expires_in\":1200,\"token_type\":\"Bearer\"}", 200)
				// real vertex response
				.RespondWith("{\"meta\":{\"app\":\"Vertex REST API v2.0.0\",\"timeReceived\":\"2022-02-01T19: 04:12.751Z\",\"timeElapsed(ms)\":31},\"errors\":[{\"status\":\"400\",\"code\":\"VertexApplicationException\",\"title\":\"Bad Request\",\"detail\":\"The LocationRole being added is invalid.This might be due to an invalid location or an invalid address field.Make sure that the locationRole is valid, and try again.\n\"}]}", 400);

			var ex = Assert.ThrowsAsync<IntegrationErrorResponseException>(async () =>
				// Act 
				await _command.CalculateEstimateAsync(_order)
			);

			var data = (IntegrationErrorResponseError) ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Vertex");
			Assert.AreEqual(data.ResponseStatus, 400);
			Assert.AreEqual(data.RequestUrl, "https://restconnect.vertexsmb.com/vertex-restapi/v1/sale");
			Assert.AreEqual(((List<VertexResponseError>) data.ResponseBody)[0].detail, "The LocationRole being added is invalid.This might be due to an invalid location or an invalid address field.Make sure that the locationRole is valid, and try again.\n");
		}

		[Test]
		public async Task SuccessResponseShouldBeMappedCorrectly()
		{
			// Arrange
			var response = _fixture.Create<VertexResponse<VertexCalculateTaxResponse>>();
			_httpTest.RespondWithJson(response);
			// Act
			var result = await _command.CalculateEstimateAsync(_order);
			// Assert
			Assert.AreEqual(response.data.transactionId, result.OrderID);
			Assert.AreEqual(response.data.transactionId, result.ExternalTransactionID);
			Assert.AreEqual(response.data.totalTax, result.TotalTax);

			var lineItems = response.data.lineItems.Where(li => !li.IsShippingLineItem()).ToList();

			for (var i = 0; i < result.LineItems.Count; i++)
			{
				var responseLineItem = lineItems[i];
				var resultLineItem = result.LineItems[i];
				Assert.AreEqual(responseLineItem.lineItemId, resultLineItem.LineItemID);
				Assert.AreEqual(responseLineItem.totalTax, resultLineItem.LineItemTotalTax);
				for (var j = 0; j < lineItems[i].taxes.Count; j++)
				{
					var responseTax = responseLineItem.taxes[j];
					var resultTax = resultLineItem.LineItemLevelTaxes[j];
					Assert.AreEqual(responseTax.calculatedTax, resultTax.Tax);
					Assert.AreEqual(responseTax.taxable, resultTax.Taxable);
					Assert.AreEqual(0, resultTax.Exempt); // should maybe be something else
					Assert.AreEqual(responseTax.impositionType.value, resultTax.TaxDescription);
					Assert.AreEqual(responseTax.jurisdiction.jurisdictionLevel.ToString(), resultTax.JurisdictionLevel);
					Assert.AreEqual(responseTax.jurisdiction.value, resultTax.JurisdictionValue);
					Assert.AreEqual(null, resultTax.ShipEstimateID);
				}
			}

			var shippingLines = response.data.lineItems.Where(li => li.IsShippingLineItem()).SelectMany(li => li.taxes).ToList();

			for (var i = 0; i < result.OrderLevelTaxes.Count; i++)
			{
				var responseShipTax = shippingLines[i];
				var resultTax = result.OrderLevelTaxes[i];
				Assert.AreEqual(responseShipTax.calculatedTax, resultTax.Tax);
				Assert.AreEqual(responseShipTax.taxable, resultTax.Taxable);
				Assert.AreEqual(0, resultTax.Exempt); // should maybe be something else
				Assert.AreEqual(responseShipTax.impositionType.value, resultTax.TaxDescription);
				Assert.AreEqual(responseShipTax.jurisdiction.jurisdictionLevel.ToString(), resultTax.JurisdictionLevel);
				Assert.AreEqual(responseShipTax.jurisdiction.value, resultTax.JurisdictionValue);
			}
		}
	}
}
