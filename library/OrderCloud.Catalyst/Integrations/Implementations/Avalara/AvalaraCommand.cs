using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public class AvalaraCommand : OCIntegrationCommand , ITaxCodesProvider
	{
		protected readonly AvalaraConfig _config;
		protected readonly AvalaraClient _client;

		public AvalaraCommand(AvalaraConfig config) : base(config)
		{
			_config = config;
			_client = new AvalaraClient(config);
		}

		public async Task<TaxCategorizationResponse> ListTaxCodesAsync(string filterTerm)
		{
			throw new NotImplementedException();
		}
	}
}
