using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Shipping.EasyPost
{
	public class EasyPostClient
	{
		private static async Task<EasyPostShipment> PostShipmentAsync(EasyPostShipment shipment, EasyPostConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var response = await request
					.AppendPathSegment("shipments")
					.PostJsonAsync(new { shipment })
					.ReceiveJson<EasyPostShipment>();
				return response;
			});					
		}

		protected static async Task<T> TryCatchRequestAsync<T>(EasyPostConfig config, Func<IFlurlRequest, Task<T>> run)
		{
			var request = config.BaseUrl.WithBasicAuth(config.ApiKey, "");
			try
			{
				return await run(request);
			}
			catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
			{
				// candidate for retry here?
				throw new IntegrationNoResponseException(config, request.Url);
			}
			catch (FlurlHttpException ex)
			{
				var status = ex?.Call?.Response?.StatusCode;
				if (status == null) // simulate by putting laptop on airplane mode
				{
					throw new IntegrationNoResponseException(config, request.Url);
				}
				if (status == 401)
				{
					throw new IntegrationAuthFailedException(config, request.Url, (int)status);
				}
				var body = await ex.Call.Response.GetJsonAsync();
				throw new IntegrationErrorResponseException(config, request.Url, (int)status, body);
			}
		}
	}
}
