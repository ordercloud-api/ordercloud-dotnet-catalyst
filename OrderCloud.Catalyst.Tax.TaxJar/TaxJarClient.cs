using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tax.TaxJar
{
	public class TaxJarClient
	{
		/// <summary>
		/// https://developers.taxjar.com/api/reference/#post-create-an-order-transaction
		/// </summary>
		public static async Task<TaxJarOrder> CreateOrderAsync(TaxJarOrder order, TaxJarConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var tax = await request
					.AppendPathSegments("v2", "taxes", "orders")
					.PostJsonAsync(order)
					.ReceiveJson<TaxJarOrder>();
				return tax;
			});
		}

		/// <summary>
		/// https://developers.taxjar.com/api/reference/#get-list-tax-categories
		/// </summary>
		public static async Task<TaxJarCategories> ListCategoriesAsync(TaxJarConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{ 
				var categories = await request
					.AppendPathSegments("v2", "categories")
					.GetJsonAsync<TaxJarCategories>();
				return categories;
			});
		}

		/// <summary>
		/// https://developers.taxjar.com/api/reference/#post-calculate-sales-tax-for-an-order
		/// </summary>
		public static async Task<TaxJarCalcResponse> CalcTaxForOrderAsync(TaxJarOrder order, TaxJarConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var tax = await request
					.AppendPathSegments("v2", "taxes")
					.PostJsonAsync(order)
					.ReceiveJson<TaxJarCalcResponse>();
				return tax;
			});
		}

		protected static async Task<T> TryCatchRequestAsync<T>(TaxJarConfig config, Func<IFlurlRequest, Task<T>> run)
		{
			var request = config.BaseUrl.WithOAuthBearerToken(config.APIToken);
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
				var body = await ex.Call.Response.GetJsonAsync<TaxJarError>();
				throw new IntegrationErrorResponseException(config, request.Url, (int)status, body);
			}
		}
	}
}
