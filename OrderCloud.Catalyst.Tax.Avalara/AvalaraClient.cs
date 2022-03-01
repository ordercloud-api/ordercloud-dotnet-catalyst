using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tax.Avalara
{
	public class AvalaraClient
	{
		/// <summary>
		/// https://developer.avalara.com/api-reference/avatax/rest/v2/methods/Definitions/ListTaxCodes/
		/// </summary>
		public static async Task<List<AvalaraTaxCode>> ListTaxCodesAsync(string filterParam, AvalaraConfig config)
		{
			
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var tax = await request
					.AppendPathSegments("definitions", "taxcodes")
					.SetQueryParam("$filter", filterParam)
					.GetJsonAsync<AvalaraFetchResult<AvalaraTaxCode>>();
				return tax.value;
			});
		}

		/// <summary>
		/// https://developer.avalara.com/api-reference/avatax/rest/v2/methods/Transactions/CreateTransaction/
		/// </summary>
		public static async Task<AvalaraTransactionModel> CreateTransaction(AvalaraCreateTransactionModel transaction, AvalaraConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var tax = await request
					.AppendPathSegments("transactions", "create")
					.PostJsonAsync(transaction).ReceiveJson<AvalaraTransactionModel>();
				return tax;
			});
		}

		protected static async Task<T> TryCatchRequestAsync<T>(AvalaraConfig config, Func<IFlurlRequest, Task<T>> run)
		{
			var request = config.BaseUrl.WithBasicAuth(config.AccountID, config.LicenseKey);
			try
			{
				return await run(request);
			}
			catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
			{
				// candidate for retry here?
				throw new IntegrationNoResponseException(config, request.Url);
			}
			catch (FlurlHttpException ex)
			{
				var status = ex?.Call?.Response?.StatusCode;
				if (status == null) // simulate by putting laptop on airplane mode
				{
					throw new IntegrationNoResponseException(config, request.Url);
				}
				if (status == 401)
				{
					throw new IntegrationAuthFailedException(config, request.Url, (int)status);
				}
				var body = await ex.Call.Response.GetJsonAsync();
				throw new IntegrationErrorResponseException(config, request.Url, (int)status, body);
			}
		}
	}
}
