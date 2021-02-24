using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public static class ListAllExtensions
	{
		public static async Task<List<Order>> ListAllAsync(this IOrdersResource resource, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
				resource.ListAsync(direction, buyerID, supplierID, from, to, search, searchOn, "ID", 1, ListAllHelper.MAX_PAGE_SIZE, filters, accessToken));
		}
	}
}
