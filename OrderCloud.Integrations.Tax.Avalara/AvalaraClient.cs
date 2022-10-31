using Flurl.Http;
using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Integrations.Tax.Avalara
{
	public class AvalaraClient
	{
		protected static IFlurlRequest BuildClient(AvalaraConfig config) => config.BaseUrl.WithBasicAuth(config.AccountID, config.LicenseKey);

		/// <summary>
		/// https://developer.avalara.com/api-reference/avatax/rest/v2/methods/Definitions/ListTaxCodes/
		/// </summary>
		public static async Task<List<AvalaraTaxCode>> ListTaxCodesAsync(string filterParam, AvalaraConfig config)
		{			
			var tax = await BuildClient(config)
				.AppendPathSegments("definitions", "taxcodes")
				.SetQueryParam("$filter", filterParam)
				.GetJsonWithErrorHandlingAsync<AvalaraFetchResult<AvalaraTaxCode>, AvalaraError>(config);
			return tax.value;
		}

		/// <summary>
		/// https://developer.avalara.com/api-reference/avatax/rest/v2/methods/Transactions/CreateTransaction/
		/// </summary>
		public static async Task<AvalaraTransactionModel> CreateTransaction(AvalaraCreateTransactionModel transaction, AvalaraConfig config)
		{			
			var tax = await BuildClient(config)
				.AppendPathSegments("transactions", "create")
				.PostJsonWithErrorHandlingAsync<AvalaraError>(transaction, config)
				.ReceiveJson<AvalaraTransactionModel>();
			return tax;
		}
	}
}
