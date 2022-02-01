using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Flurl.Http.Testing;
using NUnit.Framework;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst.Tests
{
    [TestFixture]
    public class VertexTests
    {
        private static Fixture _fixture = new Fixture();
        private HttpTest _httpTest;
        private static VertexOCIntegrationConfig _config = _fixture.Create<VertexOCIntegrationConfig>();
        private VertexOCIntegrationCommand _command = new VertexOCIntegrationCommand(_config);
		private OrderWorksheetBuilder _worksheetBuilder = new OrderWorksheetBuilder();

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
			var config = new VertexOCIntegrationConfig();
			// Act 
			var ex = Assert.Throws<IntegrationMissingConfigsException>(() => new VertexOCIntegrationCommand(config));
			// Assert
			var data = (IntegrationMissingConfigs) ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Vertex");
			Assert.AreEqual(data.MissingFieldNames, new List<string> { "CompanyName", "ClientID", "ClientSecret", "Username", "Password" });
		}

		[Test]
		public async Task ResponseShouldBeMappedCorrectly()
		{
			// Arrange
			var response = _fixture.Create<VertexResponse<VertexCalculateTaxResponse>>();
			_httpTest.RespondWithJson(response);
			// Act
			var result = await _command.CalculateEstimateAsync(_worksheetBuilder.Build(), new List<OrderPromotion> { });
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
