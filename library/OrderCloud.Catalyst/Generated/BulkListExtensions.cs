// This file is generated automatically, do not edit directly. See codegen/templates/BulkListExtensions.cs.hbs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public static class ListAllExtensions
	{
		private const int MAX_PAGE_SIZE = 100;
               
        public static async Task<List<SecurityProfile>> BulkListAsync(this ISecurityProfilesResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
               
        public static async Task<List<SecurityProfileAssignment>> BulkListAssignmentsAsync(this ISecurityProfilesResource resource, string buyerID = null, string supplierID = null, string securityProfileID = null, string userID = null, string userGroupID = null, CommerceRole? commerceRole = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, supplierID, securityProfileID, userID, userGroupID, commerceRole, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<ImpersonationConfig>> BulkListAsync(this IImpersonationConfigsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
               
        public static async Task<List<OpenIdConnect>> BulkListAsync(this IOpenIdConnectsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
               
        public static async Task<List<User>> BulkListAsync(this IAdminUsersResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IAdminUsersResource resource, object filters = null, string accessToken = null) 
            where T : User
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<UserGroup>> BulkListAsync(this IAdminUserGroupsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IAdminUserGroupsResource resource, object filters = null, string accessToken = null) 
            where T : UserGroup
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<UserGroupAssignment>> BulkListUserAssignmentsAsync(this IAdminUserGroupsResource resource, string userGroupID = null, string userID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListUserAssignmentsAsync(userGroupID, userID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<Address>> BulkListAsync(this IAdminAddressesResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IAdminAddressesResource resource, object filters = null, string accessToken = null) 
            where T : Address
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<MessageSender>> BulkListAsync(this IMessageSendersResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IMessageSendersResource resource, object filters = null, string accessToken = null) 
            where T : MessageSender
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<MessageSenderAssignment>> BulkListAssignmentsAsync(this IMessageSendersResource resource, string buyerID = null, string messageSenderID = null, string userID = null, string userGroupID = null, PartyType? level = null, string supplierID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, messageSenderID, userID, userGroupID, level, page, MAX_PAGE_SIZE, supplierID, accessToken);
			});
        }   
    
               
        public static async Task<List<MessageCCListenerAssignment>> BulkListCCListenerAssignmentsAsync(this IMessageSendersResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCCListenerAssignmentsAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
               
        public static async Task<List<ApiClient>> BulkListAsync(this IApiClientsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IApiClientsResource resource, object filters = null, string accessToken = null) 
            where T : ApiClient
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<ApiClientAssignment>> BulkListAssignmentsAsync(this IApiClientsResource resource, string apiClientID = null, string buyerID = null, string supplierID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(apiClientID, buyerID, supplierID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<Incrementor>> BulkListAsync(this IIncrementorsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
               
        public static async Task<List<IntegrationEvent>> BulkListAsync(this IIntegrationEventsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
               
        public static async Task<List<Webhook>> BulkListAsync(this IWebhooksResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
               
        public static async Task<List<XpIndex>> BulkListAsync(this IXpIndicesResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
               
        public static async Task<List<Buyer>> BulkListAsync(this IBuyersResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IBuyersResource resource, object filters = null, string accessToken = null) 
            where T : Buyer
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<User>> BulkListAsync(this IUsersResource resource, string buyerID, string userGroupID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, userGroupID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IUsersResource resource, string buyerID, string userGroupID = null, object filters = null, string accessToken = null) 
            where T : User
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, userGroupID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<UserGroup>> BulkListAsync(this IUserGroupsResource resource, string buyerID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IUserGroupsResource resource, string buyerID, object filters = null, string accessToken = null) 
            where T : UserGroup
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<UserGroupAssignment>> BulkListUserAssignmentsAsync(this IUserGroupsResource resource, string buyerID, string userGroupID = null, string userID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListUserAssignmentsAsync(buyerID, userGroupID, userID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<Address>> BulkListAsync(this IAddressesResource resource, string buyerID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IAddressesResource resource, string buyerID, object filters = null, string accessToken = null) 
            where T : Address
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<AddressAssignment>> BulkListAssignmentsAsync(this IAddressesResource resource, string buyerID, string addressID = null, string userID = null, string userGroupID = null, PartyType? level = null, bool? isShipping = null, bool? isBilling = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, addressID, userID, userGroupID, level, isShipping, isBilling, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<CostCenter>> BulkListAsync(this ICostCentersResource resource, string buyerID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this ICostCentersResource resource, string buyerID, object filters = null, string accessToken = null) 
            where T : CostCenter
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<CostCenterAssignment>> BulkListAssignmentsAsync(this ICostCentersResource resource, string buyerID, string costCenterID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, costCenterID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<CreditCard>> BulkListAsync(this ICreditCardsResource resource, string buyerID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this ICreditCardsResource resource, string buyerID, object filters = null, string accessToken = null) 
            where T : CreditCard
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<CreditCardAssignment>> BulkListAssignmentsAsync(this ICreditCardsResource resource, string buyerID, string creditCardID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, creditCardID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<SpendingAccount>> BulkListAsync(this ISpendingAccountsResource resource, string buyerID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this ISpendingAccountsResource resource, string buyerID, object filters = null, string accessToken = null) 
            where T : SpendingAccount
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<SpendingAccountAssignment>> BulkListAssignmentsAsync(this ISpendingAccountsResource resource, string buyerID, string spendingAccountID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, spendingAccountID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<ApprovalRule>> BulkListAsync(this IApprovalRulesResource resource, string buyerID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IApprovalRulesResource resource, string buyerID, object filters = null, string accessToken = null) 
            where T : ApprovalRule
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Supplier>> BulkListAsync(this ISuppliersResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this ISuppliersResource resource, object filters = null, string accessToken = null) 
            where T : Supplier
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<User>> BulkListAsync(this ISupplierUsersResource resource, string supplierID, string userGroupID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(supplierID, userGroupID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this ISupplierUsersResource resource, string supplierID, string userGroupID = null, object filters = null, string accessToken = null) 
            where T : User
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(supplierID, userGroupID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<UserGroup>> BulkListAsync(this ISupplierUserGroupsResource resource, string supplierID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(supplierID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this ISupplierUserGroupsResource resource, string supplierID, object filters = null, string accessToken = null) 
            where T : UserGroup
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(supplierID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<UserGroupAssignment>> BulkListUserAssignmentsAsync(this ISupplierUserGroupsResource resource, string supplierID, string userGroupID = null, string userID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListUserAssignmentsAsync(supplierID, userGroupID, userID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<Address>> BulkListAsync(this ISupplierAddressesResource resource, string supplierID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(supplierID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this ISupplierAddressesResource resource, string supplierID, object filters = null, string accessToken = null) 
            where T : Address
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(supplierID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Catalog>> BulkListAsync(this ICatalogsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this ICatalogsResource resource, object filters = null, string accessToken = null) 
            where T : Catalog
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<CatalogAssignment>> BulkListAssignmentsAsync(this ICatalogsResource resource, string catalogID = null, string buyerID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(catalogID, buyerID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<ProductCatalogAssignment>> BulkListProductAssignmentsAsync(this ICatalogsResource resource, string catalogID = null, string productID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListProductAssignmentsAsync(catalogID, productID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<Category>> BulkListAsync(this ICategoriesResource resource, string catalogID, string depth = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(catalogID, depth, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this ICategoriesResource resource, string catalogID, string depth = null, object filters = null, string accessToken = null) 
            where T : Category
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(catalogID, depth, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<CategoryAssignment>> BulkListAssignmentsAsync(this ICategoriesResource resource, string catalogID, string categoryID = null, string buyerID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(catalogID, categoryID, buyerID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<CategoryProductAssignment>> BulkListProductAssignmentsAsync(this ICategoriesResource resource, string catalogID, string categoryID = null, string productID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListProductAssignmentsAsync(catalogID, categoryID, productID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<Product>> BulkListAsync(this IProductsResource resource, string catalogID = null, string categoryID = null, string supplierID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllWithFacetsAsync((page, filter) =>
			{
				return resource.ListAsync(catalogID, categoryID, supplierID, null, null, SearchType.AnyTerm, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IProductsResource resource, string catalogID = null, string categoryID = null, string supplierID = null, object filters = null, string accessToken = null) 
            where T : Product
        {
            return await ListAllHelper.ListAllWithFacetsAsync((page, filter) =>
			{
				return resource.ListAsync<T>(catalogID, categoryID, supplierID, null, null, SearchType.AnyTerm, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Spec>> BulkListSpecsAsync(this IProductsResource resource, string productID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSpecsAsync(productID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListSpecsAsync<T>(this IProductsResource resource, string productID, object filters = null, string accessToken = null) 
            where T : Spec
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSpecsAsync<T>(productID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Supplier>> BulkListSuppliersAsync(this IProductsResource resource, string productID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSuppliersAsync(productID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListSuppliersAsync<T>(this IProductsResource resource, string productID, object filters = null, string accessToken = null) 
            where T : Supplier
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSuppliersAsync<T>(productID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Variant>> BulkListVariantsAsync(this IProductsResource resource, string productID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListVariantsAsync(productID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListVariantsAsync<T>(this IProductsResource resource, string productID, object filters = null, string accessToken = null) 
            where T : Variant
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListVariantsAsync<T>(productID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<ProductAssignment>> BulkListAssignmentsAsync(this IProductsResource resource, string productID = null, string priceScheduleID = null, string buyerID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(productID, priceScheduleID, buyerID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<PriceSchedule>> BulkListAsync(this IPriceSchedulesResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IPriceSchedulesResource resource, object filters = null, string accessToken = null) 
            where T : PriceSchedule
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Spec>> BulkListAsync(this ISpecsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this ISpecsResource resource, object filters = null, string accessToken = null) 
            where T : Spec
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<SpecOption>> BulkListOptionsAsync(this ISpecsResource resource, string specID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListOptionsAsync(specID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListOptionsAsync<T>(this ISpecsResource resource, string specID, object filters = null, string accessToken = null) 
            where T : SpecOption
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListOptionsAsync<T>(specID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<SpecProductAssignment>> BulkListProductAssignmentsAsync(this ISpecsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListProductAssignmentsAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
               
        public static async Task<List<ProductFacet>> BulkListAsync(this IProductFacetsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IProductFacetsResource resource, object filters = null, string accessToken = null) 
            where T : ProductFacet
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Order>> BulkListAsync(this IOrdersResource resource, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(direction, buyerID, supplierID, from, to, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IOrdersResource resource, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
            where T : Order
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(direction, buyerID, supplierID, from, to, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<OrderApproval>> BulkListApprovalsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListApprovalsAsync(direction, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListApprovalsAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : OrderApproval
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListApprovalsAsync<T>(direction, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<User>> BulkListEligibleApproversAsync(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListEligibleApproversAsync(direction, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListEligibleApproversAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : User
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListEligibleApproversAsync<T>(direction, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<OrderPromotion>> BulkListPromotionsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListPromotionsAsync(direction, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListPromotionsAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : OrderPromotion
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListPromotionsAsync<T>(direction, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Shipment>> BulkListShipmentsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListShipmentsAsync(direction, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListShipmentsAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : Shipment
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListShipmentsAsync<T>(direction, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<LineItem>> BulkListAsync(this ILineItemsResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(direction, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this ILineItemsResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : LineItem
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(direction, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Promotion>> BulkListAsync(this IPromotionsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IPromotionsResource resource, object filters = null, string accessToken = null) 
            where T : Promotion
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<PromotionAssignment>> BulkListAssignmentsAsync(this IPromotionsResource resource, string buyerID = null, string promotionID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, promotionID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
               
        public static async Task<List<Payment>> BulkListAsync(this IPaymentsResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(direction, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IPaymentsResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : Payment
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(direction, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Shipment>> BulkListAsync(this IShipmentsResource resource, string orderID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAsync<T>(this IShipmentsResource resource, string orderID = null, object filters = null, string accessToken = null) 
            where T : Shipment
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<ShipmentItem>> BulkListItemsAsync(this IShipmentsResource resource, string shipmentID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListItemsAsync(shipmentID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListItemsAsync<T>(this IShipmentsResource resource, string shipmentID, object filters = null, string accessToken = null) 
            where T : ShipmentItem
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListItemsAsync<T>(shipmentID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<BuyerAddress>> BulkListAddressesAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAddressesAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListAddressesAsync<T>(this IMeResource resource, object filters = null, string accessToken = null) 
            where T : BuyerAddress
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAddressesAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Catalog>> BulkListCatalogsAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCatalogsAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListCatalogsAsync<T>(this IMeResource resource, object filters = null, string accessToken = null) 
            where T : Catalog
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCatalogsAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Category>> BulkListCategoriesAsync(this IMeResource resource, string depth = null, string catalogID = null, string productID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCategoriesAsync(depth, catalogID, productID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListCategoriesAsync<T>(this IMeResource resource, string depth = null, string catalogID = null, string productID = null, object filters = null, string accessToken = null) 
            where T : Category
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCategoriesAsync<T>(depth, catalogID, productID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<CostCenter>> BulkListCostCentersAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCostCentersAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListCostCentersAsync<T>(this IMeResource resource, object filters = null, string accessToken = null) 
            where T : CostCenter
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCostCentersAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<BuyerCreditCard>> BulkListCreditCardsAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCreditCardsAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListCreditCardsAsync<T>(this IMeResource resource, object filters = null, string accessToken = null) 
            where T : BuyerCreditCard
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCreditCardsAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Order>> BulkListOrdersAsync(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListOrdersAsync(from, to, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListOrdersAsync<T>(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
            where T : Order
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListOrdersAsync<T>(from, to, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Order>> BulkListApprovableOrdersAsync(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListApprovableOrdersAsync(from, to, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListApprovableOrdersAsync<T>(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
            where T : Order
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListApprovableOrdersAsync<T>(from, to, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<BuyerProduct>> BulkListProductsAsync(this IMeResource resource, string catalogID = null, string categoryID = null, string depth = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllWithFacetsAsync((page, filter) =>
			{
				return resource.ListProductsAsync(catalogID, categoryID, depth, null, null, SearchType.AnyTerm, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListProductsAsync<T>(this IMeResource resource, string catalogID = null, string categoryID = null, string depth = null, object filters = null, string accessToken = null) 
            where T : BuyerProduct
        {
            return await ListAllHelper.ListAllWithFacetsAsync((page, filter) =>
			{
				return resource.ListProductsAsync<T>(catalogID, categoryID, depth, null, null, SearchType.AnyTerm, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Spec>> BulkListSpecsAsync(this IMeResource resource, string productID, string catalogID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSpecsAsync(productID, catalogID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListSpecsAsync<T>(this IMeResource resource, string productID, string catalogID = null, object filters = null, string accessToken = null) 
            where T : Spec
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSpecsAsync<T>(productID, catalogID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Variant>> BulkListVariantsAsync(this IMeResource resource, string productID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListVariantsAsync(productID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListVariantsAsync<T>(this IMeResource resource, string productID, object filters = null, string accessToken = null) 
            where T : Variant
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListVariantsAsync<T>(productID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Promotion>> BulkListPromotionsAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListPromotionsAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListPromotionsAsync<T>(this IMeResource resource, object filters = null, string accessToken = null) 
            where T : Promotion
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListPromotionsAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<Shipment>> BulkListShipmentsAsync(this IMeResource resource, string orderID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListShipmentsAsync(orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListShipmentsAsync<T>(this IMeResource resource, string orderID = null, object filters = null, string accessToken = null) 
            where T : Shipment
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListShipmentsAsync<T>(orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<ShipmentItem>> BulkListShipmentItemsAsync(this IMeResource resource, string shipmentID, string orderID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListShipmentItemsAsync(shipmentID, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListShipmentItemsAsync<T>(this IMeResource resource, string shipmentID, string orderID = null, object filters = null, string accessToken = null) 
            where T : ShipmentItem
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListShipmentItemsAsync<T>(shipmentID, orderID, null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<SpendingAccount>> BulkListSpendingAccountsAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSpendingAccountsAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListSpendingAccountsAsync<T>(this IMeResource resource, object filters = null, string accessToken = null) 
            where T : SpendingAccount
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSpendingAccountsAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
               
        public static async Task<List<UserGroup>> BulkListUserGroupsAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListUserGroupsAsync(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> BulkListUserGroupsAsync<T>(this IMeResource resource, object filters = null, string accessToken = null) 
            where T : UserGroup
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListUserGroupsAsync<T>(null, null, ListAllHelper.GetSort<UserGroup>(), page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }                        
        
    }
}