using System.Collections.Generic;
using System.Threading.Tasks;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public class VertexCommand : OCIntegrationCommand, ITaxCalculator
	{

		public VertexCommand(VertexConfig configDefault) : base(configDefault) { }

		/// <summary>
		/// Calculates tax for an order without creating any records. Use this to display tax amount to user prior to order submit.
		/// </summary>
		public async Task<OrderTaxCalculation> CalculateEstimateAsync(OrderSummaryForTax orderSummary, OCIntegrationConfig configOverride = null) =>
			await CalculateTaxAsync(VertexSaleMessageType.QUOTATION, orderSummary, configOverride);

		/// <summary>
		/// Creates a tax transaction record in the calculating system. Use this once on purchase, payment capture, or fulfillment.
		/// </summary>
		public async Task<OrderTaxCalculation> CommitTransactionAsync(OrderSummaryForTax orderSummary, OCIntegrationConfig configOverride = null) =>
			await CalculateTaxAsync(VertexSaleMessageType.INVOICE, orderSummary, configOverride);

		protected async Task<OrderTaxCalculation> CalculateTaxAsync(VertexSaleMessageType type, OrderSummaryForTax orderSummary, OCIntegrationConfig configOverride)
		{
			var config = GetValidatedConfig<VertexConfig>(configOverride);
			var request = VertexRequestMapper.ToVertexCalculateTaxRequest(orderSummary, config.CompanyName, type);
			var response = await VertexClient.CalculateTax(request, config);
			var orderTaxCalculation = response.ToOrderTaxCalculation();
			return orderTaxCalculation;
		}
	}
 }
