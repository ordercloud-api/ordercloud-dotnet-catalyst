using System.Collections.Generic;
using System.Threading.Tasks;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public class VertexCommand : OCIntegrationCommand, ITaxCalculator
	{
		protected readonly VertexClient _client;
		protected readonly VertexConfig _config;

		public VertexCommand(VertexConfig config) : base(config)
		{
			_config = config;
			_client = new VertexClient(config);
		}

		/// <summary>
		/// Calculates tax for an order without creating any records. Use this to display tax amount to user prior to order submit.
		/// </summary>
		public async Task<OrderTaxCalculation> CalculateEstimateAsync(OrderSummaryForTax orderSummary) =>
			await CalculateTaxAsync(orderSummary, VertexSaleMessageType.QUOTATION);

		/// <summary>
		/// Creates a tax transaction record in the calculating system. Use this once on purchase, payment capture, or fulfillment.
		/// </summary>
		public async Task<OrderTaxCalculation> CommitTransactionAsync(OrderSummaryForTax orderSummary) =>
			await CalculateTaxAsync(orderSummary, VertexSaleMessageType.INVOICE);

		protected async Task<OrderTaxCalculation> CalculateTaxAsync(OrderSummaryForTax orderSummary, VertexSaleMessageType type)
		{
			var request = VertexRequestMapper.ToVertexCalculateTaxRequest(orderSummary, _config.CompanyName, type);
			var response = await _client.CalculateTax(request);
			var orderTaxCalculation = response.ToOrderTaxCalculation();
			return orderTaxCalculation;
		}
	}
 }
