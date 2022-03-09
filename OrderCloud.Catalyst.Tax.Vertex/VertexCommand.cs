using System.Collections.Generic;
using System.Threading.Tasks;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst.Tax.Vertex
{
	public class VertexCommand : OCIntegrationCommand, ITaxCalculator
	{
		public VertexCommand(VertexConfig defaultConfig) : base(defaultConfig) { }

		/// <summary>
		/// Calculates tax for an order without creating any records. Use this to display tax amount to user prior to order submit.
		/// </summary>
		public async Task<OrderTaxCalculation> CalculateEstimateAsync(OrderSummaryForTax orderSummary, OCIntegrationConfig overrideConfig = null) =>
			await CalculateTaxAsync(VertexSaleMessageType.QUOTATION, orderSummary, overrideConfig);

		/// <summary>
		/// Creates a tax transaction record in the calculating system. Use this once on purchase, payment capture, or fulfillment.
		/// </summary>
		public async Task<OrderTaxCalculation> CommitTransactionAsync(OrderSummaryForTax orderSummary, OCIntegrationConfig overrideConfig = null) =>
			await CalculateTaxAsync(VertexSaleMessageType.INVOICE, orderSummary, overrideConfig);

		protected async Task<OrderTaxCalculation> CalculateTaxAsync(VertexSaleMessageType type, OrderSummaryForTax orderSummary, OCIntegrationConfig overrideConfig)
		{
			var config = ValidateConfig<VertexConfig>(overrideConfig ?? _defaultConfig);
			var request = VertexRequestMapper.ToVertexCalculateTaxRequest(orderSummary, config.CompanyName, type);
			var response = await VertexClient.CalculateTax(request, config);
			var orderTaxCalculation = response.ToOrderTaxCalculation();
			return orderTaxCalculation;
		}
	}
 }
