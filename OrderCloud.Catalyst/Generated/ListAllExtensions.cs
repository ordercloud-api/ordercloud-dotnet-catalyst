// This file is generated automatically, do not edit directly. See codegen/templates/ListAllExtensions.cs.hbs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public static class ListAllExtensions
	{
        private const int MAX_PAGE_SIZE = ListAllHelper.MAX_PAGE_SIZE;
        private const int PAGE_ONE = ListAllHelper.PAGE_ONE;
        private const string SORT_BY_ID = ListAllHelper.SORT_BY_ID;

               
        public static async Task<List<SecurityProfile>> ListAllAsync(this ISecurityProfilesResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        

        public static async Task ListAllAsync(this ISecurityProfilesResource resource, Func<ListPage<SecurityProfile>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<SecurityProfileAssignment>> ListAllAssignmentsAsync(this ISecurityProfilesResource resource, string buyerID = null, string supplierID = null, string securityProfileID = null, string userID = null, string userGroupID = null, CommerceRole? commerceRole = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, supplierID, securityProfileID, userID, userGroupID, commerceRole, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllAssignmentsAsync(this ISecurityProfilesResource resource, Func<ListPage<SecurityProfileAssignment>, Task> action, string buyerID = null, string supplierID = null, string securityProfileID = null, string userID = null, string userGroupID = null, CommerceRole? commerceRole = null, PartyType? level = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAssignmentsAsync(buyerID, supplierID, securityProfileID, userID, userGroupID, commerceRole, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<ImpersonationConfig>> ListAllAsync(this IImpersonationConfigsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        

        public static async Task ListAllAsync(this IImpersonationConfigsResource resource, Func<ListPage<ImpersonationConfig>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<OpenIdConnect>> ListAllAsync(this IOpenIdConnectsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        

        public static async Task ListAllAsync(this IOpenIdConnectsResource resource, Func<ListPage<OpenIdConnect>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<User>> ListAllAsync(this IAdminUsersResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IAdminUsersResource resource, object filters = null, string accessToken = null) 
            where T : User
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IAdminUsersResource resource, Func<ListPage<User>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IAdminUsersResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : User
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<UserGroup>> ListAllAsync(this IAdminUserGroupsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IAdminUserGroupsResource resource, object filters = null, string accessToken = null) 
            where T : UserGroup
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IAdminUserGroupsResource resource, Func<ListPage<UserGroup>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IAdminUserGroupsResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : UserGroup
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<UserGroupAssignment>> ListAllUserAssignmentsAsync(this IAdminUserGroupsResource resource, string userGroupID = null, string userID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListUserAssignmentsAsync(userGroupID, userID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllUserAssignmentsAsync(this IAdminUserGroupsResource resource, Func<ListPage<UserGroupAssignment>, Task> action, string userGroupID = null, string userID = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListUserAssignmentsAsync(userGroupID, userID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<Address>> ListAllAsync(this IAdminAddressesResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IAdminAddressesResource resource, object filters = null, string accessToken = null) 
            where T : Address
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IAdminAddressesResource resource, Func<ListPage<Address>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IAdminAddressesResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : Address
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<MessageSender>> ListAllAsync(this IMessageSendersResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IMessageSendersResource resource, object filters = null, string accessToken = null) 
            where T : MessageSender
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IMessageSendersResource resource, Func<ListPage<MessageSender>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IMessageSendersResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : MessageSender
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<MessageSenderAssignment>> ListAllAssignmentsAsync(this IMessageSendersResource resource, string buyerID = null, string messageSenderID = null, string userID = null, string userGroupID = null, PartyType? level = null, string supplierID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, messageSenderID, userID, userGroupID, level, page, MAX_PAGE_SIZE, supplierID, accessToken);
			});
        }   
    
        

        public static async Task ListAllAssignmentsAsync(this IMessageSendersResource resource, Func<ListPage<MessageSenderAssignment>, Task> action, string buyerID = null, string messageSenderID = null, string userID = null, string userGroupID = null, PartyType? level = null, string supplierID = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAssignmentsAsync(buyerID, messageSenderID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, supplierID, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<MessageCCListenerAssignment>> ListAllCCListenerAssignmentsAsync(this IMessageSendersResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCCListenerAssignmentsAsync(null, null, null, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        

        public static async Task ListAllCCListenerAssignmentsAsync(this IMessageSendersResource resource, Func<ListPage<MessageCCListenerAssignment>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListCCListenerAssignmentsAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<ApiClient>> ListAllAsync(this IApiClientsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IApiClientsResource resource, object filters = null, string accessToken = null) 
            where T : ApiClient
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IApiClientsResource resource, Func<ListPage<ApiClient>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IApiClientsResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : ApiClient
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<ApiClientAssignment>> ListAllAssignmentsAsync(this IApiClientsResource resource, string apiClientID = null, string buyerID = null, string supplierID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(apiClientID, buyerID, supplierID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllAssignmentsAsync(this IApiClientsResource resource, Func<ListPage<ApiClientAssignment>, Task> action, string apiClientID = null, string buyerID = null, string supplierID = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAssignmentsAsync(apiClientID, buyerID, supplierID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<Incrementor>> ListAllAsync(this IIncrementorsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        

        public static async Task ListAllAsync(this IIncrementorsResource resource, Func<ListPage<Incrementor>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<IntegrationEvent>> ListAllAsync(this IIntegrationEventsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        

        public static async Task ListAllAsync(this IIntegrationEventsResource resource, Func<ListPage<IntegrationEvent>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<Locale>> ListAllAsync(this ILocalesResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        

        public static async Task ListAllAsync(this ILocalesResource resource, Func<ListPage<Locale>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<LocaleAssignment>> ListAllAssignmentsAsync(this ILocalesResource resource, string buyerID = null, string localeID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, localeID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllAssignmentsAsync(this ILocalesResource resource, Func<ListPage<LocaleAssignment>, Task> action, string buyerID = null, string localeID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAssignmentsAsync(buyerID, localeID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<Webhook>> ListAllAsync(this IWebhooksResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        

        public static async Task ListAllAsync(this IWebhooksResource resource, Func<ListPage<Webhook>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<XpIndex>> ListAllAsync(this IXpIndicesResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, null, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        

        public static async Task ListAllAsync(this IXpIndicesResource resource, Func<ListPage<XpIndex>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<Buyer>> ListAllAsync(this IBuyersResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IBuyersResource resource, object filters = null, string accessToken = null) 
            where T : Buyer
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IBuyersResource resource, Func<ListPage<Buyer>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IBuyersResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : Buyer
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<BuyerSupplier>> ListAllBuyerSellersAsync(this IBuyersResource resource, string buyerID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListBuyerSellersAsync(buyerID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        

        public static async Task ListAllBuyerSellersAsync(this IBuyersResource resource, Func<ListPage<BuyerSupplier>, Task> action, string buyerID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListBuyerSellersAsync(buyerID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<User>> ListAllAsync(this IUsersResource resource, string buyerID, string userGroupID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, userGroupID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IUsersResource resource, string buyerID, string userGroupID = null, object filters = null, string accessToken = null) 
            where T : User
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, userGroupID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IUsersResource resource, Func<ListPage<User>, Task> action, string buyerID, string userGroupID = null, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(buyerID, userGroupID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IUsersResource resource, Func<ListPage<T>, Task> action, string buyerID, string userGroupID = null, object filters = null, string accessToken = null) 
            where T : User
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(buyerID, userGroupID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<UserGroup>> ListAllAsync(this IUserGroupsResource resource, string buyerID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IUserGroupsResource resource, string buyerID, object filters = null, string accessToken = null) 
            where T : UserGroup
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IUserGroupsResource resource, Func<ListPage<UserGroup>, Task> action, string buyerID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(buyerID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IUserGroupsResource resource, Func<ListPage<T>, Task> action, string buyerID, object filters = null, string accessToken = null) 
            where T : UserGroup
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(buyerID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<UserGroupAssignment>> ListAllUserAssignmentsAsync(this IUserGroupsResource resource, string buyerID, string userGroupID = null, string userID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListUserAssignmentsAsync(buyerID, userGroupID, userID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllUserAssignmentsAsync(this IUserGroupsResource resource, Func<ListPage<UserGroupAssignment>, Task> action, string buyerID, string userGroupID = null, string userID = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListUserAssignmentsAsync(buyerID, userGroupID, userID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<Address>> ListAllAsync(this IAddressesResource resource, string buyerID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IAddressesResource resource, string buyerID, object filters = null, string accessToken = null) 
            where T : Address
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IAddressesResource resource, Func<ListPage<Address>, Task> action, string buyerID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(buyerID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IAddressesResource resource, Func<ListPage<T>, Task> action, string buyerID, object filters = null, string accessToken = null) 
            where T : Address
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(buyerID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<AddressAssignment>> ListAllAssignmentsAsync(this IAddressesResource resource, string buyerID, string addressID = null, string userID = null, string userGroupID = null, PartyType? level = null, bool? isShipping = null, bool? isBilling = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, addressID, userID, userGroupID, level, isShipping, isBilling, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllAssignmentsAsync(this IAddressesResource resource, Func<ListPage<AddressAssignment>, Task> action, string buyerID, string addressID = null, string userID = null, string userGroupID = null, PartyType? level = null, bool? isShipping = null, bool? isBilling = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAssignmentsAsync(buyerID, addressID, userID, userGroupID, level, isShipping, isBilling, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<CostCenter>> ListAllAsync(this ICostCentersResource resource, string buyerID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this ICostCentersResource resource, string buyerID, object filters = null, string accessToken = null) 
            where T : CostCenter
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this ICostCentersResource resource, Func<ListPage<CostCenter>, Task> action, string buyerID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(buyerID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this ICostCentersResource resource, Func<ListPage<T>, Task> action, string buyerID, object filters = null, string accessToken = null) 
            where T : CostCenter
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(buyerID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<CostCenterAssignment>> ListAllAssignmentsAsync(this ICostCentersResource resource, string buyerID, string costCenterID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, costCenterID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllAssignmentsAsync(this ICostCentersResource resource, Func<ListPage<CostCenterAssignment>, Task> action, string buyerID, string costCenterID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAssignmentsAsync(buyerID, costCenterID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<CreditCard>> ListAllAsync(this ICreditCardsResource resource, string buyerID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this ICreditCardsResource resource, string buyerID, object filters = null, string accessToken = null) 
            where T : CreditCard
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this ICreditCardsResource resource, Func<ListPage<CreditCard>, Task> action, string buyerID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(buyerID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this ICreditCardsResource resource, Func<ListPage<T>, Task> action, string buyerID, object filters = null, string accessToken = null) 
            where T : CreditCard
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(buyerID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<CreditCardAssignment>> ListAllAssignmentsAsync(this ICreditCardsResource resource, string buyerID, string creditCardID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, creditCardID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllAssignmentsAsync(this ICreditCardsResource resource, Func<ListPage<CreditCardAssignment>, Task> action, string buyerID, string creditCardID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAssignmentsAsync(buyerID, creditCardID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<SpendingAccount>> ListAllAsync(this ISpendingAccountsResource resource, string buyerID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this ISpendingAccountsResource resource, string buyerID, object filters = null, string accessToken = null) 
            where T : SpendingAccount
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this ISpendingAccountsResource resource, Func<ListPage<SpendingAccount>, Task> action, string buyerID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(buyerID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this ISpendingAccountsResource resource, Func<ListPage<T>, Task> action, string buyerID, object filters = null, string accessToken = null) 
            where T : SpendingAccount
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(buyerID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<SpendingAccountAssignment>> ListAllAssignmentsAsync(this ISpendingAccountsResource resource, string buyerID, string spendingAccountID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, spendingAccountID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllAssignmentsAsync(this ISpendingAccountsResource resource, Func<ListPage<SpendingAccountAssignment>, Task> action, string buyerID, string spendingAccountID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAssignmentsAsync(buyerID, spendingAccountID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<ApprovalRule>> ListAllAsync(this IApprovalRulesResource resource, string buyerID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(buyerID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IApprovalRulesResource resource, string buyerID, object filters = null, string accessToken = null) 
            where T : ApprovalRule
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(buyerID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IApprovalRulesResource resource, Func<ListPage<ApprovalRule>, Task> action, string buyerID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(buyerID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IApprovalRulesResource resource, Func<ListPage<T>, Task> action, string buyerID, object filters = null, string accessToken = null) 
            where T : ApprovalRule
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(buyerID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Supplier>> ListAllAsync(this ISuppliersResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this ISuppliersResource resource, object filters = null, string accessToken = null) 
            where T : Supplier
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this ISuppliersResource resource, Func<ListPage<Supplier>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this ISuppliersResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : Supplier
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<SupplierBuyer>> ListAllBuyersAsync(this ISuppliersResource resource, string supplierID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListBuyersAsync(supplierID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        

        public static async Task ListAllBuyersAsync(this ISuppliersResource resource, Func<ListPage<SupplierBuyer>, Task> action, string supplierID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListBuyersAsync(supplierID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<User>> ListAllAsync(this ISupplierUsersResource resource, string supplierID, string userGroupID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(supplierID, userGroupID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this ISupplierUsersResource resource, string supplierID, string userGroupID = null, object filters = null, string accessToken = null) 
            where T : User
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(supplierID, userGroupID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this ISupplierUsersResource resource, Func<ListPage<User>, Task> action, string supplierID, string userGroupID = null, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(supplierID, userGroupID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this ISupplierUsersResource resource, Func<ListPage<T>, Task> action, string supplierID, string userGroupID = null, object filters = null, string accessToken = null) 
            where T : User
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(supplierID, userGroupID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<UserGroup>> ListAllAsync(this ISupplierUserGroupsResource resource, string supplierID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(supplierID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this ISupplierUserGroupsResource resource, string supplierID, object filters = null, string accessToken = null) 
            where T : UserGroup
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(supplierID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this ISupplierUserGroupsResource resource, Func<ListPage<UserGroup>, Task> action, string supplierID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(supplierID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this ISupplierUserGroupsResource resource, Func<ListPage<T>, Task> action, string supplierID, object filters = null, string accessToken = null) 
            where T : UserGroup
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(supplierID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<UserGroupAssignment>> ListAllUserAssignmentsAsync(this ISupplierUserGroupsResource resource, string supplierID, string userGroupID = null, string userID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListUserAssignmentsAsync(supplierID, userGroupID, userID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllUserAssignmentsAsync(this ISupplierUserGroupsResource resource, Func<ListPage<UserGroupAssignment>, Task> action, string supplierID, string userGroupID = null, string userID = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListUserAssignmentsAsync(supplierID, userGroupID, userID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<Address>> ListAllAsync(this ISupplierAddressesResource resource, string supplierID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(supplierID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this ISupplierAddressesResource resource, string supplierID, object filters = null, string accessToken = null) 
            where T : Address
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(supplierID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this ISupplierAddressesResource resource, Func<ListPage<Address>, Task> action, string supplierID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(supplierID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this ISupplierAddressesResource resource, Func<ListPage<T>, Task> action, string supplierID, object filters = null, string accessToken = null) 
            where T : Address
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(supplierID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Catalog>> ListAllAsync(this ICatalogsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this ICatalogsResource resource, object filters = null, string accessToken = null) 
            where T : Catalog
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this ICatalogsResource resource, Func<ListPage<Catalog>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this ICatalogsResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : Catalog
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<CatalogAssignment>> ListAllAssignmentsAsync(this ICatalogsResource resource, string catalogID = null, string buyerID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(catalogID, buyerID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllAssignmentsAsync(this ICatalogsResource resource, Func<ListPage<CatalogAssignment>, Task> action, string catalogID = null, string buyerID = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAssignmentsAsync(catalogID, buyerID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<ProductCatalogAssignment>> ListAllProductAssignmentsAsync(this ICatalogsResource resource, string catalogID = null, string productID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListProductAssignmentsAsync(catalogID, productID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllProductAssignmentsAsync(this ICatalogsResource resource, Func<ListPage<ProductCatalogAssignment>, Task> action, string catalogID = null, string productID = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListProductAssignmentsAsync(catalogID, productID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<Category>> ListAllAsync(this ICategoriesResource resource, string catalogID, string depth = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(catalogID, depth, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this ICategoriesResource resource, string catalogID, string depth = null, object filters = null, string accessToken = null) 
            where T : Category
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(catalogID, depth, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this ICategoriesResource resource, Func<ListPage<Category>, Task> action, string catalogID, string depth = null, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(catalogID, depth, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this ICategoriesResource resource, Func<ListPage<T>, Task> action, string catalogID, string depth = null, object filters = null, string accessToken = null) 
            where T : Category
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(catalogID, depth, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<CategoryAssignment>> ListAllAssignmentsAsync(this ICategoriesResource resource, string catalogID, string categoryID = null, string buyerID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(catalogID, categoryID, buyerID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllAssignmentsAsync(this ICategoriesResource resource, Func<ListPage<CategoryAssignment>, Task> action, string catalogID, string categoryID = null, string buyerID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAssignmentsAsync(catalogID, categoryID, buyerID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<CategoryProductAssignment>> ListAllProductAssignmentsAsync(this ICategoriesResource resource, string catalogID, string categoryID = null, string productID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListProductAssignmentsAsync(catalogID, categoryID, productID, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllProductAssignmentsAsync(this ICategoriesResource resource, Func<ListPage<CategoryProductAssignment>, Task> action, string catalogID, string categoryID = null, string productID = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListProductAssignmentsAsync(catalogID, categoryID, productID, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<Product>> ListAllAsync(this IProductsResource resource, string catalogID = null, string categoryID = null, string supplierID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllWithFacetsAsync((page, filter) =>
			{
				return resource.ListAsync(catalogID, categoryID, supplierID, null, null, SearchType.AnyTerm, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IProductsResource resource, string catalogID = null, string categoryID = null, string supplierID = null, object filters = null, string accessToken = null) 
            where T : Product
        {
            return await ListAllHelper.ListAllWithFacetsAsync((page, filter) =>
			{
				return resource.ListAsync<T>(catalogID, categoryID, supplierID, null, null, SearchType.AnyTerm, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IProductsResource resource, Func<ListPageWithFacets<Product>, Task> action, string catalogID = null, string categoryID = null, string supplierID = null, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedWithFacetsAsync(async (filter) =>
			{
				var result = await resource.ListAsync(catalogID, categoryID, supplierID, null, null, SearchType.AnyTerm, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IProductsResource resource, Func<ListPageWithFacets<T>, Task> action, string catalogID = null, string categoryID = null, string supplierID = null, object filters = null, string accessToken = null) 
            where T : Product
        {
            await ListAllHelper.ListBatchedWithFacetsAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(catalogID, categoryID, supplierID, null, null, SearchType.AnyTerm, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Spec>> ListAllSpecsAsync(this IProductsResource resource, string productID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSpecsAsync(productID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllSpecsAsync<T>(this IProductsResource resource, string productID, object filters = null, string accessToken = null) 
            where T : Spec
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSpecsAsync<T>(productID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllSpecsAsync(this IProductsResource resource, Func<ListPage<Spec>, Task> action, string productID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListSpecsAsync(productID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllSpecsAsync<T>(this IProductsResource resource, Func<ListPage<T>, Task> action, string productID, object filters = null, string accessToken = null) 
            where T : Spec
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListSpecsAsync<T>(productID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<ProductSupplier>> ListAllSuppliersAsync(this IProductsResource resource, string productID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSuppliersAsync(productID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllSuppliersAsync<T>(this IProductsResource resource, string productID, object filters = null, string accessToken = null) 
            where T : ProductSupplier
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSuppliersAsync<T>(productID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllSuppliersAsync(this IProductsResource resource, Func<ListPage<ProductSupplier>, Task> action, string productID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListSuppliersAsync(productID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllSuppliersAsync<T>(this IProductsResource resource, Func<ListPage<T>, Task> action, string productID, object filters = null, string accessToken = null) 
            where T : ProductSupplier
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListSuppliersAsync<T>(productID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Variant>> ListAllVariantsAsync(this IProductsResource resource, string productID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListVariantsAsync(productID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllVariantsAsync<T>(this IProductsResource resource, string productID, object filters = null, string accessToken = null) 
            where T : Variant
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListVariantsAsync<T>(productID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllVariantsAsync(this IProductsResource resource, Func<ListPage<Variant>, Task> action, string productID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListVariantsAsync(productID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllVariantsAsync<T>(this IProductsResource resource, Func<ListPage<T>, Task> action, string productID, object filters = null, string accessToken = null) 
            where T : Variant
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListVariantsAsync<T>(productID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<ProductAssignment>> ListAllAssignmentsAsync(this IProductsResource resource, string productID = null, string priceScheduleID = null, string buyerID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(productID, priceScheduleID, buyerID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllAssignmentsAsync(this IProductsResource resource, Func<ListPage<ProductAssignment>, Task> action, string productID = null, string priceScheduleID = null, string buyerID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAssignmentsAsync(productID, priceScheduleID, buyerID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<PriceSchedule>> ListAllAsync(this IPriceSchedulesResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IPriceSchedulesResource resource, object filters = null, string accessToken = null) 
            where T : PriceSchedule
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IPriceSchedulesResource resource, Func<ListPage<PriceSchedule>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IPriceSchedulesResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : PriceSchedule
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Spec>> ListAllAsync(this ISpecsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this ISpecsResource resource, object filters = null, string accessToken = null) 
            where T : Spec
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this ISpecsResource resource, Func<ListPage<Spec>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this ISpecsResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : Spec
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<SpecOption>> ListAllOptionsAsync(this ISpecsResource resource, string specID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListOptionsAsync(specID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllOptionsAsync<T>(this ISpecsResource resource, string specID, object filters = null, string accessToken = null) 
            where T : SpecOption
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListOptionsAsync<T>(specID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllOptionsAsync(this ISpecsResource resource, Func<ListPage<SpecOption>, Task> action, string specID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListOptionsAsync(specID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllOptionsAsync<T>(this ISpecsResource resource, Func<ListPage<T>, Task> action, string specID, object filters = null, string accessToken = null) 
            where T : SpecOption
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListOptionsAsync<T>(specID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<SpecProductAssignment>> ListAllProductAssignmentsAsync(this ISpecsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListProductAssignmentsAsync(null, null, null, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        

        public static async Task ListAllProductAssignmentsAsync(this ISpecsResource resource, Func<ListPage<SpecProductAssignment>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListProductAssignmentsAsync(null, null, null, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<ProductFacet>> ListAllAsync(this IProductFacetsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IProductFacetsResource resource, object filters = null, string accessToken = null) 
            where T : ProductFacet
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IProductFacetsResource resource, Func<ListPage<ProductFacet>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IProductFacetsResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : ProductFacet
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<InventoryRecord>> ListAllAsync(this IInventoryRecordsResource resource, string productID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(productID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IInventoryRecordsResource resource, string productID, object filters = null, string accessToken = null) 
            where T : InventoryRecord
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(productID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IInventoryRecordsResource resource, Func<ListPage<InventoryRecord>, Task> action, string productID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(productID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IInventoryRecordsResource resource, Func<ListPage<T>, Task> action, string productID, object filters = null, string accessToken = null) 
            where T : InventoryRecord
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(productID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<InventoryRecord>> ListAllVariantAsync(this IInventoryRecordsResource resource, string productID, string variantID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListVariantAsync(productID, variantID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllVariantAsync<T>(this IInventoryRecordsResource resource, string productID, string variantID, object filters = null, string accessToken = null) 
            where T : InventoryRecord
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListVariantAsync<T>(productID, variantID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllVariantAsync(this IInventoryRecordsResource resource, Func<ListPage<InventoryRecord>, Task> action, string productID, string variantID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListVariantAsync(productID, variantID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllVariantAsync<T>(this IInventoryRecordsResource resource, Func<ListPage<T>, Task> action, string productID, string variantID, object filters = null, string accessToken = null) 
            where T : InventoryRecord
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListVariantAsync<T>(productID, variantID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Order>> ListAllAsync(this IOrdersResource resource, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(direction, buyerID, supplierID, from, to, null, null, SearchType.AnyTerm, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IOrdersResource resource, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
            where T : Order
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(direction, buyerID, supplierID, from, to, null, null, SearchType.AnyTerm, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IOrdersResource resource, Func<ListPage<Order>, Task> action, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(direction, buyerID, supplierID, from, to, null, null, SearchType.AnyTerm, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IOrdersResource resource, Func<ListPage<T>, Task> action, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
            where T : Order
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(direction, buyerID, supplierID, from, to, null, null, SearchType.AnyTerm, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<OrderApproval>> ListAllApprovalsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListApprovalsAsync(direction, orderID, null, null, null, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllApprovalsAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : OrderApproval
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListApprovalsAsync<T>(direction, orderID, null, null, null, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllApprovalsAsync(this IOrdersResource resource, Func<ListPage<OrderApproval>, Task> action, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListApprovalsAsync(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllApprovalsAsync<T>(this IOrdersResource resource, Func<ListPage<T>, Task> action, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : OrderApproval
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListApprovalsAsync<T>(direction, orderID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<User>> ListAllEligibleApproversAsync(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListEligibleApproversAsync(direction, orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllEligibleApproversAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : User
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListEligibleApproversAsync<T>(direction, orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllEligibleApproversAsync(this IOrdersResource resource, Func<ListPage<User>, Task> action, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListEligibleApproversAsync(direction, orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllEligibleApproversAsync<T>(this IOrdersResource resource, Func<ListPage<T>, Task> action, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : User
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListEligibleApproversAsync<T>(direction, orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<OrderPromotion>> ListAllPromotionsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListPromotionsAsync(direction, orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllPromotionsAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : OrderPromotion
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListPromotionsAsync<T>(direction, orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllPromotionsAsync(this IOrdersResource resource, Func<ListPage<OrderPromotion>, Task> action, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListPromotionsAsync(direction, orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllPromotionsAsync<T>(this IOrdersResource resource, Func<ListPage<T>, Task> action, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : OrderPromotion
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListPromotionsAsync<T>(direction, orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Shipment>> ListAllShipmentsAsync(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListShipmentsAsync(direction, orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllShipmentsAsync<T>(this IOrdersResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : Shipment
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListShipmentsAsync<T>(direction, orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllShipmentsAsync(this IOrdersResource resource, Func<ListPage<Shipment>, Task> action, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListShipmentsAsync(direction, orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllShipmentsAsync<T>(this IOrdersResource resource, Func<ListPage<T>, Task> action, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : Shipment
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListShipmentsAsync<T>(direction, orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<ExtendedLineItem>> ListAllAcrossOrdersAsync(this ILineItemsResource resource, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAcrossOrdersAsync(direction, buyerID, supplierID, from, to, null, null, SearchType.AnyTerm, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAcrossOrdersAsync<T>(this ILineItemsResource resource, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
            where T : ExtendedLineItem
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAcrossOrdersAsync<T>(direction, buyerID, supplierID, from, to, null, null, SearchType.AnyTerm, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAcrossOrdersAsync(this ILineItemsResource resource, Func<ListPage<ExtendedLineItem>, Task> action, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAcrossOrdersAsync(direction, buyerID, supplierID, from, to, null, null, SearchType.AnyTerm, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAcrossOrdersAsync<T>(this ILineItemsResource resource, Func<ListPage<T>, Task> action, OrderDirection direction, string buyerID = null, string supplierID = null, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
            where T : ExtendedLineItem
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAcrossOrdersAsync<T>(direction, buyerID, supplierID, from, to, null, null, SearchType.AnyTerm, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<LineItem>> ListAllAsync(this ILineItemsResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(direction, orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this ILineItemsResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : LineItem
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(direction, orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this ILineItemsResource resource, Func<ListPage<LineItem>, Task> action, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(direction, orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this ILineItemsResource resource, Func<ListPage<T>, Task> action, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : LineItem
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(direction, orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Promotion>> ListAllAsync(this IPromotionsResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IPromotionsResource resource, object filters = null, string accessToken = null) 
            where T : Promotion
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IPromotionsResource resource, Func<ListPage<Promotion>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IPromotionsResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : Promotion
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<PromotionAssignment>> ListAllAssignmentsAsync(this IPromotionsResource resource, string buyerID = null, string promotionID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAssignmentsAsync(buyerID, promotionID, userID, userGroupID, level, page, MAX_PAGE_SIZE, accessToken);
			});
        }   
    
        

        public static async Task ListAllAssignmentsAsync(this IPromotionsResource resource, Func<ListPage<PromotionAssignment>, Task> action, string buyerID = null, string promotionID = null, string userID = null, string userGroupID = null, PartyType? level = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAssignmentsAsync(buyerID, promotionID, userID, userGroupID, level, PAGE_ONE, MAX_PAGE_SIZE, accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<Payment>> ListAllAsync(this IPaymentsResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(direction, orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IPaymentsResource resource, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : Payment
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(direction, orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IPaymentsResource resource, Func<ListPage<Payment>, Task> action, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(direction, orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IPaymentsResource resource, Func<ListPage<T>, Task> action, OrderDirection direction, string orderID, object filters = null, string accessToken = null) 
            where T : Payment
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(direction, orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Shipment>> ListAllAsync(this IShipmentsResource resource, string orderID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IShipmentsResource resource, string orderID = null, object filters = null, string accessToken = null) 
            where T : Shipment
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IShipmentsResource resource, Func<ListPage<Shipment>, Task> action, string orderID = null, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IShipmentsResource resource, Func<ListPage<T>, Task> action, string orderID = null, object filters = null, string accessToken = null) 
            where T : Shipment
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<ShipmentItem>> ListAllItemsAsync(this IShipmentsResource resource, string shipmentID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListItemsAsync(shipmentID, null, null, null, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllItemsAsync<T>(this IShipmentsResource resource, string shipmentID, object filters = null, string accessToken = null) 
            where T : ShipmentItem
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListItemsAsync<T>(shipmentID, null, null, null, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllItemsAsync(this IShipmentsResource resource, Func<ListPage<ShipmentItem>, Task> action, string shipmentID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListItemsAsync(shipmentID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllItemsAsync<T>(this IShipmentsResource resource, Func<ListPage<T>, Task> action, string shipmentID, object filters = null, string accessToken = null) 
            where T : ShipmentItem
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListItemsAsync<T>(shipmentID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<OrderReturn>> ListAllAsync(this IOrderReturnsResource resource, bool approvable = false, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(approvable, null, null, null, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this IOrderReturnsResource resource, bool approvable = false, object filters = null, string accessToken = null) 
            where T : OrderReturn
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(approvable, null, null, null, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this IOrderReturnsResource resource, Func<ListPage<OrderReturn>, Task> action, bool approvable = false, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(approvable, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this IOrderReturnsResource resource, Func<ListPage<T>, Task> action, bool approvable = false, object filters = null, string accessToken = null) 
            where T : OrderReturn
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(approvable, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<OrderReturnApproval>> ListAllApprovalsAsync(this IOrderReturnsResource resource, string returnID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListApprovalsAsync(returnID, null, null, null, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllApprovalsAsync<T>(this IOrderReturnsResource resource, string returnID, object filters = null, string accessToken = null) 
            where T : OrderReturnApproval
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListApprovalsAsync<T>(returnID, null, null, null, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllApprovalsAsync(this IOrderReturnsResource resource, Func<ListPage<OrderReturnApproval>, Task> action, string returnID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListApprovalsAsync(returnID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllApprovalsAsync<T>(this IOrderReturnsResource resource, Func<ListPage<T>, Task> action, string returnID, object filters = null, string accessToken = null) 
            where T : OrderReturnApproval
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListApprovalsAsync<T>(returnID, null, null, null, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<User>> ListAllEligibleApproversAsync(this IOrderReturnsResource resource, string returnID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListEligibleApproversAsync(returnID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllEligibleApproversAsync<T>(this IOrderReturnsResource resource, string returnID, object filters = null, string accessToken = null) 
            where T : User
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListEligibleApproversAsync<T>(returnID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllEligibleApproversAsync(this IOrderReturnsResource resource, Func<ListPage<User>, Task> action, string returnID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListEligibleApproversAsync(returnID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllEligibleApproversAsync<T>(this IOrderReturnsResource resource, Func<ListPage<T>, Task> action, string returnID, object filters = null, string accessToken = null) 
            where T : User
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListEligibleApproversAsync<T>(returnID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<SellerApprovalRule>> ListAllAsync(this ISellerApprovalRulesResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAsync<T>(this ISellerApprovalRulesResource resource, object filters = null, string accessToken = null) 
            where T : SellerApprovalRule
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAsync(this ISellerApprovalRulesResource resource, Func<ListPage<SellerApprovalRule>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAsync<T>(this ISellerApprovalRulesResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : SellerApprovalRule
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<BuyerAddress>> ListAllAddressesAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAddressesAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllAddressesAsync<T>(this IMeResource resource, object filters = null, string accessToken = null) 
            where T : BuyerAddress
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListAddressesAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllAddressesAsync(this IMeResource resource, Func<ListPage<BuyerAddress>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAddressesAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllAddressesAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : BuyerAddress
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListAddressesAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Catalog>> ListAllCatalogsAsync(this IMeResource resource, object filters = null, string sellerID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCatalogsAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), sellerID, accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllCatalogsAsync<T>(this IMeResource resource, object filters = null, string sellerID = null, string accessToken = null) 
            where T : Catalog
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCatalogsAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), sellerID, accessToken);
			});
        }

        public static async Task ListAllCatalogsAsync(this IMeResource resource, Func<ListPage<Catalog>, Task> action, object filters = null, string sellerID = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListCatalogsAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), sellerID, accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllCatalogsAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, object filters = null, string sellerID = null, string accessToken = null) 
            where T : Catalog
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListCatalogsAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), sellerID, accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Category>> ListAllCategoriesAsync(this IMeResource resource, string depth = null, string catalogID = null, string productID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCategoriesAsync(depth, catalogID, productID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllCategoriesAsync<T>(this IMeResource resource, string depth = null, string catalogID = null, string productID = null, object filters = null, string accessToken = null) 
            where T : Category
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCategoriesAsync<T>(depth, catalogID, productID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllCategoriesAsync(this IMeResource resource, Func<ListPage<Category>, Task> action, string depth = null, string catalogID = null, string productID = null, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListCategoriesAsync(depth, catalogID, productID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllCategoriesAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, string depth = null, string catalogID = null, string productID = null, object filters = null, string accessToken = null) 
            where T : Category
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListCategoriesAsync<T>(depth, catalogID, productID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<CostCenter>> ListAllCostCentersAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCostCentersAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllCostCentersAsync<T>(this IMeResource resource, object filters = null, string accessToken = null) 
            where T : CostCenter
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCostCentersAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllCostCentersAsync(this IMeResource resource, Func<ListPage<CostCenter>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListCostCentersAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllCostCentersAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : CostCenter
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListCostCentersAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<BuyerCreditCard>> ListAllCreditCardsAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCreditCardsAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllCreditCardsAsync<T>(this IMeResource resource, object filters = null, string accessToken = null) 
            where T : BuyerCreditCard
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListCreditCardsAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllCreditCardsAsync(this IMeResource resource, Func<ListPage<BuyerCreditCard>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListCreditCardsAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllCreditCardsAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : BuyerCreditCard
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListCreditCardsAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Order>> ListAllOrdersAsync(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListOrdersAsync(from, to, null, null, SearchType.AnyTerm, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllOrdersAsync<T>(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
            where T : Order
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListOrdersAsync<T>(from, to, null, null, SearchType.AnyTerm, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllOrdersAsync(this IMeResource resource, Func<ListPage<Order>, Task> action, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListOrdersAsync(from, to, null, null, SearchType.AnyTerm, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllOrdersAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
            where T : Order
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListOrdersAsync<T>(from, to, null, null, SearchType.AnyTerm, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Order>> ListAllApprovableOrdersAsync(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListApprovableOrdersAsync(from, to, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllApprovableOrdersAsync<T>(this IMeResource resource, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
            where T : Order
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListApprovableOrdersAsync<T>(from, to, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllApprovableOrdersAsync(this IMeResource resource, Func<ListPage<Order>, Task> action, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListApprovableOrdersAsync(from, to, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllApprovableOrdersAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, DateTimeOffset? from = null, DateTimeOffset? to = null, object filters = null, string accessToken = null) 
            where T : Order
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListApprovableOrdersAsync<T>(from, to, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<BuyerProduct>> ListAllProductsAsync(this IMeResource resource, string catalogID = null, string categoryID = null, string depth = null, object filters = null, string sellerID = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllWithFacetsAsync((page, filter) =>
			{
				return resource.ListProductsAsync(catalogID, categoryID, depth, null, null, SearchType.AnyTerm, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), sellerID, accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllProductsAsync<T>(this IMeResource resource, string catalogID = null, string categoryID = null, string depth = null, object filters = null, string sellerID = null, string accessToken = null) 
            where T : BuyerProduct
        {
            return await ListAllHelper.ListAllWithFacetsAsync((page, filter) =>
			{
				return resource.ListProductsAsync<T>(catalogID, categoryID, depth, null, null, SearchType.AnyTerm, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), sellerID, accessToken);
			});
        }

        public static async Task ListAllProductsAsync(this IMeResource resource, Func<ListPageWithFacets<BuyerProduct>, Task> action, string catalogID = null, string categoryID = null, string depth = null, object filters = null, string sellerID = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedWithFacetsAsync(async (filter) =>
			{
				var result = await resource.ListProductsAsync(catalogID, categoryID, depth, null, null, SearchType.AnyTerm, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), sellerID, accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllProductsAsync<T>(this IMeResource resource, Func<ListPageWithFacets<T>, Task> action, string catalogID = null, string categoryID = null, string depth = null, object filters = null, string sellerID = null, string accessToken = null) 
            where T : BuyerProduct
        {
            await ListAllHelper.ListBatchedWithFacetsAsync(async (filter) =>
			{
				var result = await resource.ListProductsAsync<T>(catalogID, categoryID, depth, null, null, SearchType.AnyTerm, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), sellerID, accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Spec>> ListAllSpecsAsync(this IMeResource resource, string productID, string catalogID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSpecsAsync(productID, catalogID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllSpecsAsync<T>(this IMeResource resource, string productID, string catalogID = null, object filters = null, string accessToken = null) 
            where T : Spec
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSpecsAsync<T>(productID, catalogID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllSpecsAsync(this IMeResource resource, Func<ListPage<Spec>, Task> action, string productID, string catalogID = null, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListSpecsAsync(productID, catalogID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllSpecsAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, string productID, string catalogID = null, object filters = null, string accessToken = null) 
            where T : Spec
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListSpecsAsync<T>(productID, catalogID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Variant>> ListAllVariantsAsync(this IMeResource resource, string productID, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListVariantsAsync(productID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllVariantsAsync<T>(this IMeResource resource, string productID, object filters = null, string accessToken = null) 
            where T : Variant
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListVariantsAsync<T>(productID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllVariantsAsync(this IMeResource resource, Func<ListPage<Variant>, Task> action, string productID, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListVariantsAsync(productID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllVariantsAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, string productID, object filters = null, string accessToken = null) 
            where T : Variant
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListVariantsAsync<T>(productID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<Promotion>> ListAllPromotionsAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListPromotionsAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllPromotionsAsync<T>(this IMeResource resource, object filters = null, string accessToken = null) 
            where T : Promotion
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListPromotionsAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllPromotionsAsync(this IMeResource resource, Func<ListPage<Promotion>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListPromotionsAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllPromotionsAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : Promotion
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListPromotionsAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<BuyerSupplier>> ListAllBuyerSellersAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListBuyerSellersAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        

        public static async Task ListAllBuyerSellersAsync(this IMeResource resource, Func<ListPage<BuyerSupplier>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListBuyerSellersAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        
               
        public static async Task<List<Shipment>> ListAllShipmentsAsync(this IMeResource resource, string orderID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListShipmentsAsync(orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllShipmentsAsync<T>(this IMeResource resource, string orderID = null, object filters = null, string accessToken = null) 
            where T : Shipment
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListShipmentsAsync<T>(orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllShipmentsAsync(this IMeResource resource, Func<ListPage<Shipment>, Task> action, string orderID = null, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListShipmentsAsync(orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllShipmentsAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, string orderID = null, object filters = null, string accessToken = null) 
            where T : Shipment
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListShipmentsAsync<T>(orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<ShipmentItem>> ListAllShipmentItemsAsync(this IMeResource resource, string shipmentID, string orderID = null, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListShipmentItemsAsync(shipmentID, orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllShipmentItemsAsync<T>(this IMeResource resource, string shipmentID, string orderID = null, object filters = null, string accessToken = null) 
            where T : ShipmentItem
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListShipmentItemsAsync<T>(shipmentID, orderID, null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllShipmentItemsAsync(this IMeResource resource, Func<ListPage<ShipmentItem>, Task> action, string shipmentID, string orderID = null, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListShipmentItemsAsync(shipmentID, orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllShipmentItemsAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, string shipmentID, string orderID = null, object filters = null, string accessToken = null) 
            where T : ShipmentItem
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListShipmentItemsAsync<T>(shipmentID, orderID, null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<SpendingAccount>> ListAllSpendingAccountsAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSpendingAccountsAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllSpendingAccountsAsync<T>(this IMeResource resource, object filters = null, string accessToken = null) 
            where T : SpendingAccount
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListSpendingAccountsAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllSpendingAccountsAsync(this IMeResource resource, Func<ListPage<SpendingAccount>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListSpendingAccountsAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllSpendingAccountsAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : SpendingAccount
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListSpendingAccountsAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
               
        public static async Task<List<UserGroup>> ListAllUserGroupsAsync(this IMeResource resource, object filters = null, string accessToken = null) 
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListUserGroupsAsync(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }   
    
        public static async Task<List<T>> ListAllUserGroupsAsync<T>(this IMeResource resource, object filters = null, string accessToken = null) 
            where T : UserGroup
        {
            return await ListAllHelper.ListAllAsync((page, filter) =>
			{
				return resource.ListUserGroupsAsync<T>(null, null, SORT_BY_ID, page, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
			});
        }

        public static async Task ListAllUserGroupsAsync(this IMeResource resource, Func<ListPage<UserGroup>, Task> action, object filters = null, string accessToken = null) 
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListUserGroupsAsync(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }

        public static async Task ListAllUserGroupsAsync<T>(this IMeResource resource, Func<ListPage<T>, Task> action, object filters = null, string accessToken = null) 
            where T : UserGroup
        {
            await ListAllHelper.ListBatchedAsync(async (filter) =>
			{
				var result = await resource.ListUserGroupsAsync<T>(null, null, SORT_BY_ID, PAGE_ONE, MAX_PAGE_SIZE, filters.AndFilter(filter), accessToken);
                await action(result);
                return result;
			});
        }
        
    }
}