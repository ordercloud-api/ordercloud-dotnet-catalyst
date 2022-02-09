using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public class AvalaraClient
	{
		protected readonly FlurlClient _flurl;
		protected readonly AvalaraConfig _config;

		public AvalaraClient(AvalaraConfig config)
		{
			_config = config;
			_flurl = new FlurlClient(config.BaseUrl).WithBasicAuth(config.AccountID, config.LicenseKey);
		}

		/// <summary>
		/// https://developer.avalara.com/api-reference/avatax/rest/v2/methods/Definitions/ListTaxCodes/
		/// </summary>
		public async Task<List<AvalaraTaxCode>> ListTaxCodesAsync(string filterParam)
		{
			var request = _flurl.Request("api", "v2", "definitions", "taxcodes");
			return await TryCatchRequestAsync(request, async () =>
			{
				var tax = await request.SetQueryParam("$filter", filterParam).GetJsonAsync<AvalaraFetchResult<AvalaraTaxCode>>();
				return tax.value;
			});
		}

		/// <summary>
		/// https://developer.avalara.com/api-reference/avatax/rest/v2/methods/Transactions/CreateTransaction/
		/// </summary>
		public async Task<AvalaraTransactionModel> CreateTransaction(AvalaraCreateTransactionModel transaction)
		{
			var request = _flurl.Request("api", "v2", "transactions", "create");
			return await TryCatchRequestAsync(request, async () =>
			{
				var tax = await request.PostJsonAsync(transaction).ReceiveJson<AvalaraTransactionModel>();
				return tax;
			});
		}

		protected async Task<T> TryCatchRequestAsync<T>(IFlurlRequest request, Func<Task<T>> run)
		{
			try
			{
				return await run();
			}
			catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
			{
				// candidate for retry here?
				throw new IntegrationNoResponseException(_config, request.Url);
			}
			catch (FlurlHttpException ex)
			{
				var status = ex?.Call?.Response?.StatusCode;
				if (status == null) // simulate by putting laptop on airplane mode
				{
					throw new IntegrationNoResponseException(_config, request.Url);
				}
				if (status == 401)
				{
					throw new IntegrationAuthFailedException(_config, request.Url, (int)status);
				}
				var body = await ex.Call.Response.GetJsonAsync();
				throw new IntegrationErrorResponseException(_config, request.Url, (int)status, body);
			}
		}
	}
}
