using System.Collections.Generic;
using System.Threading.Tasks;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public class VertexOCIntegrationCommand : OCIntegrationCommand, ITaxCalculator
	{
		protected readonly VertexClient _client;
		protected readonly VertexOCIntegrationConfig _config;

		public VertexOCIntegrationCommand(VertexOCIntegrationConfig config) : base(config)
		{
			_config = config;
			_client = new VertexClient(config);
		}

		/// <summary>
		/// Calculates tax for an order without creating any records. Use this to display tax amount to user prior to order submit.
		/// </summary>
		public async Task<OrderTaxCalculation> CalculateEstimateAsync(OrderWorksheet orderWorksheet, List<OrderPromotion> promotions) =>
			await CalculateTaxAsync(orderWorksheet, promotions, VertexSaleMessageType.QUOTATION);

		/// <summary>
		/// Creates a tax transaction record in the calculating system. Use this once on purchase, payment capture, or fulfillment.
		/// </summary>
		public async Task<OrderTaxCalculation> CommitTransactionAsync(OrderWorksheet orderWorksheet, List<OrderPromotion> promotions) =>
			await CalculateTaxAsync(orderWorksheet, promotions, VertexSaleMessageType.INVOICE);

		protected async Task<OrderTaxCalculation> CalculateTaxAsync(OrderWorksheet orderWorksheet, List<OrderPromotion> promotions, VertexSaleMessageType type)
		{
			var request = orderWorksheet.ToVertexCalculateTaxRequest(promotions, _config.CompanyName, type);
			var response = await _client.CalculateTax(request);
			var orderTaxCalculation = response.ToOrderTaxCalculation();
			return orderTaxCalculation;
		}
	}
 }
