using Flurl.Http;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tax.TaxJar
{
	public class TaxJarCommand : OCIntegrationCommand, ITaxCalculator, ITaxCodesProvider
	{
		public TaxJarCommand(TaxJarConfig configDefault) : base(configDefault) { }

		public async Task<TaxCategorizationResponse> ListTaxCodesAsync(string filterTerm = "", OCIntegrationConfig configOverride = null)
		{
			var config = GetValidatedConfig<TaxJarConfig>(configOverride);
			var categories = await TaxJarClient.ListCategoriesAsync(config);
			return TaxJarCategoryMapper.ToTaxCategorization(categories, filterTerm);
		}

		public async Task<OrderTaxCalculation> CalculateEstimateAsync(OrderSummaryForTax orderSummary, OCIntegrationConfig configOverride = null)
		{
			var config = GetValidatedConfig<TaxJarConfig>(configOverride);
			var orders = await CalculateTax(orderSummary, config);
			var orderTaxCalculation = TaxJarResponseMapper.ToOrderTaxCalculation(orders);
			return orderTaxCalculation;
		}

		public async Task<OrderTaxCalculation> CommitTransactionAsync(OrderSummaryForTax orderSummary, OCIntegrationConfig configOverride = null)
		{
			var config = GetValidatedConfig<TaxJarConfig>(configOverride);
			var orders = await CalculateTax(orderSummary, config);
			foreach (var response in orders)
			{
				response.request.transaction_date = DateTime.UtcNow.ToString("yyyy/MM/dd");
				response.request.sales_tax = response.response.tax.amount_to_collect;
			}

			await Throttler.RunAsync(orders, 100, 8, async order => await TaxJarClient.CreateOrderAsync(order.request, config));

			var orderTaxCalculation = TaxJarResponseMapper.ToOrderTaxCalculation(orders);
			return orderTaxCalculation;
		}

		protected async Task<IEnumerable<(TaxJarOrder request, TaxJarCalcResponse response)>> CalculateTax(OrderSummaryForTax orderSummary, TaxJarConfig config)
		{
			var orders = TaxJarRequestMapper.ToOrders(orderSummary);
			return await Throttler.RunAsync(orders, 100, 8, async order =>
			{
				var tax = await TaxJarClient.CalcTaxForOrderAsync(order, config);
				return (order, tax);
			});
		}
	}
}
