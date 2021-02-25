using Flurl.Util;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public static class ListAllExtensions
	{
		public const int MAX_PAGE_SIZE = 100;
		public const string SORT = "ID";

		private static List<KeyValuePair<string, object>> AndFilter(this object filters, KeyValuePair<string, object> filter)
		{
			var filterList = filters?.ToKeyValuePairs()?.ToList() ?? new List<KeyValuePair<string, object>>();
			filterList.Add(filter);
			return filterList;
		}

		public static async Task<List<Order>> ListAllAsync(this IOrdersResource resource, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) => {
				return resource.ListAsync(
					direction, buyerID, supplierID, from, to, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken
				);
			});
		}
	}
}
