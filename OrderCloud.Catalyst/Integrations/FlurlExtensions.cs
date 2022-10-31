using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public static class FlurlExtensions
	{
		public static async Task<TSuccessBody> GetJsonWithErrorHandlingAsync<TSuccessBody, TErrorBody>(this IFlurlRequest request, OCIntegrationConfig config)
		{
			return await ErrorHandlingAsync<TSuccessBody, TErrorBody>(request, config, async r => await r.GetJsonAsync<TSuccessBody>());
		}

		public static async Task<IFlurlResponse> PostJsonWithErrorHandlingAsync<TErrorBody>(this IFlurlRequest request, object data, OCIntegrationConfig config)
		{
			return await ErrorHandlingAsync<IFlurlResponse, TErrorBody>(request, config, async r => await request.PostJsonAsync(data));
		}

		public static async Task<IFlurlResponse> PostWithErrorHandlingAsync<TErrorBody>(this IFlurlRequest request, OCIntegrationConfig config)
		{
			return await ErrorHandlingAsync<IFlurlResponse, TErrorBody>(request, config, async r => await request.PostAsync());
		}

		private static async Task<T> ErrorHandlingAsync<T, TErrorBody>(this IFlurlRequest request, OCIntegrationConfig config, Func<IFlurlRequest, Task<T>> run)
		{
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
				if (status == 401 || status == 403)
				{
					throw new IntegrationAuthFailedException(config, request.Url, (int)status);
				}
				var body = await ex.Call.Response.GetJsonAsync<TErrorBody>();
				throw new IntegrationErrorResponseException(config, request.Url, (int)status, body);
			}
		}
	}
}
