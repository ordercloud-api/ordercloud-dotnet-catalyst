using Flurl.Http;
using Flurl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrderCloud.Catalyst;

namespace OrderCloud.Catalyst
{
	public class VertexClient
	{
		protected const string ApiUrl = "https://restconnect.vertexsmb.com";
		protected const string AuthUrl = "https://auth.vertexsmb.com";
		protected readonly VertexOCIntegrationConfig _config;
		protected VertexTokenResponse _token;

		public VertexClient(VertexOCIntegrationConfig config)
		{
			_config = config;
		}

		public async Task<VertexCalculateTaxResponse> CalculateTax(VertexCalculateTaxRequest request)
		{
			return await MakeRequest<VertexCalculateTaxResponse>(() => 
				$"{ApiUrl}/vertex-restapi/v1/sale"
					.WithOAuthBearerToken(_token.access_token)
					.AllowAnyHttpStatus()
					.PostJsonAsync(request)
			);
		}

		protected async Task<T> MakeRequest<T>(Func<Task<IFlurlResponse>> request)
		{
			if (_token?.access_token == null)
			{
				_token = await GetToken(_config);
			}
			var response = await (await request()).GetJsonAsync<VertexResponse<T>>();
			if (response.errors.Exists(e => e.detail == "invalid access token"))
			{
				// refresh the token
				_token = await GetToken(_config);
				// try the request again
				response = await (await request()).GetJsonAsync<VertexResponse<T>>();
			}

			// Catch and throw any api errors 
			Require.That(response.errors.Count == 0, new VertexException(response.errors));

			return response.data;
		}

		protected async Task<VertexTokenResponse> GetToken(VertexOCIntegrationConfig config)
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
			var response = await $"{AuthUrl}/identity/connect/token".PostUrlEncodedAsync(body);
			var token = await response.GetJsonAsync<VertexTokenResponse>();
			return token;
		}
	}
}
