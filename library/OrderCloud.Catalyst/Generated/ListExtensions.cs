// This file is generated automatically, do not edit directly. See codegen/templates/ListExtensions.cs.hbs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public static class ListExtensions
	{
               
        public static async Task<ListPage<SecurityProfile>> ListAsync(this ISecurityProfilesResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<SecurityProfile>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
               
        public static async Task<ListPage<SecurityProfileAssignment>> ListAssignmentsAsync(this ISecurityProfilesResource resource, string buyerID = null, string supplierID = null, string securityProfileID = null, string userID = null, string userGroupID = null, CommerceRole? commerceRole = null, PartyType? level = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<SecurityProfileAssignment>();
			return await resource.ListAssignmentsAsync(buyerID, supplierID, securityProfileID, userID, userGroupID, commerceRole, level, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<ImpersonationConfig>> ListAsync(this IImpersonationConfigsResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<ImpersonationConfig>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
               
        public static async Task<ListPage<OpenIdConnect>> ListAsync(this IOpenIdConnectsResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<OpenIdConnect>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
               
        public static async Task<ListPage<User>> ListAsync(this IAdminUsersResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<User>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IAdminUsersResource resource, IListArgs args = null, string accessToken = null) 
            where T : User
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<UserGroup>> ListAsync(this IAdminUserGroupsResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<UserGroup>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IAdminUserGroupsResource resource, IListArgs args = null, string accessToken = null) 
            where T : UserGroup
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<UserGroupAssignment>> ListUserAssignmentsAsync(this IAdminUserGroupsResource resource, string userGroupID = null, string userID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<UserGroupAssignment>();
			return await resource.ListUserAssignmentsAsync(userGroupID, userID, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<Address>> ListAsync(this IAdminAddressesResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Address>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IAdminAddressesResource resource, IListArgs args = null, string accessToken = null) 
            where T : Address
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<MessageSender>> ListAsync(this IMessageSendersResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<MessageSender>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IMessageSendersResource resource, IListArgs args = null, string accessToken = null) 
            where T : MessageSender
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<MessageSenderAssignment>> ListAssignmentsAsync(this IMessageSendersResource resource, string buyerID = null, string messageSenderID = null, string userID = null, string userGroupID = null, PartyType? level = null, string supplierID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<MessageSenderAssignment>();
			return await resource.ListAssignmentsAsync(buyerID, messageSenderID, userID, userGroupID, level, args.Page, args.PageSize, supplierID, accessToken);
        }
        
               
        public static async Task<ListPage<MessageCCListenerAssignment>> ListCCListenerAssignmentsAsync(this IMessageSendersResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<MessageCCListenerAssignment>();
			return await resource.ListCCListenerAssignmentsAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
               
        public static async Task<ListPage<ApiClient>> ListAsync(this IApiClientsResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<ApiClient>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IApiClientsResource resource, IListArgs args = null, string accessToken = null) 
            where T : ApiClient
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<ApiClientAssignment>> ListAssignmentsAsync(this IApiClientsResource resource, string apiClientID = null, string buyerID = null, string supplierID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<ApiClientAssignment>();
			return await resource.ListAssignmentsAsync(apiClientID, buyerID, supplierID, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<Incrementor>> ListAsync(this IIncrementorsResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Incrementor>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
               
        public static async Task<ListPage<IntegrationEvent>> ListAsync(this IIntegrationEventsResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<IntegrationEvent>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
               
        public static async Task<ListPage<Webhook>> ListAsync(this IWebhooksResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Webhook>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
               
        public static async Task<ListPage<XpIndex>> ListAsync(this IXpIndicesResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<XpIndex>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
               
        public static async Task<ListPage<Buyer>> ListAsync(this IBuyersResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Buyer>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IBuyersResource resource, IListArgs args = null, string accessToken = null) 
            where T : Buyer
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<User>> ListAsync(this IUsersResource resource, string buyerID, string userGroupID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<User>();
			return await resource.ListAsync(buyerID, userGroupID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IUsersResource resource, string buyerID, string userGroupID = null, IListArgs args = null, string accessToken = null) 
            where T : User
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(buyerID, userGroupID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<UserGroup>> ListAsync(this IUserGroupsResource resource, string buyerID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<UserGroup>();
			return await resource.ListAsync(buyerID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IUserGroupsResource resource, string buyerID, IListArgs args = null, string accessToken = null) 
            where T : UserGroup
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(buyerID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<UserGroupAssignment>> ListUserAssignmentsAsync(this IUserGroupsResource resource, string buyerID, string userGroupID = null, string userID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<UserGroupAssignment>();
			return await resource.ListUserAssignmentsAsync(buyerID, userGroupID, userID, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<Address>> ListAsync(this IAddressesResource resource, string buyerID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Address>();
			return await resource.ListAsync(buyerID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IAddressesResource resource, string buyerID, IListArgs args = null, string accessToken = null) 
            where T : Address
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(buyerID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<AddressAssignment>> ListAssignmentsAsync(this IAddressesResource resource, string buyerID, string addressID = null, string userID = null, string userGroupID = null, PartyType? level = null, bool? isShipping = null, bool? isBilling = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<AddressAssignment>();
			return await resource.ListAssignmentsAsync(buyerID, addressID, userID, userGroupID, level, isShipping, isBilling, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<CostCenter>> ListAsync(this ICostCentersResource resource, string buyerID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<CostCenter>();
			return await resource.ListAsync(buyerID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this ICostCentersResource resource, string buyerID, IListArgs args = null, string accessToken = null) 
            where T : CostCenter
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(buyerID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<CostCenterAssignment>> ListAssignmentsAsync(this ICostCentersResource resource, string buyerID, string costCenterID = null, string userID = null, string userGroupID = null, PartyType? level = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<CostCenterAssignment>();
			return await resource.ListAssignmentsAsync(buyerID, costCenterID, userID, userGroupID, level, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<CreditCard>> ListAsync(this ICreditCardsResource resource, string buyerID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<CreditCard>();
			return await resource.ListAsync(buyerID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this ICreditCardsResource resource, string buyerID, IListArgs args = null, string accessToken = null) 
            where T : CreditCard
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(buyerID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<CreditCardAssignment>> ListAssignmentsAsync(this ICreditCardsResource resource, string buyerID, string creditCardID = null, string userID = null, string userGroupID = null, PartyType? level = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<CreditCardAssignment>();
			return await resource.ListAssignmentsAsync(buyerID, creditCardID, userID, userGroupID, level, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<SpendingAccount>> ListAsync(this ISpendingAccountsResource resource, string buyerID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<SpendingAccount>();
			return await resource.ListAsync(buyerID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this ISpendingAccountsResource resource, string buyerID, IListArgs args = null, string accessToken = null) 
            where T : SpendingAccount
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(buyerID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<SpendingAccountAssignment>> ListAssignmentsAsync(this ISpendingAccountsResource resource, string buyerID, string spendingAccountID = null, string userID = null, string userGroupID = null, PartyType? level = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<SpendingAccountAssignment>();
			return await resource.ListAssignmentsAsync(buyerID, spendingAccountID, userID, userGroupID, level, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<ApprovalRule>> ListAsync(this IApprovalRulesResource resource, string buyerID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<ApprovalRule>();
			return await resource.ListAsync(buyerID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IApprovalRulesResource resource, string buyerID, IListArgs args = null, string accessToken = null) 
            where T : ApprovalRule
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(buyerID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Supplier>> ListAsync(this ISuppliersResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Supplier>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this ISuppliersResource resource, IListArgs args = null, string accessToken = null) 
            where T : Supplier
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<SupplierBuyer>> ListBuyersAsync(this ISuppliersResource resource, string supplierID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<SupplierBuyer>();
			return await resource.ListBuyersAsync(supplierID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
               
        public static async Task<ListPage<User>> ListAsync(this ISupplierUsersResource resource, string supplierID, string userGroupID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<User>();
			return await resource.ListAsync(supplierID, userGroupID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this ISupplierUsersResource resource, string supplierID, string userGroupID = null, IListArgs args = null, string accessToken = null) 
            where T : User
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(supplierID, userGroupID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<UserGroup>> ListAsync(this ISupplierUserGroupsResource resource, string supplierID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<UserGroup>();
			return await resource.ListAsync(supplierID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this ISupplierUserGroupsResource resource, string supplierID, IListArgs args = null, string accessToken = null) 
            where T : UserGroup
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(supplierID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<UserGroupAssignment>> ListUserAssignmentsAsync(this ISupplierUserGroupsResource resource, string supplierID, string userGroupID = null, string userID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<UserGroupAssignment>();
			return await resource.ListUserAssignmentsAsync(supplierID, userGroupID, userID, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<Address>> ListAsync(this ISupplierAddressesResource resource, string supplierID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Address>();
			return await resource.ListAsync(supplierID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this ISupplierAddressesResource resource, string supplierID, IListArgs args = null, string accessToken = null) 
            where T : Address
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(supplierID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Catalog>> ListAsync(this ICatalogsResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Catalog>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this ICatalogsResource resource, IListArgs args = null, string accessToken = null) 
            where T : Catalog
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<CatalogAssignment>> ListAssignmentsAsync(this ICatalogsResource resource, string catalogID = null, string buyerID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<CatalogAssignment>();
			return await resource.ListAssignmentsAsync(catalogID, buyerID, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<ProductCatalogAssignment>> ListProductAssignmentsAsync(this ICatalogsResource resource, string catalogID = null, string productID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<ProductCatalogAssignment>();
			return await resource.ListProductAssignmentsAsync(catalogID, productID, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<Category>> ListAsync(this ICategoriesResource resource, string catalogID, string depth = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Category>();
			return await resource.ListAsync(catalogID, depth, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this ICategoriesResource resource, string catalogID, string depth = null, IListArgs args = null, string accessToken = null) 
            where T : Category
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(catalogID, depth, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<CategoryAssignment>> ListAssignmentsAsync(this ICategoriesResource resource, string catalogID, string categoryID = null, string buyerID = null, string userID = null, string userGroupID = null, PartyType? level = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<CategoryAssignment>();
			return await resource.ListAssignmentsAsync(catalogID, categoryID, buyerID, userID, userGroupID, level, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<CategoryProductAssignment>> ListProductAssignmentsAsync(this ICategoriesResource resource, string catalogID, string categoryID = null, string productID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<CategoryProductAssignment>();
			return await resource.ListProductAssignmentsAsync(catalogID, categoryID, productID, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPageWithFacets<Product>> ListAsync(this IProductsResource resource, string catalogID = null, string categoryID = null, string supplierID = null, SearchArgs<Product> args = null, string accessToken = null) 
        {
            args = args ?? new SearchArgs<Product>();
			return await resource.ListAsync(catalogID, categoryID, supplierID, args.Search, args.SearchOn, args.SearchType, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPageWithFacets<T>> ListAsync<T>(this IProductsResource resource, string catalogID = null, string categoryID = null, string supplierID = null, SearchArgs<T> args = null, string accessToken = null) 
            where T : Product
        {
            args = args ?? new SearchArgs<T>();
			return await resource.ListAsync<T>(catalogID, categoryID, supplierID, args.Search, args.SearchOn, args.SearchType, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Spec>> ListSpecsAsync(this IProductsResource resource, string productID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Spec>();
			return await resource.ListSpecsAsync(productID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListSpecsAsync<T>(this IProductsResource resource, string productID, IListArgs args = null, string accessToken = null) 
            where T : Spec
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListSpecsAsync<T>(productID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<ProductSupplier>> ListSuppliersAsync(this IProductsResource resource, string productID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<ProductSupplier>();
			return await resource.ListSuppliersAsync(productID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListSuppliersAsync<T>(this IProductsResource resource, string productID, IListArgs args = null, string accessToken = null) 
            where T : ProductSupplier
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListSuppliersAsync<T>(productID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Variant>> ListVariantsAsync(this IProductsResource resource, string productID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Variant>();
			return await resource.ListVariantsAsync(productID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListVariantsAsync<T>(this IProductsResource resource, string productID, IListArgs args = null, string accessToken = null) 
            where T : Variant
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListVariantsAsync<T>(productID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<ProductAssignment>> ListAssignmentsAsync(this IProductsResource resource, string productID = null, string priceScheduleID = null, string buyerID = null, string userID = null, string userGroupID = null, PartyType? level = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<ProductAssignment>();
			return await resource.ListAssignmentsAsync(productID, priceScheduleID, buyerID, userID, userGroupID, level, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<PriceSchedule>> ListAsync(this IPriceSchedulesResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<PriceSchedule>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IPriceSchedulesResource resource, IListArgs args = null, string accessToken = null) 
            where T : PriceSchedule
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Spec>> ListAsync(this ISpecsResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Spec>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this ISpecsResource resource, IListArgs args = null, string accessToken = null) 
            where T : Spec
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<SpecOption>> ListOptionsAsync(this ISpecsResource resource, string specID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<SpecOption>();
			return await resource.ListOptionsAsync(specID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListOptionsAsync<T>(this ISpecsResource resource, string specID, IListArgs args = null, string accessToken = null) 
            where T : SpecOption
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListOptionsAsync<T>(specID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<SpecProductAssignment>> ListProductAssignmentsAsync(this ISpecsResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<SpecProductAssignment>();
			return await resource.ListProductAssignmentsAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
               
        public static async Task<ListPage<ProductFacet>> ListAsync(this IProductFacetsResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<ProductFacet>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IProductFacetsResource resource, IListArgs args = null, string accessToken = null) 
            where T : ProductFacet
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Order>> ListAsync(this IOrdersResource resource, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Order>();
			return await resource.ListAsync(direction, buyerID, supplierID, from, to, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IOrdersResource resource, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, IListArgs args = null, string accessToken = null) 
            where T : Order
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(direction, buyerID, supplierID, from, to, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<OrderApproval>> ListApprovalsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<OrderApproval>();
			return await resource.ListApprovalsAsync(direction, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListApprovalsAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, IListArgs args = null, string accessToken = null) 
            where T : OrderApproval
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListApprovalsAsync<T>(direction, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<User>> ListEligibleApproversAsync(this IOrdersResource resource, OrderDirection direction, string orderID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<User>();
			return await resource.ListEligibleApproversAsync(direction, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListEligibleApproversAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, IListArgs args = null, string accessToken = null) 
            where T : User
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListEligibleApproversAsync<T>(direction, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<OrderPromotion>> ListPromotionsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<OrderPromotion>();
			return await resource.ListPromotionsAsync(direction, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListPromotionsAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, IListArgs args = null, string accessToken = null) 
            where T : OrderPromotion
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListPromotionsAsync<T>(direction, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Shipment>> ListShipmentsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Shipment>();
			return await resource.ListShipmentsAsync(direction, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListShipmentsAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, IListArgs args = null, string accessToken = null) 
            where T : Shipment
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListShipmentsAsync<T>(direction, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<LineItem>> ListAsync(this ILineItemsResource resource, OrderDirection direction, string orderID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<LineItem>();
			return await resource.ListAsync(direction, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this ILineItemsResource resource, OrderDirection direction, string orderID, IListArgs args = null, string accessToken = null) 
            where T : LineItem
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(direction, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Promotion>> ListAsync(this IPromotionsResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Promotion>();
			return await resource.ListAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IPromotionsResource resource, IListArgs args = null, string accessToken = null) 
            where T : Promotion
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<PromotionAssignment>> ListAssignmentsAsync(this IPromotionsResource resource, string buyerID = null, string promotionID = null, string userID = null, string userGroupID = null, PartyType? level = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<PromotionAssignment>();
			return await resource.ListAssignmentsAsync(buyerID, promotionID, userID, userGroupID, level, args.Page, args.PageSize, accessToken);
        }
        
               
        public static async Task<ListPage<Payment>> ListAsync(this IPaymentsResource resource, OrderDirection direction, string orderID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Payment>();
			return await resource.ListAsync(direction, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IPaymentsResource resource, OrderDirection direction, string orderID, IListArgs args = null, string accessToken = null) 
            where T : Payment
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(direction, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Shipment>> ListAsync(this IShipmentsResource resource, string orderID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Shipment>();
			return await resource.ListAsync(orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAsync<T>(this IShipmentsResource resource, string orderID = null, IListArgs args = null, string accessToken = null) 
            where T : Shipment
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAsync<T>(orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<ShipmentItem>> ListItemsAsync(this IShipmentsResource resource, string shipmentID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<ShipmentItem>();
			return await resource.ListItemsAsync(shipmentID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListItemsAsync<T>(this IShipmentsResource resource, string shipmentID, IListArgs args = null, string accessToken = null) 
            where T : ShipmentItem
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListItemsAsync<T>(shipmentID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<BuyerAddress>> ListAddressesAsync(this IMeResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<BuyerAddress>();
			return await resource.ListAddressesAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListAddressesAsync<T>(this IMeResource resource, IListArgs args = null, string accessToken = null) 
            where T : BuyerAddress
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListAddressesAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Catalog>> ListCatalogsAsync(this IMeResource resource, string sellerID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Catalog>();
			return await resource.ListCatalogsAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), sellerID, accessToken);
        }
        
        public static async Task<ListPage<T>> ListCatalogsAsync<T>(this IMeResource resource, string sellerID = null, IListArgs args = null, string accessToken = null) 
            where T : Catalog
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListCatalogsAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), sellerID, accessToken);
        }       
        public static async Task<ListPage<Category>> ListCategoriesAsync(this IMeResource resource, string depth = null, string catalogID = null, string productID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Category>();
			return await resource.ListCategoriesAsync(depth, catalogID, productID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListCategoriesAsync<T>(this IMeResource resource, string depth = null, string catalogID = null, string productID = null, IListArgs args = null, string accessToken = null) 
            where T : Category
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListCategoriesAsync<T>(depth, catalogID, productID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<CostCenter>> ListCostCentersAsync(this IMeResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<CostCenter>();
			return await resource.ListCostCentersAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListCostCentersAsync<T>(this IMeResource resource, IListArgs args = null, string accessToken = null) 
            where T : CostCenter
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListCostCentersAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<BuyerCreditCard>> ListCreditCardsAsync(this IMeResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<BuyerCreditCard>();
			return await resource.ListCreditCardsAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListCreditCardsAsync<T>(this IMeResource resource, IListArgs args = null, string accessToken = null) 
            where T : BuyerCreditCard
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListCreditCardsAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Order>> ListOrdersAsync(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Order>();
			return await resource.ListOrdersAsync(from, to, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListOrdersAsync<T>(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, IListArgs args = null, string accessToken = null) 
            where T : Order
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListOrdersAsync<T>(from, to, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Order>> ListApprovableOrdersAsync(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Order>();
			return await resource.ListApprovableOrdersAsync(from, to, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListApprovableOrdersAsync<T>(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, IListArgs args = null, string accessToken = null) 
            where T : Order
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListApprovableOrdersAsync<T>(from, to, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPageWithFacets<BuyerProduct>> ListProductsAsync(this IMeResource resource, string catalogID = null, string categoryID = null, string depth = null, string sellerID = null, SearchArgs<BuyerProduct> args = null, string accessToken = null) 
        {
            args = args ?? new SearchArgs<BuyerProduct>();
			return await resource.ListProductsAsync(catalogID, categoryID, depth, args.Search, args.SearchOn, args.SearchType, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), sellerID, accessToken);
        }
        
        public static async Task<ListPageWithFacets<T>> ListProductsAsync<T>(this IMeResource resource, string catalogID = null, string categoryID = null, string depth = null, string sellerID = null, SearchArgs<T> args = null, string accessToken = null) 
            where T : BuyerProduct
        {
            args = args ?? new SearchArgs<T>();
			return await resource.ListProductsAsync<T>(catalogID, categoryID, depth, args.Search, args.SearchOn, args.SearchType, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), sellerID, accessToken);
        }       
        public static async Task<ListPage<Spec>> ListSpecsAsync(this IMeResource resource, string productID, string catalogID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Spec>();
			return await resource.ListSpecsAsync(productID, catalogID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListSpecsAsync<T>(this IMeResource resource, string productID, string catalogID = null, IListArgs args = null, string accessToken = null) 
            where T : Spec
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListSpecsAsync<T>(productID, catalogID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Variant>> ListVariantsAsync(this IMeResource resource, string productID, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Variant>();
			return await resource.ListVariantsAsync(productID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListVariantsAsync<T>(this IMeResource resource, string productID, IListArgs args = null, string accessToken = null) 
            where T : Variant
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListVariantsAsync<T>(productID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<Promotion>> ListPromotionsAsync(this IMeResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Promotion>();
			return await resource.ListPromotionsAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListPromotionsAsync<T>(this IMeResource resource, IListArgs args = null, string accessToken = null) 
            where T : Promotion
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListPromotionsAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<BuyerSupplier>> ListBuyerSellersAsync(this IMeResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<BuyerSupplier>();
			return await resource.ListBuyerSellersAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
               
        public static async Task<ListPage<Shipment>> ListShipmentsAsync(this IMeResource resource, string orderID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<Shipment>();
			return await resource.ListShipmentsAsync(orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListShipmentsAsync<T>(this IMeResource resource, string orderID = null, IListArgs args = null, string accessToken = null) 
            where T : Shipment
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListShipmentsAsync<T>(orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<ShipmentItem>> ListShipmentItemsAsync(this IMeResource resource, string shipmentID, string orderID = null, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<ShipmentItem>();
			return await resource.ListShipmentItemsAsync(shipmentID, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListShipmentItemsAsync<T>(this IMeResource resource, string shipmentID, string orderID = null, IListArgs args = null, string accessToken = null) 
            where T : ShipmentItem
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListShipmentItemsAsync<T>(shipmentID, orderID, args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<SpendingAccount>> ListSpendingAccountsAsync(this IMeResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<SpendingAccount>();
			return await resource.ListSpendingAccountsAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListSpendingAccountsAsync<T>(this IMeResource resource, IListArgs args = null, string accessToken = null) 
            where T : SpendingAccount
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListSpendingAccountsAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }       
        public static async Task<ListPage<UserGroup>> ListUserGroupsAsync(this IMeResource resource, IListArgs args = null, string accessToken = null) 
        {
            args = args ?? new ListArgs<UserGroup>();
			return await resource.ListUserGroupsAsync(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
        
        public static async Task<ListPage<T>> ListUserGroupsAsync<T>(this IMeResource resource, IListArgs args = null, string accessToken = null) 
            where T : UserGroup
        {
            args = args ?? new ListArgs<T>();
			return await resource.ListUserGroupsAsync<T>(args.Search, args.SearchOn, args.ToSortString(), args.Page, args.PageSize, args.ToFilterString(), accessToken);
        }
    }
}