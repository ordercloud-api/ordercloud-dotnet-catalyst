using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public class TaxJarClient
	{
		protected readonly FlurlClient _flurl;
		protected readonly TaxJarConfig _config;

		public TaxJarClient(TaxJarConfig config)
		{
			_config = config;
			_flurl = new FlurlClient(config.BaseUrl).WithOAuthBearerToken(config.APIToken);
		}

		/// <summary>
		/// https://developers.taxjar.com/api/reference/#post-create-an-order-transaction
		/// </summary>
		public async Task<TaxJarOrder> CreateOrderAsync(TaxJarOrder order)
		{
			var tax = await _flurl.Request("v2", "taxes", "orders").PostJsonAsync(order).ReceiveJson<TaxJarOrder>();
			return tax;
		}

		/// <summary>
		/// https://developers.taxjar.com/api/reference/#get-list-tax-categories
		/// </summary>
		public async Task<TaxJarCategories> ListCategoriesAsync()
		{
			var categories = await _flurl.Request("v2", "categories").GetJsonAsync<TaxJarCategories>();
			return categories;
		}

		/// <summary>
		/// https://developers.taxjar.com/api/reference/#post-calculate-sales-tax-for-an-order
		/// </summary>
		public async Task<TaxJarCalcResponse> CalcTaxForOrderAsync(TaxJarOrder order)
		{
			var request = _flurl.Request("v2", "taxes");
			return await TryCatchRequestAsync(request, async () =>
			{
				var tax = await request.PostJsonAsync(order).ReceiveJson<TaxJarCalcResponse>();
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
