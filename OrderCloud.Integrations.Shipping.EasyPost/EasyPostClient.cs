using Flurl.Http;
using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Integrations.Shipping.EasyPost
{
	public class EasyPostClient
	{
		protected static IFlurlRequest BuildClient(EasyPostConfig config) => config.BaseUrl.WithBasicAuth(config.ApiKey, "");

		public static async Task<EasyPostShipment> PostShipmentAsync(EasyPostShipment shipment, EasyPostConfig config)
		{
			var response = await BuildClient(config)
				.AppendPathSegment("shipments")
				.PostJsonWithErrorHandlingAsync<EasyPostError>(new { shipment }, config)
				.ReceiveJson<EasyPostShipment>();
			return response;					
		}
	}
}
