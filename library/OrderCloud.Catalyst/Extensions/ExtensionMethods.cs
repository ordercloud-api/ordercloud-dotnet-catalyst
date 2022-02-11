using Flurl;
using Flurl.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst
{
	public static class ExtensionMethods
	{
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
		{
			return items == null || !items.Any();
		}

		public static Type WithoutGenericArgs(this Type type)
		{
			return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
		}

		/// <summary>
		/// Chain to IServiceCollection (typically in Startup.ConfigureServices) to enable authenticating by passing a valid
		/// OrderCloud access token in the Authorization header. Add [OrderCloudUserAuth] attribute to specific controllers or actions
		/// where this should be enforced. Typical use case is custom endpoints for front-end user apps.
		/// </summary>
		public static IServiceCollection AddOrderCloudUserAuth(this IServiceCollection services)
		{
			services
				.AddHttpContextAccessor()
				.AddSingleton<RequestAuthenticationService>()
				.AddSingleton<ISimpleCache, LazyCacheService>() // Can override by registering own implmentation
				.AddAuthentication()
				.AddScheme<OrderCloudUserAuthOptions, OrderCloudUserAuthHandler>("OrderCloudUser", null);
			return services;
		}

		/// <summary>
		/// Chain to IServiceCollection (typically in Startup.ConfigureServices) to enable validation of incoming webhooks.
		/// </summary>
		public static IServiceCollection AddOrderCloudWebhookAuth(this IServiceCollection services, Action<OrderCloudWebhookAuthOptions> configureOptions)
		{
			services.AddAuthentication()
				.AddScheme<OrderCloudWebhookAuthOptions, OrderCloudWebhookAuthHandler>("OrderCloudWebhook", null, configureOptions);
			return services;
		}

		/// <summary>
		/// Looks for an OrderCloudUserAuthAttribute on the current route to find required roles.
		/// </summary>
		public static List<string> GetRequiredOrderCloudRoles(this HttpContext context)
		{
			var endpointFeature = context.Features[typeof(IEndpointFeature)] as IEndpointFeature;
			return endpointFeature?.Endpoint?.Metadata
				.Where(x => x.GetType() == typeof(OrderCloudUserAuthAttribute))
				.SelectMany(x => (x as OrderCloudUserAuthAttribute).OrderCloudRoles)
				.ToList();
		}

		/// <summary>
		/// Looks for a UserTypeRestrictedToAttribute on the current route to find allowed user types.
		/// </summary>
		public static List<CommerceRole> GetAllowedUserTypes(this HttpContext context)
		{
			var endpointFeature = context.Features[typeof(IEndpointFeature)] as IEndpointFeature;

			var attributes = endpointFeature?.Endpoint?.Metadata
				.Where(x => x.GetType() == typeof(UserTypeRestrictedToAttribute));

			if (attributes.Count() == 0)
			{
				// If attribute is not included, all types are allowed.
				return new List<CommerceRole> { CommerceRole.Buyer, CommerceRole.Seller, CommerceRole.Supplier };
			}

			return attributes
				.SelectMany(x => (x as UserTypeRestrictedToAttribute).AllowedUserTypes)
				.ToList();
		}

		// See https://github.com/tmenier/Flurl/blob/ce480aa1aa8ce1f2ff4ebce9f1d6eaf30b7d6d8c/src/Flurl.Http/Configuration/DefaultUrlEncodedSerializer.cs
		/// <summary>
		/// Add a new filter to an existing set of filters. All must evaluate to true.
		/// </summary>
		/// <param name="filters">An existing set of filters.</param>
		/// <param name="listFunc">A new filter that must also evaluate to true.</param>
		/// <returns></returns>
		public static string AndFilter(this object filters, (string Key, object Value) filter)
		{
			var filterList = filters?.ToKeyValuePairs()?.ToList() ?? new List<(string Key, object Value)>();
			filterList.Add(filter);
			var qp = new QueryParamCollection();
			foreach (var kv in filterList)
			{
				if (kv.Value != null)
				{
					qp.Add(kv.Key, kv.Value);
				}
			}
			if (qp.Count == 0) { return null; }
			return string.Join("&", qp.Select(x => $"{x.Name}={x.Value}"));
		}

		/// <summary>
		/// Helper method for dealing with the ShipEstimate model. 
		/// </summary>
		/// <returns>The selected ShipMethod</returns>
		public static ShipMethod GetSelectedShipMethod(this ShipEstimate shipEstimate)
		{
			var selectedMethod = shipEstimate.ShipMethods.FirstOrDefault(m => m.ID == shipEstimate.SelectedShipMethodID);
			Require.That(selectedMethod != null, new InvalidOperationException($"SelectedShipMethodID is ${shipEstimate.SelectedShipMethodID} but no matching object was found in ShipMethods List."));
			return selectedMethod;
		}

		/// <summary>
		/// Helper method for dealing with the ShipEstimate model. Maps ShipEstimateItems from lineItem IDs to full LineItems.
		/// </summary>
		/// <returns>Full model of all LineItems on the specified ShipEstimate</returns>
		public static List<(int Quantity, LineItem LineItem)> GetShipEstimateLineItems(this OrderWorksheet order, string shipEstimateID)
		{
			var shipEstimate = order.ShipEstimateResponse.ShipEstimates.FirstOrDefault(se => se.ID == shipEstimateID);
			Require.That(shipEstimate != null, new ArgumentException($"No matching ship estimate found with ID {shipEstimateID}", "shipEstimateID"));

			var lineItems = new List<(int quantity, LineItem lineItem)> { };
			foreach (var shipEstimateItem in shipEstimate.ShipEstimateItems)
			{
				var lineItem = order.LineItems.FirstOrDefault(li => li.ID == shipEstimateItem.LineItemID);
				Require.That(lineItem != null, new InvalidOperationException($"ShipEstimateItem.LineItemID is ${shipEstimateItem.LineItemID} but no matching object was was found in OrderWorksheet.LineItems."));
				lineItems.Add((shipEstimateItem.Quantity, lineItem));
			}

			return lineItems;
		}
	}
}
