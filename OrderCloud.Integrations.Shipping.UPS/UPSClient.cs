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
		public async static Task<UPSRestResponseBody> ShopRates(UPSRestRequestBody rateRequest, UPSConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var response = await request
					.AppendPathSegments("rating", "Shop")
					.PostJsonAsync(rateRequest)
					.ReceiveJson<UPSRestResponseBody>();
				return response;
			});		
		}

		protected static async Task<T> TryCatchRequestAsync<T>(UPSConfig config, Func<IFlurlRequest, Task<T>> run)
		{
			var request = config.BaseUrl.WithHeaders(new { ContentType = "application/json", AccessLicenseNumber = config.ApiKey });
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
				var body = await ex.Call.Response.GetJsonAsync<UPSErrorBody>();
				// here's a list of possible error codes https://www.easypost.com/errors-guide#error-codes
				throw new IntegrationErrorResponseException(config, request.Url, (int)status, body);
			}
		}
	}
}
