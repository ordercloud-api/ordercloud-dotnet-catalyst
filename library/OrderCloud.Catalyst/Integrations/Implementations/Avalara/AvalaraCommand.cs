using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public class AvalaraCommand : OCIntegrationCommand , ITaxCodesProvider, ITaxCalculator
	{
		protected readonly AvalaraConfig _config;
		protected readonly AvalaraClient _client;

		public AvalaraCommand(AvalaraConfig config) : base(config)
		{
			_config = config;
			_client = new AvalaraClient(config);
		}

		public async Task<OrderTaxCalculation> CalculateEstimateAsync(OrderSummaryForTax orderSummary) => 
			await CreateTransactionAsync(AvalaraDocumentType.SalesOrder, orderSummary);

		public async Task<OrderTaxCalculation> CommitTransactionAsync(OrderSummaryForTax orderSummary) =>
			await CreateTransactionAsync(AvalaraDocumentType.SalesInvoice, orderSummary);

		protected async Task<OrderTaxCalculation> CreateTransactionAsync(AvalaraDocumentType type, OrderSummaryForTax orderSummary)
		{
			var createTransaction = AvalaraRequestMapper.ToAvalaraTransactionModel(orderSummary, _config.CompanyCode, type);
			var transaction = await _client.CreateTransaction(createTransaction);
			var calculation = AvalaraResponseMapper.ToOrderTaxCalculation(transaction);
			return calculation;
		}

		public async Task<TaxCategorizationResponse> ListTaxCodesAsync(string filterTerm)
		{
			var filter = AvalaraTaxCodeMapper.MapFilterTerm(filterTerm);
			var codes = await _client.ListTaxCodesAsync(filter);
			return new TaxCategorizationResponse()
			{
				Categories = AvalaraTaxCodeMapper.MapTaxCodes(codes)
			};
		}
	}
}
