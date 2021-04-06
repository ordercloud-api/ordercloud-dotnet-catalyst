using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst
{
	public static class HttpRequestExtensions
	{
		public static string GetOrderCloudToken(this HttpRequest request)
		{
			if (!request.Headers.TryGetValue("Authorization", out var header))
				return null;

			var parts = header.FirstOrDefault()?.Split(new[] { ' ' }, 2);
			if (parts?.Length != 2)
				return null;

			if (parts[0] != "Bearer")
				return null;

			return parts[1].Trim();
		}

		public static List<string> GetRequiredOrderCloudRoles(this HttpContext context)
		{
			var endpoint = context.GetEndpoint();
			var authorizeAttributes = endpoint?.Metadata.GetOrderedMetadata<OrderCloudUserAuthAttribute>() ?? Array.Empty<OrderCloudUserAuthAttribute>();
			return authorizeAttributes.SelectMany(a => a.OrderCloudRoles).ToList();
		}
	}
}
