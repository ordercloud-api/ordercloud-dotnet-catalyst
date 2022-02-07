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

		public TaxJarClient(TaxJarOCIntegrationConfig config)
		{
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
			var tax = await _flurl.Request("v2", "taxes").PostJsonAsync(order).ReceiveJson<TaxJarCalcResponse>();
			return tax;
		}
	}
}
