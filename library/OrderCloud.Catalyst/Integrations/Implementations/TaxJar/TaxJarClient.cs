using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public class TaxJarClient
	{
		protected readonly FlurlClient _flurl;
		protected readonly TaxJarOCIntegrationConfig _config;

		public TaxJarClient(TaxJarOCIntegrationConfig config)
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
				if (ex.Call.Response == null || ex.Call.Response.StatusCode > 500)  // simulate by putting laptop on airplane mode
				{
					// candidate for retry here?
					throw new IntegrationNoResponseException(_config, request.Url);
				}
				if (ex.Call.Response.StatusCode == 401)
				{
					throw new IntegrationAuthFailedException(_config, request.Url);
				}
				var body = await ex.Call.Response.GetJsonAsync<TaxJarError>();
				throw new IntegrationErrorResponseException(_config, request.Url, body);
			}
		}
	}
}
