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

		public static async Task<List<Address>> ListAllAsync(this IAddressesResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IAddressesResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Address
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Address>> ListAllAsync(this IAdminAddressesResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IAdminAddressesResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Address
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<UserGroupAssignment>> ListAllUserAssignmentsAsync(this IAdminUserGroupsResource resource, string userGroupID = null, string userID = null,  string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListUserAssignmentsAsync(userGroupID, userID, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<UserGroup>> ListAllAsync(this IAdminUserGroupsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IAdminUserGroupsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : UserGroup
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<User>> ListAllAsync(this IAdminUsersResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IAdminUsersResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : User
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<ApiClient>> ListAllAsync(this IApiClientsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IApiClientsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : ApiClient
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<ApiClientAssignment>> ListAllAssignmentsAsync(this IApiClientsResource resource, string apiClientID = null, string buyerID = null, string supplierID = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListAssignmentsAsync(apiClientID, buyerID, supplierID, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<ApprovalRule>> ListAllAsync(this IApprovalRulesResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IApprovalRulesResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : ApprovalRule
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Buyer>> ListAllAsync(this IBuyersResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IBuyersResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T: Buyer
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Catalog>> ListAllAsync(this ICatalogsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this ICatalogsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Catalog
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<CatalogAssignment>> ListAllAssignmentsAsync(this ICatalogsResource resource, string catalogID = null, string buyerID = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListAssignmentsAsync(catalogID, buyerID, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<ProductCatalogAssignment>> ListAllProductAssignmentsAsync(this ICatalogsResource resource, string catalogID = null, string buyerID = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListProductAssignmentsAsync(catalogID, buyerID, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<Category>> ListAllAsync(this ICategoriesResource resource, string catalogID, string depth = "1", string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(catalogID, depth, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this ICategoriesResource resource, string catalogID, string depth = "1", string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Category
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(catalogID, depth, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<CategoryAssignment>> ListAllAssignmentsAsync(this ICategoriesResource resource, string catalogID, string categoryID = null, string buyerID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListAssignmentsAsync(catalogID, categoryID, buyerID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<CategoryProductAssignment>> ListAllProductAssignmentsAsync(this ICategoriesResource resource, string catalogID, string categoryID = null, string productID = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListProductAssignmentsAsync(catalogID, categoryID, productID, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<CostCenter>> ListAllAsync(this ICostCentersResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this ICostCentersResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : CostCenter
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<CostCenterAssignment>> ListAllAssignmentsAsync(this ICostCentersResource resource, string buyerID, string costCenterID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListAssignmentsAsync(buyerID, costCenterID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<CreditCard>> ListAllAsync(this ICreditCardsResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this ICreditCardsResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : CreditCard
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<CreditCardAssignment>> ListAllAssignmentsAsync(this ICreditCardsResource resource, string buyerID, string creditCardID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListAssignmentsAsync(buyerID, creditCardID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<ImpersonationConfig>> ListAllAsync(this IImpersonationConfigsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Incrementor>> ListAllAsync(this IIncrementorsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}


		public static async Task<List<IntegrationEvent>> ListAllAsync(this IIntegrationEventsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{

			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<LineItem>> ListAllAsync(this ILineItemsResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this ILineItemsResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : LineItem
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<CostCenter>> ListAllCostCentersAsync(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListCostCentersAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllCostCentersAsync<T>(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : CostCenter
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListCostCentersAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<UserGroup>> ListAllUserGroupsAsync(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListUserGroupsAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllUserGroupsAsync<T>(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : UserGroup
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListUserGroupsAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<BuyerAddress>> ListAllAddressesAsync(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAddressesAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAddressesAsync<T>(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : BuyerAddress 
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAddressesAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<BuyerCreditCard>> ListAllCreditCardsAsync(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListCreditCardsAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllCreditCardsAsync<T>(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : BuyerCreditCard
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListCreditCardsAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Category>> ListAllCategoriesAsync(this IMeResource resource, string depth = "1", string catalogID = null, string productID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListCategoriesAsync(depth, catalogID, productID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllCategoriesAsync<T>(this IMeResource resource, string depth = "1", string catalogID = null, string productID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Category
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListCategoriesAsync<T>(depth, catalogID, productID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<BuyerProduct>> ListAllProductsAsync(this IMeResource resource, string catalogID = null, string categoryID = null, string depth = null, string search = null, string searchOn = null, SearchType searchType = SearchType.AnyTerm, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterWithFacetsAsync((filter) =>
			{
				return resource.ListProductsAsync(catalogID, categoryID, depth, search, searchOn, searchType, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllProductsAsync<T>(this IMeResource resource, string catalogID = null, string categoryID = null, string depth = null, string search = null, string searchOn = null, SearchType searchType = SearchType.AnyTerm, object filters = null, string accessToken = null)
			where T : BuyerProduct
		{
			return await ListAllHelper.ListAllByFilterWithFacetsAsync((filter) =>
			{
				return resource.ListProductsAsync<T>(catalogID, categoryID, depth, search, searchOn, searchType, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Spec>> ListAllSpecsAsync(this IMeResource resource, string productID, string catalogID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListSpecsAsync(productID, catalogID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllSpecsAsync<T>(this IMeResource resource, string productID, string catalogID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Spec
		{
			return await ListAllHelper.ListAllByFilterAsync<T>((filter) =>
			{
				return resource.ListSpecsAsync<T>(productID, catalogID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Order>> ListAllOrdersAsync(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListOrdersAsync(from, to, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllOrdersAsync<T>(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Order
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListOrdersAsync<T>(from, to, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Order>> ListAllApprovableOrdersAsync(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListApprovableOrdersAsync(from, to, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllApprovableOrdersAsync<T>(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Order
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListApprovableOrdersAsync<T>(from, to, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Promotion>> ListAllPromotionsAsync(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListPromotionsAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllPromotionsAsync<T>(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Promotion
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListPromotionsAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<SpendingAccount>> ListAllSpendingAccountsAsync(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListSpendingAccountsAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllSpendingAccountsAsync<T>(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : SpendingAccount
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListSpendingAccountsAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Shipment>> ListAllShipmentsAsync(this IMeResource resource, string orderID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListShipmentsAsync(orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllShipmentsAsync<T>(this IMeResource resource, string orderID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Shipment
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListShipmentsAsync<T>(orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<ShipmentItem>> ListAllShipmentItemsAsync(this IMeResource resource, string shipmentID, string orderID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListShipmentItemsAsync(shipmentID, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllShipmentItemsAsync<T>(this IMeResource resource, string shipmentID, string orderID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : ShipmentItem
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListShipmentItemsAsync<T>(shipmentID, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Catalog>> ListAllCatalogsAsync(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListCatalogsAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllCatalogsAsync<T>(this IMeResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Catalog
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListCatalogsAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<MessageSender>> ListAllAsync(this IMessageSendersResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IMessageSendersResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : MessageSender
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<MessageSenderAssignment>> ListAllAssignmentsAsync(this IMessageSendersResource resource, string buyerID = null, string messageSenderID = null, string userID = null, string userGroupID = null, PartyType? level = null, string supplierID = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListAssignmentsAsync(buyerID, messageSenderID, userID, userGroupID, level, page, MAX_PAGE_SIZE, supplierID, accessToken);
			});
		}

		public static async Task<List<MessageCCListenerAssignment>> ListAllCCListenerAssignmentsAsync(this IMessageSendersResource resource, string search = null, string searchOn = null, string sortBy = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListCCListenerAssignmentsAsync(search, searchOn, sortBy, page, MAX_PAGE_SIZE, filters, accessToken);
			});
		}

		public static async Task<List<OpenIdConnect>> ListAllAsync(this IOpenIdConnectsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Order>> ListAllAsync(this IOrdersResource resource, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(direction, buyerID, supplierID, from, to, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IOrdersResource resource, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Order
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(direction, buyerID, supplierID, from, to, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<OrderApproval>> ListAllApprovalsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListApprovalsAsync(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllApprovalsAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : OrderApproval
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListApprovalsAsync<T>(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<User>> ListAllEligibleApproversAsync(this IOrdersResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListEligibleApproversAsync(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllEligibleApproversAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : User
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListEligibleApproversAsync<T>(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Shipment>> ListAllShipmentsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListShipmentsAsync(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllShipmentsAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Shipment
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListShipmentsAsync<T>(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<OrderPromotion>> ListAllPromotionsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListPromotionsAsync(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllPromotionsAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : OrderPromotion
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListPromotionsAsync<T>(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Payment>> ListAllAsync(this IPaymentsResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IPaymentsResource resource, OrderDirection direction, string orderID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Payment
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(direction, orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<PriceSchedule>> ListAllAsync(this IPriceSchedulesResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IPriceSchedulesResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : PriceSchedule
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<ProductFacet>> ListAllAsync(this IProductFacetsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IProductFacetsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : ProductFacet
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Product>> ListAllAsync(this IProductsResource resource, string catalogID = null, string categoryID = null, string supplierID = null, string search = null, string searchOn = null, SearchType searchType = SearchType.AnyTerm, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterWithFacetsAsync((filter) =>
			{
				return resource.ListAsync(catalogID, categoryID, supplierID, search, searchOn, searchType, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IProductsResource resource, string catalogID = null, string categoryID = null, string supplierID = null, string search = null, string searchOn = null, SearchType searchType = SearchType.AnyTerm, object filters = null, string accessToken = null)
			where T : Product
		{
			return await ListAllHelper.ListAllByFilterWithFacetsAsync((filter) =>
			{
				return resource.ListAsync<T>(catalogID, categoryID, supplierID, search, searchOn, searchType, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Variant>> ListAllVariantsAsync(this IProductsResource resource, string productID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListVariantsAsync(productID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllVariantsAsync<T>(this IProductsResource resource, string productID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Variant
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListVariantsAsync<T>(productID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Spec>> ListAllSpecsAsync(this IProductsResource resource, string productID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListSpecsAsync(productID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllSpecsAsync<T>(this IProductsResource resource, string productID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Spec
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListSpecsAsync<T>(productID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Supplier>> ListAllSuppliersAsync(this IProductsResource resource, string productID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListSuppliersAsync(productID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllSuppliersAsync<T>(this IProductsResource resource, string productID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Supplier
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListSuppliersAsync<T>(productID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<ProductAssignment>> ListAllAssignmentsAsync(this IProductsResource resource, string productID = null, string priceScheduleID = null, string buyerID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListAssignmentsAsync(productID, priceScheduleID, buyerID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<Promotion>> ListAllAsync(this IPromotionsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IPromotionsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Promotion
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<PromotionAssignment>> ListAllAssignmentsAsync(this IPromotionsResource resource, string buyerID = null, string promotionID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListAssignmentsAsync(buyerID, promotionID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<SecurityProfile>> ListAllAsync(this ISecurityProfilesResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<SecurityProfileAssignment>> ListAllAssignmentsAsync(this ISecurityProfilesResource resource, string buyerID = null, string supplierID = null, string securityProfileID = null, string userID = null, string userGroupID = null, CommerceRole? commerceRole = null, PartyType? level = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListAssignmentsAsync(buyerID, supplierID, securityProfileID, userID, userGroupID, commerceRole, level, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<Shipment>> ListAllAsync(this IShipmentsResource resource, string orderID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IShipmentsResource resource, string orderID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Shipment
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(orderID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<ShipmentItem>> ListAllItemsAsync(this IShipmentsResource resource, string shipmentID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListItemsAsync(shipmentID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllItemsAsync<T>(this IShipmentsResource resource, string shipmentID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : ShipmentItem
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListItemsAsync<T>(shipmentID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Spec>> ListAllAsync(this ISpecsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this ISpecsResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Spec
		{
			return await ListAllHelper.ListAllByFilterAsync<T>((filter) =>
			{
				return resource.ListAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<SpecOption>> ListAllOptionsAsync(this ISpecsResource resource, string specID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListOptionsAsync(specID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllOptionsAsync<T>(this ISpecsResource resource, string specID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : SpecOption
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListOptionsAsync<T>(specID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<SpecProductAssignment>> ListAllProductAssignmentsAsync(this ISpecsResource resource, string search = null, string searchOn = null, string sortBy = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListProductAssignmentsAsync(search, searchOn, sortBy, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<SpendingAccount>> ListAllAsync(this ISpendingAccountsResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this ISpendingAccountsResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : SpendingAccount
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<SpendingAccountAssignment>> ListAllAssignmentsAsync(this ISpendingAccountsResource resource, string buyerID, string spendingAccountID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListAssignmentsAsync(buyerID, spendingAccountID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<Address>> ListAllAsync(this ISupplierAddressesResource resource, string supplierID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(supplierID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this ISupplierAddressesResource resource, string supplierID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Address
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(supplierID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<Supplier>> ListAllAsync(this ISuppliersResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this ISuppliersResource resource, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : Supplier
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<UserGroup>> ListAllAsync(this ISupplierUserGroupsResource resource, string supplierID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(supplierID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this ISupplierUserGroupsResource resource, string supplierID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : UserGroup
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(supplierID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<UserGroupAssignment>> ListAllUserAssignmentsAsync(this ISupplierUserGroupsResource resource, string supplierID, string userGroupID = null, string userID = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListUserAssignmentsAsync(supplierID, userGroupID, userID, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<User>> ListAllAsync(this ISupplierUsersResource resource, string supplierID, string userGroupID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(supplierID, userGroupID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this ISupplierUsersResource resource, string supplierID, string userGroupID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : User
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(supplierID, userGroupID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<UserGroup>> ListAllAsync(this IUserGroupsResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IUserGroupsResource resource, string buyerID, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : UserGroup
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(buyerID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<UserGroupAssignment>> ListAllUserAssignmentsAsync(this IUserGroupsResource resource, string buyerID, string userGroupID = null, string userID = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByPage((page) =>
			{
				return resource.ListUserAssignmentsAsync(buyerID, userGroupID, userID, page, MAX_PAGE_SIZE, accessToken);
			});
		}

		public static async Task<List<User>> ListAllAsync(this IUsersResource resource, string buyerID, string userGroupID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync(buyerID, userGroupID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
		}

		public static async Task<List<T>> ListAllAsync<T>(this IUsersResource resource, string buyerID, string userGroupID = null, string search = null, string searchOn = null, object filters = null, string accessToken = null)
			where T : User
		{
			return await ListAllHelper.ListAllByFilterAsync((filter) =>
			{
				return resource.ListAsync<T>(buyerID, userGroupID, search, searchOn, SORT, 1, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
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
