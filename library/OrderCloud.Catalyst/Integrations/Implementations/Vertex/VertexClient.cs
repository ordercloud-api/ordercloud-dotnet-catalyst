using Flurl.Http;
using Flurl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public class VertexClient
	{
		protected const string ApiUrl = "https://restconnect.vertexsmb.com";
		protected const string AuthUrl = "https://auth.vertexsmb.com";
		protected DateTimeOffset? CurrentTokenExpires = null;
		protected readonly VertexOCIntegrationConfig _config;
		protected VertexTokenResponse _token;

		public VertexClient(VertexOCIntegrationConfig config)
		{
			_config = config;
		}

		public async Task<VertexCalculateTaxResponse> CalculateTax(VertexCalculateTaxRequest request)
		{
			var token = await GetToken(_config);
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
				throw new IntegrationNoResponseException(_config, url);
			}
			catch (FlurlHttpException ex)
			{
				if (ex.Call.Response == null || ex.Call.Response.StatusCode > 500)  // simulate by putting laptop on airplane mode
				{
					// candidate for retry here?
					throw new IntegrationNoResponseException(_config, url);
				}
				var body = await ex.Call.Response.GetJsonAsync<VertexResponse<VertexCalculateTaxResponse>>();
				throw new IntegrationErrorResponseException(_config, url, body.errors);
			}
		}


		protected async Task<VertexTokenResponse> GetToken(VertexOCIntegrationConfig config)
		{
			if (_token?.access_token != null && CurrentTokenExpires != null && CurrentTokenExpires > DateTimeOffset.Now)
			{
				return _token;
			}

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
				_token = token;
				CurrentTokenExpires = DateTimeOffset.Now.AddSeconds(token.expires_in);
				return token;
			} catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
			{
				// candidate for retry here?
				throw new IntegrationNoResponseException(_config, url);
			}
			catch (FlurlHttpException ex)
			{
				if (ex.Call.Response == null || ex.Call.Response.StatusCode > 500)  // simulate by putting laptop on airplane mode
				{
					// candidate for retry here?
					throw new IntegrationNoResponseException(_config, url);
				}
				throw new IntegrationAuthFailedException(_config, url);
			}
		}
	}
}
