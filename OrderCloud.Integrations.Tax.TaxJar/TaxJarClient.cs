using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Tax.TaxJar
{
	public class TaxJarClient
	{
		protected static IFlurlRequest BuildClient(TaxJarConfig config) => config.BaseUrl.WithOAuthBearerToken(config.APIToken);

		/// <summary>
		/// https://developers.taxjar.com/api/reference/#post-create-an-order-transaction
		/// </summary>
		public static async Task<TaxJarOrder> CreateOrderAsync(TaxJarOrder order, TaxJarConfig config)
		{
			var tax = await BuildClient(config)
				.AppendPathSegments("taxes", "orders")
				.PostJsonWithErrorHandlingAsync<TaxJarError>(order, config)
				.ReceiveJson<TaxJarOrder>();
			return tax;
		}

		/// <summary>
		/// https://developers.taxjar.com/api/reference/#get-list-tax-categories
		/// </summary>
		public static async Task<TaxJarCategories> ListCategoriesAsync(TaxJarConfig config)
		{
			var categories = await BuildClient(config)
				.AppendPathSegments("categories")
				.GetJsonWithErrorHandlingAsync<TaxJarCategories, TaxJarError>(config);
			return categories;
		}

		/// <summary>
		/// https://developers.taxjar.com/api/reference/#post-calculate-sales-tax-for-an-order
		/// </summary>
		public static async Task<TaxJarCalcResponse> CalcTaxForOrderAsync(TaxJarOrder order, TaxJarConfig config)
		{
			var tax = await BuildClient(config)
				.AppendPathSegments("taxes")
				.PostJsonWithErrorHandlingAsync<TaxJarError>(order, config)
				.ReceiveJson<TaxJarCalcResponse>();
			return tax;
		}
	}
}



