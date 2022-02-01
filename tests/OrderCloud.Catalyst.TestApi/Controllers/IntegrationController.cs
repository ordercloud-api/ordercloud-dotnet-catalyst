using Microsoft.AspNetCore.Mvc;
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

		[HttpGet("vertex")]
		public async Task<OrderTaxCalculation> GetTaxRates()
		{
			var command = new VertexOCIntegrationCommand(new VertexOCIntegrationConfig()
			{
				CompanyName = "OrderCloudTest",
				ClientID = "REST-API-SiteCoreUSA",
				ClientSecret = "dbbd2e08e5c3bef5f921c0110f17c950",
				Username = "e44b802c-d160-4e9f-8f72-614bb08d4d86",
				Password = "sD$9/n2A",
			});

			var worksheet = new OrderWorksheet()
			{
				Order = new Order()
				{
					ID = "testId",
					FromUserID = "testUserID",
					FromUser = new User()
					{
						Email = "oheywood@test.com",
					},
				},
				LineItems = new List<LineItem>
				{
					new LineItem()
					{
						ProductID = "testProductID",
						Quantity = 10,
						UnitPrice = 20,
						ID = "testLineItemID",
						LineTotal = 200,
						Product = new LineItemProduct()
						{
							Name= "productName"
						},
						ShippingAddress = new Address()
						{
							//Street1 = "2418 e menlo blvd",
							//State = "wi",
							//Zip = "53211",
							//City = "shorewood",
							//Country = "us"
						}
					}
				},
				ShipEstimateResponse = new ShipEstimateResponse()
				{
					ShipEstimates = new List<ShipEstimate> { }
				}
			};

			return await command.CalculateEstimateAsync(worksheet, new List<OrderPromotion> { });
		}
	}
}
