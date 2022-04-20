using Flurl.Http;
using Flurl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Tax.Vertex
{
	public class VertexClient
	{
		protected const string ApiUrl = "https://restconnect.vertexsmb.com";
		protected const string AuthUrl = "https://auth.vertexsmb.com";

		public static async Task<VertexCalculateTaxResponse> CalculateTax(VertexCalculateTaxRequest request, VertexConfig config)
		{
			// TODO - cache the token. But, support config overrides and don't create multiple HttpClient objects in memory
			var token = await GetToken(config); 
			var url = $"{ApiUrl}/vertex-restapi/v1/sale";
			try
			{
				var response = await url
					.WithOAuthBearerToken(token.access_token)
					.PostJsonAsync(request)
					.ReceiveJson<VertexResponse<VertexCalculateTaxResponse>>();
				return response.data;
			} catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
			{
				// candidate for retry here?
				throw new IntegrationNoResponseException(config, url);
			}
			catch (FlurlHttpException ex)
			{
				var status = ex?.Call?.Response?.StatusCode;
				if (status == null) // simulate by putting laptop on airplane mode
				{
					throw new IntegrationNoResponseException(config, url);
				}
				var body = await ex.Call.Response.GetJsonAsync<VertexResponse<VertexCalculateTaxResponse>>();
				throw new IntegrationErrorResponseException(config, url, ex.Call.Response.StatusCode, body.errors);
			}
		}


		protected static async Task<VertexTokenResponse> GetToken(VertexConfig config)
		{
			var body = new
			{
				scope = "calc-rest-api",
				grant_type = "password",
				client_id = config.ClientID,
				client_secret = config.ClientSecret,
				username = config.Username,
				password = config.Password
			};
			var url = $"{AuthUrl}/identity/connect/token";
			try
			{
				var response = await url.PostUrlEncodedAsync(body);
				var token = await response.GetJsonAsync<VertexTokenResponse>();
				return token;
			} catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
			{
				// candidate for retry here?
				throw new IntegrationNoResponseException(config, url);
			}
			catch (FlurlHttpException ex)
			{
				var status = ex?.Call?.Response?.StatusCode;
				if (status == null) // simulate by putting laptop on airplane mode
				{
					throw new IntegrationNoResponseException(config, url);
				}
				throw new IntegrationAuthFailedException(config, url, (int)status);
			}
		}
	}
}
