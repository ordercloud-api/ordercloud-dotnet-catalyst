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

		public static async Task<List<Address>> ListAllAsync(this IAddressesResource resource, string buyerID, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Address>> ListAllAsync(this IAdminAddressesResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<UserGroup>> ListAllAsync(this IAdminUserGroupsResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<User>> ListAllAsync(this IAdminUsersResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<ApiClient>> ListAllAsync(this IApiClientsResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<ApprovalRule>> ListAllAsync(this IApprovalRulesResource resource, string buyerID, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Buyer>> ListAllAsync(this IBuyersResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Catalog>> ListAllAsync(this ICatalogsResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Category>> ListAllAsync(this ICategoriesResource resource, string catalogID, string depth = "1", string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(catalogID, depth, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<CostCenter>> ListAllAsync(this ICostCentersResource resource, string buyerID, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<CreditCard>> ListAllAsync(this ICreditCardsResource resource, string buyerID, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<ImpersonationConfig>> ListAllAsync(this IImpersonationConfigsResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Incrementor>> ListAllAsync(this IIncrementorsResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<IntegrationEvent>> ListAllAsync(this IIntegrationEventsResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{

			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<LineItem>> ListAllAsync(this ILineItemsResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(direction, orderID, search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<CostCenter>> ListAllCostCentersAsync(this IMeResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListCostCentersAsync(search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<UserGroup>> ListAllUserGroupsAsync(this IMeResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListUserGroupsAsync(search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<BuyerAddress>> ListAllAddressesAsync(this IMeResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAddressesAsync(search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<BuyerCreditCard>> ListAllCreditCardsAsync(this IMeResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListCreditCardsAsync(search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Category>> ListAllCategoriesAsync(this IMeResource resource, string depth = "1", string catalogID = null, string productID = null, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListCategoriesAsync(depth, catalogID, productID, search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<BuyerProduct>> ListAllProductsAsync(this IMeResource resource, string catalogID = null, string categoryID = null, string depth = null, string search = null, string searchOn = null, SearchType searchType = SearchType.AnyTerm, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterWithFacetsAsync((filter) =>
			{
				return resource.ListProductsAsync(catalogID, categoryID, depth, search, searchOn, searchType, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Spec>> ListAllSpecsAsync(this IMeResource resource, string productID, string catalogID = null, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListSpecsAsync(productID, catalogID, search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Order>> ListAllOrdersAsync(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListOrdersAsync(from, to, search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Order>> ListAllApprovableOrdersAsync(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListApprovableOrdersAsync(from, to, search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Promotion>> ListAllApprovableOrdersAsync(this IMeResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListPromotionsAsync(search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<SpendingAccount>> ListAllSpendingAccountsAsync(this IMeResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListSpendingAccountsAsync(search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Shipment>> ListAllShipmentsAsync(this IMeResource resource, string orderID = null, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListShipmentsAsync(orderID, search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<ShipmentItem>> ListAllShipmentItemsAsync(this IMeResource resource, string shipmentID, string orderID = null, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListShipmentItemsAsync(shipmentID, orderID, search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Catalog>> ListAllCatalogsAsync(this IMeResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListCatalogsAsync(search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<MessageSender>> ListAllAsync(this IMessageSendersResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<OpenIdConnect>> ListAllAsync(this IOpenIdConnectsResource resource, string search = null, string searchOn = null, string sortBy = null, int page = 1, int pageSize = 20, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, sortBy, page, pageSize, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Order>> ListAllAsync(this IOrdersResource resource, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(direction, buyerID, supplierID, from, to, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<OrderApproval>> ListAllApprovalsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListApprovalsAsync(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<User>> ListAllEligibleApproversAsync(this IOrdersResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListEligibleApproversAsync(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Shipment>> ListAllShipmentsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListShipmentsAsync(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<OrderPromotion>> ListAllPromotionsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListPromotionsAsync(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Payment>> ListAllAsync(this IPaymentsResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<PriceSchedule>> ListAllAsync(this IPriceSchedulesResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<ProductFacet>> ListAllAsync(this IProductFacetsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Product>> ListAllAsync(this IProductsResource resource, string catalogID = null, string categoryID = null, string supplierID = null, string search = null, string searchOn = null, SearchType searchType = SearchType.AnyTerm, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterWithFacetsAsync((filter) =>
			{
				return resource.ListAsync(catalogID, categoryID, supplierID, search, searchOn, searchType, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Variant>> ListAllVariantsAsync(this IProductsResource resource, string productID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListVariantsAsync(productID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Spec>> ListAllSpecsAsync(this IProductsResource resource, string productID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListSpecsAsync(productID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Supplier>> ListAllSuppliersAsync(this IProductsResource resource, string productID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListSuppliersAsync(productID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Promotion>> ListAllAsync(this IPromotionsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<SecurityProfile>> ListAllAsync(this ISecurityProfilesResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Shipment>> ListAllAsync(this IShipmentsResource resource, string orderID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<ShipmentItem>> ListAllItemsAsync(this IShipmentsResource resource, string shipmentID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListItemsAsync(shipmentID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Spec>> ListAllAsync(this ISpecsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<SpecOption>> ListAllOptionsAsync(this ISpecsResource resource, string specID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListOptionsAsync(specID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<SpendingAccount>> ListAllAsync(this ISpendingAccountsResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Address>> ListAllAsync(this ISupplierAddressesResource resource, string supplierID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(supplierID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Supplier>> ListAllAsync(this ISuppliersResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<UserGroup>> ListAllAsync(this ISupplierUserGroupsResource resource, string supplierID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(supplierID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<User>> ListAllAsync(this ISupplierUsersResource resource, string supplierID, string userGroupID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(supplierID, userGroupID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<UserGroup>> ListAllAsync(this IUserGroupsResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<User>> ListAllAsync(this IUsersResource resource, string buyerID, string userGroupID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, userGroupID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Webhook>> ListAllAsync(this IWebhooksResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<XpIndex>> ListAllAsync(this IXpIndicesResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}
	}
}
