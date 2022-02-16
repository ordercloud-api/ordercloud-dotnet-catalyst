using Microsoft.AspNetCore.Mvc;
using OrderCloud.Catalyst.Tax.Avalara;
using OrderCloud.Catalyst.Tax.TaxJar;
using OrderCloud.Catalyst.Tax.Vertex;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.TestApi.Controllers
{
	[Route("int")]
	public class IntegrationController : CatalystController
	{
		public IntegrationController()
		{
		}

		private readonly OrderSummaryForTax Order = new OrderSummaryForTax()
		{
			OrderID = "testId",
			CustomerCode = "testUserID",
			PromotionDiscount = 0,
			LineItems = new List<LineItemSummaryForTax>
			{
				new LineItemSummaryForTax()
				{
					ProductID = "testProductID",
					Quantity = 10,
					UnitPrice = 20,
					LineTotal = 200,
					ProductName = "productName",
					ShipFrom = new Address() 
					{
						Street1 = "2418 e menlo blvd",
						State = "wi",
						Zip = "53211",
						City = "shorewood",
						Country = "us"
					},
					ShipTo = new Address()
					{
						Street1 = "2418 e menlo blvd",
						State = "wi",
						Zip = "53211",
						City = "shorewood",
						Country = "us"
					}
				}
			},
			ShipEstimates = new List<ShipEstimateSummaryForTax> { }
		};

		[HttpGet("taxjar")]
		public async Task<object> GetTaxJarCategories()
		{
			var command = new TaxJarCommand(new TaxJarConfig()
			{
				BaseUrl = "https://api.sandbox.taxjar.com",
				APIToken = "ce17ad417ed4597e27313c088a9161a6",
			});

			return await command.CalculateEstimateAsync(Order);
		}

		[HttpGet("vertex")]
		public async Task<OrderTaxCalculation> GetTaxRates()
		{
			var command = new VertexCommand(new VertexConfig()
			{
				CompanyName = "OrderCloudTest",
				ClientID = "REST-API-SiteCoreUSA",
				ClientSecret = "dbbd2e08e5c3bef5f921c0110f17c950",
				Username = "e44b802c-d160-4e9f-8f72-614bb08d4d86",
				Password = "sD$9/n2A",
			});

			return await command.CalculateEstimateAsync(Order);
		}

		[HttpGet("avalara")]
		public async Task<OrderTaxCalculation> GetAvalaraTaxRates()
		{
			var command = new AvalaraCommand(new AvalaraConfig()
			{
				CompanyCode = "SEBVENDORPORTAL",
				LicenseKey = "746AD9B585887041",
				BaseUrl = "https://sandbox-rest.avatax.com",
				AccountID = "2000004068",
			});

			return await command.CalculateEstimateAsync(Order);
		}
	}
}
