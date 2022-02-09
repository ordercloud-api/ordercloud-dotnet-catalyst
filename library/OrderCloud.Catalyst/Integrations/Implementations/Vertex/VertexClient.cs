using Flurl.Http;
using Flurl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace OrderCloud.Catalyst
{
	public class VertexClient
	{
		protected const string ApiUrl = "https://restconnect.vertexsmb.com";
		protected const string AuthUrl = "https://auth.vertexsmb.com";
		protected DateTimeOffset? CurrentTokenExpires = null;
		protected readonly VertexConfig _config;
		protected VertexTokenResponse _token;

		public VertexClient(VertexConfig config)
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
				var status = ex?.Call?.Response?.StatusCode;
				if (status == null) // simulate by putting laptop on airplane mode
				{
					throw new IntegrationNoResponseException(_config, url);
				}
				var body = await ex.Call.Response.GetJsonAsync<VertexResponse<VertexCalculateTaxResponse>>();
				throw new IntegrationErrorResponseException(_config, url, ex.Call.Response.StatusCode, body.errors);
			}
		}


		protected async Task<VertexTokenResponse> GetToken(VertexConfig config)
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
				var status = ex?.Call?.Response?.StatusCode;
				if (status == null) // simulate by putting laptop on airplane mode
				{
					throw new IntegrationNoResponseException(_config, url);
				}
				throw new IntegrationAuthFailedException(_config, url, (int)status);
			}
		}
	}
}
