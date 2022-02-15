// This file is generated automatically, do not edit directly. See codegen/templates/ListByIDExtensions.cs.hbs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public static class ListByIDExtensions
	{
        private const int MAX_PAGE_SIZE = ListAllHelper.MAX_PAGE_SIZE;
        private const int PAGE_ONE = ListAllHelper.PAGE_ONE;

               
        public static async Task<List<SecurityProfile>> ListByIDAsync(this ISecurityProfilesResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        
               
        public static async Task<List<SecurityProfileAssignment>> ListAssignmentsByIDAsync(this ISecurityProfilesResource resource, IEnumerable<string> ids, string buyerID = null, string supplierID = null, string securityProfileID = null, string userID = null, string userGroupID = null, CommerceRole? commerceRole = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAssignmentsAsync(buyerID, supplierID, securityProfileID, userID, userGroupID, commerceRole, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<ImpersonationConfig>> ListByIDAsync(this IImpersonationConfigsResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        
               
        public static async Task<List<OpenIdConnect>> ListByIDAsync(this IOpenIdConnectsResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        
               
        public static async Task<List<User>> ListByIDAsync(this IAdminUsersResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IAdminUsersResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : User
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<UserGroup>> ListByIDAsync(this IAdminUserGroupsResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IAdminUserGroupsResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : UserGroup
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<UserGroupAssignment>> ListUserAssignmentsByIDAsync(this IAdminUserGroupsResource resource, IEnumerable<string> ids, string userGroupID = null, string userID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListUserAssignmentsAsync(userGroupID, userID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<Address>> ListByIDAsync(this IAdminAddressesResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IAdminAddressesResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : Address
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<MessageSender>> ListByIDAsync(this IMessageSendersResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IMessageSendersResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : MessageSender
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<MessageSenderAssignment>> ListAssignmentsByIDAsync(this IMessageSendersResource resource, IEnumerable<string> ids, string buyerID = null, string messageSenderID = null, string userID = null, string userGroupID = null, PartyType? level = null, string supplierID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAssignmentsAsync(buyerID, messageSenderID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, supplierID, accessToken);
			});
        }   
    
        
               
        public static async Task<List<MessageCCListenerAssignment>> ListCCListenerAssignmentsByIDAsync(this IMessageSendersResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListCCListenerAssignmentsAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        
               
        public static async Task<List<ApiClient>> ListByIDAsync(this IApiClientsResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IApiClientsResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : ApiClient
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<ApiClientAssignment>> ListAssignmentsByIDAsync(this IApiClientsResource resource, IEnumerable<string> ids, string apiClientID = null, string buyerID = null, string supplierID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAssignmentsAsync(apiClientID, buyerID, supplierID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<Incrementor>> ListByIDAsync(this IIncrementorsResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        
               
        public static async Task<List<IntegrationEvent>> ListByIDAsync(this IIntegrationEventsResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        
               
        public static async Task<List<Locale>> ListByIDAsync(this ILocalesResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        
               
        public static async Task<List<LocaleAssignment>> ListAssignmentsByIDAsync(this ILocalesResource resource, IEnumerable<string> ids, string buyerID = null, string localeID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAssignmentsAsync(buyerID, localeID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<Webhook>> ListByIDAsync(this IWebhooksResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        
               
        public static async Task<List<XpIndex>> ListByIDAsync(this IXpIndicesResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        
               
        public static async Task<List<Buyer>> ListByIDAsync(this IBuyersResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IBuyersResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : Buyer
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<BuyerSupplier>> ListBuyerSellersByIDAsync(this IBuyersResource resource, IEnumerable<string> ids, string buyerID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListBuyerSellersAsync(buyerID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        
               
        public static async Task<List<User>> ListByIDAsync(this IUsersResource resource, IEnumerable<string> ids, string buyerID, string userGroupID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(buyerID, userGroupID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IUsersResource resource, IEnumerable<string> ids, string buyerID, string userGroupID = null, string accessToken = null) 
            where T : User
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(buyerID, userGroupID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<UserGroup>> ListByIDAsync(this IUserGroupsResource resource, IEnumerable<string> ids, string buyerID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(buyerID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IUserGroupsResource resource, IEnumerable<string> ids, string buyerID, string accessToken = null) 
            where T : UserGroup
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<UserGroupAssignment>> ListUserAssignmentsByIDAsync(this IUserGroupsResource resource, IEnumerable<string> ids, string buyerID, string userGroupID = null, string userID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListUserAssignmentsAsync(buyerID, userGroupID, userID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<Address>> ListByIDAsync(this IAddressesResource resource, IEnumerable<string> ids, string buyerID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(buyerID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IAddressesResource resource, IEnumerable<string> ids, string buyerID, string accessToken = null) 
            where T : Address
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<AddressAssignment>> ListAssignmentsByIDAsync(this IAddressesResource resource, IEnumerable<string> ids, string buyerID, string addressID = null, string userID = null, string userGroupID = null, PartyType? level = null, bool? isShipping = null, bool? isBilling = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAssignmentsAsync(buyerID, addressID, userID, userGroupID, level, isShipping, isBilling, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<CostCenter>> ListByIDAsync(this ICostCentersResource resource, IEnumerable<string> ids, string buyerID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(buyerID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this ICostCentersResource resource, IEnumerable<string> ids, string buyerID, string accessToken = null) 
            where T : CostCenter
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<CostCenterAssignment>> ListAssignmentsByIDAsync(this ICostCentersResource resource, IEnumerable<string> ids, string buyerID, string costCenterID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAssignmentsAsync(buyerID, costCenterID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<CreditCard>> ListByIDAsync(this ICreditCardsResource resource, IEnumerable<string> ids, string buyerID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(buyerID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this ICreditCardsResource resource, IEnumerable<string> ids, string buyerID, string accessToken = null) 
            where T : CreditCard
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<CreditCardAssignment>> ListAssignmentsByIDAsync(this ICreditCardsResource resource, IEnumerable<string> ids, string buyerID, string creditCardID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAssignmentsAsync(buyerID, creditCardID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<SpendingAccount>> ListByIDAsync(this ISpendingAccountsResource resource, IEnumerable<string> ids, string buyerID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(buyerID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this ISpendingAccountsResource resource, IEnumerable<string> ids, string buyerID, string accessToken = null) 
            where T : SpendingAccount
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<SpendingAccountAssignment>> ListAssignmentsByIDAsync(this ISpendingAccountsResource resource, IEnumerable<string> ids, string buyerID, string spendingAccountID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAssignmentsAsync(buyerID, spendingAccountID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<ApprovalRule>> ListByIDAsync(this IApprovalRulesResource resource, IEnumerable<string> ids, string buyerID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(buyerID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IApprovalRulesResource resource, IEnumerable<string> ids, string buyerID, string accessToken = null) 
            where T : ApprovalRule
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Supplier>> ListByIDAsync(this ISuppliersResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this ISuppliersResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : Supplier
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<SupplierBuyer>> ListBuyersByIDAsync(this ISuppliersResource resource, IEnumerable<string> ids, string supplierID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListBuyersAsync(supplierID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        
               
        public static async Task<List<User>> ListByIDAsync(this ISupplierUsersResource resource, IEnumerable<string> ids, string supplierID, string userGroupID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(supplierID, userGroupID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this ISupplierUsersResource resource, IEnumerable<string> ids, string supplierID, string userGroupID = null, string accessToken = null) 
            where T : User
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(supplierID, userGroupID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<UserGroup>> ListByIDAsync(this ISupplierUserGroupsResource resource, IEnumerable<string> ids, string supplierID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(supplierID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this ISupplierUserGroupsResource resource, IEnumerable<string> ids, string supplierID, string accessToken = null) 
            where T : UserGroup
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(supplierID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<UserGroupAssignment>> ListUserAssignmentsByIDAsync(this ISupplierUserGroupsResource resource, IEnumerable<string> ids, string supplierID, string userGroupID = null, string userID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListUserAssignmentsAsync(supplierID, userGroupID, userID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<Address>> ListByIDAsync(this ISupplierAddressesResource resource, IEnumerable<string> ids, string supplierID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(supplierID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this ISupplierAddressesResource resource, IEnumerable<string> ids, string supplierID, string accessToken = null) 
            where T : Address
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(supplierID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Catalog>> ListByIDAsync(this ICatalogsResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this ICatalogsResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : Catalog
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<CatalogAssignment>> ListAssignmentsByIDAsync(this ICatalogsResource resource, IEnumerable<string> ids, string catalogID = null, string buyerID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAssignmentsAsync(catalogID, buyerID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<ProductCatalogAssignment>> ListProductAssignmentsByIDAsync(this ICatalogsResource resource, IEnumerable<string> ids, string catalogID = null, string productID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListProductAssignmentsAsync(catalogID, productID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<Category>> ListByIDAsync(this ICategoriesResource resource, IEnumerable<string> ids, string catalogID, string depth = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(catalogID, depth, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this ICategoriesResource resource, IEnumerable<string> ids, string catalogID, string depth = null, string accessToken = null) 
            where T : Category
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(catalogID, depth, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<CategoryAssignment>> ListAssignmentsByIDAsync(this ICategoriesResource resource, IEnumerable<string> ids, string catalogID, string categoryID = null, string buyerID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAssignmentsAsync(catalogID, categoryID, buyerID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<CategoryProductAssignment>> ListProductAssignmentsByIDAsync(this ICategoriesResource resource, IEnumerable<string> ids, string catalogID, string categoryID = null, string productID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListProductAssignmentsAsync(catalogID, categoryID, productID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<Product>> ListByIDAsync(this IProductsResource resource, IEnumerable<string> ids, string catalogID = null, string categoryID = null, string supplierID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDWithFacetsAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(catalogID, categoryID, supplierID, null, null, SearchType.AnyTerm, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IProductsResource resource, IEnumerable<string> ids, string catalogID = null, string categoryID = null, string supplierID = null, string accessToken = null) 
            where T : Product
        {
            return await ListAllHelper.ListByIDWithFacetsAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(catalogID, categoryID, supplierID, null, null, SearchType.AnyTerm, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Spec>> ListSpecsByIDAsync(this IProductsResource resource, IEnumerable<string> ids, string productID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListSpecsAsync(productID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListSpecsByIDAsync<T>(this IProductsResource resource, IEnumerable<string> ids, string productID, string accessToken = null) 
            where T : Spec
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListSpecsAsync<T>(productID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<ProductSupplier>> ListSuppliersByIDAsync(this IProductsResource resource, IEnumerable<string> ids, string productID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListSuppliersAsync(productID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListSuppliersByIDAsync<T>(this IProductsResource resource, IEnumerable<string> ids, string productID, string accessToken = null) 
            where T : ProductSupplier
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListSuppliersAsync<T>(productID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Variant>> ListVariantsByIDAsync(this IProductsResource resource, IEnumerable<string> ids, string productID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListVariantsAsync(productID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListVariantsByIDAsync<T>(this IProductsResource resource, IEnumerable<string> ids, string productID, string accessToken = null) 
            where T : Variant
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListVariantsAsync<T>(productID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<ProductAssignment>> ListAssignmentsByIDAsync(this IProductsResource resource, IEnumerable<string> ids, string productID = null, string priceScheduleID = null, string buyerID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAssignmentsAsync(productID, priceScheduleID, buyerID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<PriceSchedule>> ListByIDAsync(this IPriceSchedulesResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IPriceSchedulesResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : PriceSchedule
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Spec>> ListByIDAsync(this ISpecsResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this ISpecsResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : Spec
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<SpecOption>> ListOptionsByIDAsync(this ISpecsResource resource, IEnumerable<string> ids, string specID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListOptionsAsync(specID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListOptionsByIDAsync<T>(this ISpecsResource resource, IEnumerable<string> ids, string specID, string accessToken = null) 
            where T : SpecOption
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListOptionsAsync<T>(specID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<SpecProductAssignment>> ListProductAssignmentsByIDAsync(this ISpecsResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListProductAssignmentsAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        
               
        public static async Task<List<ProductFacet>> ListByIDAsync(this IProductFacetsResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IProductFacetsResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : ProductFacet
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<InventoryRecord>> ListByIDAsync(this IInventoryRecordsResource resource, IEnumerable<string> ids, string productID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(productID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IInventoryRecordsResource resource, IEnumerable<string> ids, string productID, string accessToken = null) 
            where T : InventoryRecord
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(productID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<InventoryRecord>> ListVariantByIDAsync(this IInventoryRecordsResource resource, IEnumerable<string> ids, string productID, string variantID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListVariantAsync(productID, variantID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListVariantByIDAsync<T>(this IInventoryRecordsResource resource, IEnumerable<string> ids, string productID, string variantID, string accessToken = null) 
            where T : InventoryRecord
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListVariantAsync<T>(productID, variantID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Order>> ListByIDAsync(this IOrdersResource resource, IEnumerable<string> ids, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(direction, buyerID, supplierID, from, to, null, null, SearchType.AnyTerm, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IOrdersResource resource, IEnumerable<string> ids, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, string accessToken = null) 
            where T : Order
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(direction, buyerID, supplierID, from, to, null, null, SearchType.AnyTerm, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<OrderApproval>> ListApprovalsByIDAsync(this IOrdersResource resource, IEnumerable<string> ids, OrderDirection direction, string orderID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListApprovalsAsync(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListApprovalsByIDAsync<T>(this IOrdersResource resource, IEnumerable<string> ids, OrderDirection direction, string orderID, string accessToken = null) 
            where T : OrderApproval
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListApprovalsAsync<T>(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<User>> ListEligibleApproversByIDAsync(this IOrdersResource resource, IEnumerable<string> ids, OrderDirection direction, string orderID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListEligibleApproversAsync(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListEligibleApproversByIDAsync<T>(this IOrdersResource resource, IEnumerable<string> ids, OrderDirection direction, string orderID, string accessToken = null) 
            where T : User
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListEligibleApproversAsync<T>(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<OrderPromotion>> ListPromotionsByIDAsync(this IOrdersResource resource, IEnumerable<string> ids, OrderDirection direction, string orderID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListPromotionsAsync(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListPromotionsByIDAsync<T>(this IOrdersResource resource, IEnumerable<string> ids, OrderDirection direction, string orderID, string accessToken = null) 
            where T : OrderPromotion
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListPromotionsAsync<T>(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Shipment>> ListShipmentsByIDAsync(this IOrdersResource resource, IEnumerable<string> ids, OrderDirection direction, string orderID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListShipmentsAsync(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListShipmentsByIDAsync<T>(this IOrdersResource resource, IEnumerable<string> ids, OrderDirection direction, string orderID, string accessToken = null) 
            where T : Shipment
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListShipmentsAsync<T>(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<LineItem>> ListByIDAsync(this ILineItemsResource resource, IEnumerable<string> ids, OrderDirection direction, string orderID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this ILineItemsResource resource, IEnumerable<string> ids, OrderDirection direction, string orderID, string accessToken = null) 
            where T : LineItem
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Promotion>> ListByIDAsync(this IPromotionsResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IPromotionsResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : Promotion
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<PromotionAssignment>> ListAssignmentsByIDAsync(this IPromotionsResource resource, IEnumerable<string> ids, string buyerID = null, string promotionID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAssignmentsAsync(buyerID, promotionID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        
               
        public static async Task<List<Payment>> ListByIDAsync(this IPaymentsResource resource, IEnumerable<string> ids, OrderDirection direction, string orderID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IPaymentsResource resource, IEnumerable<string> ids, OrderDirection direction, string orderID, string accessToken = null) 
            where T : Payment
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Shipment>> ListByIDAsync(this IShipmentsResource resource, IEnumerable<string> ids, string orderID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync(orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListByIDAsync<T>(this IShipmentsResource resource, IEnumerable<string> ids, string orderID = null, string accessToken = null) 
            where T : Shipment
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAsync<T>(orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<ShipmentItem>> ListItemsByIDAsync(this IShipmentsResource resource, IEnumerable<string> ids, string shipmentID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListItemsAsync(shipmentID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListItemsByIDAsync<T>(this IShipmentsResource resource, IEnumerable<string> ids, string shipmentID, string accessToken = null) 
            where T : ShipmentItem
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListItemsAsync<T>(shipmentID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<BuyerAddress>> ListAddressesByIDAsync(this IMeResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAddressesAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAddressesByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : BuyerAddress
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListAddressesAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Catalog>> ListCatalogsByIDAsync(this IMeResource resource, IEnumerable<string> ids, string sellerID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListCatalogsAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", sellerID, accessToken);
			});
        }   
    
        public static async Task<List<T>> ListCatalogsByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, string sellerID = null, string accessToken = null) 
            where T : Catalog
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListCatalogsAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", sellerID, accessToken);
			});
        }
               
        public static async Task<List<Category>> ListCategoriesByIDAsync(this IMeResource resource, IEnumerable<string> ids, string depth = null, string catalogID = null, string productID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListCategoriesAsync(depth, catalogID, productID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListCategoriesByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, string depth = null, string catalogID = null, string productID = null, string accessToken = null) 
            where T : Category
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListCategoriesAsync<T>(depth, catalogID, productID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<CostCenter>> ListCostCentersByIDAsync(this IMeResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListCostCentersAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListCostCentersByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : CostCenter
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListCostCentersAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<BuyerCreditCard>> ListCreditCardsByIDAsync(this IMeResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListCreditCardsAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListCreditCardsByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : BuyerCreditCard
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListCreditCardsAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Order>> ListOrdersByIDAsync(this IMeResource resource, IEnumerable<string> ids, DateTimeOffset? from = null, DateTimeOffset? to = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListOrdersAsync(from, to, null, null, SearchType.AnyTerm, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListOrdersByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, DateTimeOffset? from = null, DateTimeOffset? to = null, string accessToken = null) 
            where T : Order
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListOrdersAsync<T>(from, to, null, null, SearchType.AnyTerm, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Order>> ListApprovableOrdersByIDAsync(this IMeResource resource, IEnumerable<string> ids, DateTimeOffset? from = null, DateTimeOffset? to = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListApprovableOrdersAsync(from, to, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListApprovableOrdersByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, DateTimeOffset? from = null, DateTimeOffset? to = null, string accessToken = null) 
            where T : Order
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListApprovableOrdersAsync<T>(from, to, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<BuyerProduct>> ListProductsByIDAsync(this IMeResource resource, IEnumerable<string> ids, string catalogID = null, string categoryID = null, string depth = null, string sellerID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDWithFacetsAsync(ids, (filterValue) =>
			{
				return resource.ListProductsAsync(catalogID, categoryID, depth, null, null, SearchType.AnyTerm, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", sellerID, accessToken);
			});
        }   
    
        public static async Task<List<T>> ListProductsByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, string catalogID = null, string categoryID = null, string depth = null, string sellerID = null, string accessToken = null) 
            where T : BuyerProduct
        {
            return await ListAllHelper.ListByIDWithFacetsAsync(ids, (filterValue) =>
			{
				return resource.ListProductsAsync<T>(catalogID, categoryID, depth, null, null, SearchType.AnyTerm, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", sellerID, accessToken);
			});
        }
               
        public static async Task<List<Spec>> ListSpecsByIDAsync(this IMeResource resource, IEnumerable<string> ids, string productID, string catalogID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListSpecsAsync(productID, catalogID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListSpecsByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, string productID, string catalogID = null, string accessToken = null) 
            where T : Spec
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListSpecsAsync<T>(productID, catalogID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Variant>> ListVariantsByIDAsync(this IMeResource resource, IEnumerable<string> ids, string productID, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListVariantsAsync(productID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListVariantsByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, string productID, string accessToken = null) 
            where T : Variant
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListVariantsAsync<T>(productID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<Promotion>> ListPromotionsByIDAsync(this IMeResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListPromotionsAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListPromotionsByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : Promotion
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListPromotionsAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<BuyerSupplier>> ListBuyerSellersByIDAsync(this IMeResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListBuyerSellersAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        
               
        public static async Task<List<Shipment>> ListShipmentsByIDAsync(this IMeResource resource, IEnumerable<string> ids, string orderID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListShipmentsAsync(orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListShipmentsByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, string orderID = null, string accessToken = null) 
            where T : Shipment
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListShipmentsAsync<T>(orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<ShipmentItem>> ListShipmentItemsByIDAsync(this IMeResource resource, IEnumerable<string> ids, string shipmentID, string orderID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListShipmentItemsAsync(shipmentID, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListShipmentItemsByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, string shipmentID, string orderID = null, string accessToken = null) 
            where T : ShipmentItem
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListShipmentItemsAsync<T>(shipmentID, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<SpendingAccount>> ListSpendingAccountsByIDAsync(this IMeResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListSpendingAccountsAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListSpendingAccountsByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : SpendingAccount
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListSpendingAccountsAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
               
        public static async Task<List<UserGroup>> ListUserGroupsByIDAsync(this IMeResource resource, IEnumerable<string> ids, string accessToken = null) 
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListUserGroupsAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }   
    
        public static async Task<List<T>> ListUserGroupsByIDAsync<T>(this IMeResource resource, IEnumerable<string> ids, string accessToken = null) 
            where T : UserGroup
        {
            return await ListAllHelper.ListByIDAsync(ids, (filterValue) =>
			{
				return resource.ListUserGroupsAsync<T>(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, $"ID={filterValue}", accessToken);
			});
        }
        
    }
}