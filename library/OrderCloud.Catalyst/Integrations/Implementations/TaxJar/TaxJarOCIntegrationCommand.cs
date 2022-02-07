using Flurl.Http;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public class TaxJarOCIntegrationCommand : OCIntegrationCommand, ITaxCalculator, ITaxCodesProvider
	{
		protected readonly TaxJarOCIntegrationConfig _config;
		protected readonly TaxJarClient _client;

		public TaxJarOCIntegrationCommand(TaxJarOCIntegrationConfig config) : base(config)
		{
			_config = config;
			_client = new TaxJarClient(config);
		}

		public async Task<TaxCategorizationResponse> ListTaxCodesAsync(string filterTerm = "")
		{
			var categories = await _client.ListCategoriesAsync();
			return TaxJarCategoryMapper.ToTaxCategorization(categories, filterTerm);
		}

		public async Task<OrderTaxCalculation> CalculateEstimateAsync(OrderWorksheet orderWorksheet, List<OrderPromotion> promotions)
		{
			var orders = await CalculateTax(orderWorksheet);
			var orderTaxCalculation = TaxJarResponseMapper.ToOrderTaxCalculation(orders);
			return orderTaxCalculation;
		}

		public async Task<OrderTaxCalculation> CommitTransactionAsync(OrderWorksheet orderWorksheet, List<OrderPromotion> promotions)
		{
			var orders = await CalculateTax(orderWorksheet);
			foreach (var response in orders)
			{
				response.request.transaction_date = DateTime.UtcNow.ToString("yyyy/MM/dd");
				response.request.sales_tax = response.response.tax.amount_to_collect;
			}

			await Throttler.RunAsync(orders, 100, 8, async order => await _client.CreateOrderAsync(order.request));

			var orderTaxCalculation = TaxJarResponseMapper.ToOrderTaxCalculation(orders);
			return orderTaxCalculation;
		}

		protected async Task<IEnumerable<(TaxJarOrder request, TaxJarCalcResponse response)>> CalculateTax(OrderWorksheet orderWorksheet)
		{
			var orders = TaxJarRequestMapper.ToOrders(orderWorksheet);
			return await Throttler.RunAsync(orders, 100, 8, async order =>
			{
				var tax = await _client.CalcTaxForOrderAsync(order);
				return (order, tax);
			});
		}
	}
}
