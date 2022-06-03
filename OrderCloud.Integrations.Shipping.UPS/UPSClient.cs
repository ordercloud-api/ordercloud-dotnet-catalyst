using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Shipping.UPS
{
	public class UPSClient
	{
		protected static IFlurlRequest BuildClient(UPSConfig config) => config.BaseUrl.WithHeaders(new { ContentType = "application/json", AccessLicenseNumber = config.ApiKey });

		public async static Task<UPSRestResponseBody> ShopRates(UPSRestRequestBody rateRequest, UPSConfig config)
		{
			var response = await BuildClient(config)
				.AppendPathSegments("rating", "Shop")
				.PostJsonWithErrorHandlingAsync<UPSErrorBody>(rateRequest, config)
				.ReceiveJson<UPSRestResponseBody>();
			return response;	
		}
	}
}
