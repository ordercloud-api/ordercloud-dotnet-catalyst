using Microsoft.AspNetCore.Mvc;
using OrderCloud.Integrations.Shipping.EasyPost;
using OrderCloud.Integrations.Shipping.Fedex;
using OrderCloud.Integrations.Shipping.UPS;
using OrderCloud.Integrations.Tax.Avalara;
using OrderCloud.Integrations.Tax.TaxJar;
using OrderCloud.Integrations.Tax.Vertex;
using OrderCloud.SDK;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.TestApi.Controllers
{
	[Route("int")]
	public class IntegrationController : CatalystController
	{
		public IntegrationController()
		{
		}

		private readonly List<ShippingPackage> Packages = new List<ShippingPackage>()
	{
		new ShippingPackage()
		{
			Length = 10,
			Width =10,
			Height =10,
			Weight = 10,
			ShipFrom = new Address()
			{
				Street1 = "2418 e menlo blvd",
				State = "WI",
				Zip = "53211",
				City = "shorewood",
				Country = "us"
			},
			ShipTo = new Address()
			{
				Street1 = "3611 Bryant Ave S",
				State = "MN",
				Zip = "55409",
				City = "Minneapolis",
				Country = "kust"
			},
			ReturnAddress = new Address()
			{
				Street1 = "2418 e menlo blvd",
				State = "ZZ",
				Zip = "53211",
				City = "shorewood",
				Country = "us"
			},
			Customs = new CustomsInfo(),
			Insurance = new InsuranceInfo()
		}
	};

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
					State = "WI",
					Zip = "53211",
					City = "shorewood",
					Country = "us"
				},
				ShipTo = new Address()
				{
					Street1 = "2418 e menlo blvd",
					State = "WI",
					Zip = "53211",
					City = "shorewood",
					Country = "us"
				}
			}
		},
			ShippingCosts = new List<ShipEstimateSummaryForTax> { }
		};

		[HttpGet("taxjar")]
		public async Task<object> GetTaxJarCategories()
		{
			var service = new TaxJarService(new TaxJarConfig()
			{
				BaseUrl = "https://api.sandbox.taxjar.com",
				APIToken = "ce17ad417ed4597e27313c088a9161a6",
			});

			return await service.CalculateEstimateAsync(Order);
		}

		[HttpGet("fedex")]
		public async Task<List<List<ShippingRate>>> GeFedexShipping()
		{
			var service = new FedexService(new FedexConfig()
			{
				AccountNumber = "740561073",
				ClientID = "l7d9da9b8938c948e0850e3b623de4ff15",
				ClientSecret = "ac4e0d0053f54d419cbb568b903dcbad",
				BaseUrl = "https://apis-sandbox.fedex.com",
			});

			return await service.CalculateShippingRatesAsync(Packages);
		}

		[HttpGet("easypost")]
		public async Task<List<List<ShippingRate>>> GetEasyPostShipping()
		{
			var service = new EasyPostService(new EasyPostConfig()
			{
				ApiKey = "EZTK9958cd2868b64389b379b52a22bbb6abnmkmZhaGXPzf9JIYTNrSiQ",
				BaseUrl = "https://api.easypost.com/v2",
				CarrierAccountIDs = new List<string> { "ca_8bdb711131894ab4b42abcd1645d988c" }
			});

			return await service.CalculateShippingRatesAsync(Packages);
		}

		[HttpGet("ups")]
		public async Task<List<List<ShippingRate>>> GetUPSShipping()
		{
			var service = new UPSService(new UPSConfig()
			{
				BaseUrl = "https://onlinetools.ups.com/ship/v1",
				ApiKey = "7D5181BE75E927A8",
			});

			return await service.CalculateShippingRatesAsync(Packages);
		}


		[HttpGet("vertex")]
		public async Task<OrderTaxCalculation> GetTaxRates()
		{
			var service = new VertexService(new VertexConfig()
			{
				CompanyName = "OrderCloudTest",
				ClientID = "REST-API-SiteCoreUSA",
				ClientSecret = "dbbd2e08e5c3bef5f921c0110f17c950",
				Username = "e44b802c-d160-4e9f-8f72-614bb08d4d86",
				Password = "sD$9/n2A",
			});

			return await service.CalculateEstimateAsync(Order);
		}

		[HttpGet("avalara")]
		public async Task<OrderTaxCalculation> GetAvalaraTaxRates()
		{
			var service = new AvalaraService(new AvalaraConfig()
			{
				CompanyCode = "SEBVENDORPORTAL",
				LicenseKey = "746AD9B585887041",
				BaseUrl = "https://sandbox-rest.avatax.com",
				AccountID = "2000004068",
			});

			return await service.CalculateEstimateAsync(Order);
		}
	}
}
