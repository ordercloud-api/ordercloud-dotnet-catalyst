using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tax.Avalara
{
	public class AvalaraCommand : OCIntegrationCommand , ITaxCodesProvider, ITaxCalculator
	{
		public AvalaraCommand(AvalaraConfig defaultConfig) : base(defaultConfig) { }

		public async Task<OrderTaxCalculation> CalculateEstimateAsync(OrderSummaryForTax orderSummary, OCIntegrationConfig overrideConfig = null) => 
			await CreateTransactionAsync(AvalaraDocumentType.SalesOrder, orderSummary, overrideConfig);

		public async Task<OrderTaxCalculation> CommitTransactionAsync(OrderSummaryForTax orderSummary, OCIntegrationConfig overrideConfig = null) =>
			await CreateTransactionAsync(AvalaraDocumentType.SalesInvoice, orderSummary, overrideConfig);

		protected async Task<OrderTaxCalculation> CreateTransactionAsync(AvalaraDocumentType type, OrderSummaryForTax orderSummary, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<AvalaraConfig>(overrideConfig ?? _defaultConfig);
			var createTransaction = AvalaraRequestMapper.ToAvalaraTransactionModel(orderSummary, config.CompanyCode, type);
			var transaction = await AvalaraClient.CreateTransaction(createTransaction, config);
			var calculation = AvalaraResponseMapper.ToOrderTaxCalculation(transaction);
			return calculation;
		}

		public async Task<TaxCategorizationResponse> ListTaxCodesAsync(string filterTerm, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<AvalaraConfig>(overrideConfig ?? _defaultConfig);
			var filter = AvalaraTaxCodeMapper.MapFilterTerm(filterTerm);
			var codes = await AvalaraClient.ListTaxCodesAsync(filter, config);
			return new TaxCategorizationResponse()
			{
				Categories = AvalaraTaxCodeMapper.MapTaxCodes(codes)
			};
		}
	}
}
