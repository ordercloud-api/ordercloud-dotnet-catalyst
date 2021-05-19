using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst
{
	public static class HttpRequestExtensions
	{
		public static List<string> GetRequiredOrderCloudRoles(this HttpContext context)
		{
			var endpoint = context.GetEndpoint();
			var authorizeAttributes = endpoint?.Metadata.GetOrderedMetadata<OrderCloudUserAuthAttribute>() ?? Array.Empty<OrderCloudUserAuthAttribute>();
			return authorizeAttributes.SelectMany(a => a.OrderCloudRoles).ToList();
		}
	}
}
