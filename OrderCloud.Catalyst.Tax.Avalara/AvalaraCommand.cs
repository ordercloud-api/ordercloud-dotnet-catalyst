using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tax.Avalara
{
	public class AvalaraCommand : OCIntegrationCommand , ITaxCodesProvider, ITaxCalculator
	{
		public AvalaraCommand(AvalaraConfig configDefault) : base(configDefault) { }

		public async Task<OrderTaxCalculation> CalculateEstimateAsync(OrderSummaryForTax orderSummary, OCIntegrationConfig configOverride = null) => 
			await CreateTransactionAsync(AvalaraDocumentType.SalesOrder, orderSummary, configOverride);

		public async Task<OrderTaxCalculation> CommitTransactionAsync(OrderSummaryForTax orderSummary, OCIntegrationConfig configOverride = null) =>
			await CreateTransactionAsync(AvalaraDocumentType.SalesInvoice, orderSummary, configOverride);

		protected async Task<OrderTaxCalculation> CreateTransactionAsync(AvalaraDocumentType type, OrderSummaryForTax orderSummary, OCIntegrationConfig configOverride = null)
		{
			var config = GetValidatedConfig<AvalaraConfig>(configOverride);
			var createTransaction = AvalaraRequestMapper.ToAvalaraTransactionModel(orderSummary, config.CompanyCode, type);
			var transaction = await AvalaraClient.CreateTransaction(createTransaction, config);
			var calculation = AvalaraResponseMapper.ToOrderTaxCalculation(transaction);
			return calculation;
		}

		public async Task<TaxCategorizationResponse> ListTaxCodesAsync(string filterTerm, OCIntegrationConfig configOverride = null)
		{
			var config = GetValidatedConfig<AvalaraConfig>(configOverride);
			var filter = AvalaraTaxCodeMapper.MapFilterTerm(filterTerm);
			var codes = await AvalaraClient.ListTaxCodesAsync(filter, config);
			return new TaxCategorizationResponse()
			{
				Categories = AvalaraTaxCodeMapper.MapTaxCodes(codes)
			};
		}
	}
}
