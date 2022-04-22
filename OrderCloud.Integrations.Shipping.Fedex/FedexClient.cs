using Flurl.Http;
using Flurl;
using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using OrderCloud.Catalyst;


namespace OrderCloud.Integrations.Shipping.Fedex
{
	public class FedexClient
	{
		private FedexConfig _defaultConfig;
		private FedexTokenResponse _token;

		public FedexClient(FedexConfig defaultConfig)
		{
			_defaultConfig = defaultConfig;
		}

		// https://developer.fedex.com/api/en-us/catalog/rate/v1/docs.html
		public async Task<FedexRateResponse> GetRates(FedexRateRequestBody rateRequestBody, FedexConfig optionalOverride = null)
		{
			var token = await GetToken(optionalOverride);
			var config = optionalOverride ?? _defaultConfig;
			var request = config.BaseUrl
				.AppendPathSegments("rate", "v1", "rates", "quotes")
				.WithOAuthBearerToken(token.access_token);
			try
			{ 
				var response = await request
					.PostJsonAsync(rateRequestBody)
					.ReceiveJson<FedexRateResponse>();
				return response;
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
				var body = await ex.Call.Response.GetJsonAsync<FedexError>();
				throw new IntegrationErrorResponseException(config, request.Url, (int)status, body);
			}
		}

		protected async Task<FedexTokenResponse> GetToken(FedexConfig optionalOverride)
		{
			if (optionalOverride != null)
			{
				return await RequestToken(optionalOverride);
			} else if (_token == null || IsJWTExpired(_token))
			{
				_token = await RequestToken(_defaultConfig);
			}
			return _token;
		}

		protected bool IsJWTExpired(FedexTokenResponse response)
		{
			var handler = new JwtSecurityTokenHandler();
			var token = handler.ReadJwtToken(response.access_token);
			var exp = token.Claims.First(claim => claim.Type == "exp").Value;
			return int.Parse(exp).FromUnixEpoch() < DateTime.UtcNow;
		}

		// https://developer.fedex.com/api/en-us/catalog/authorization/v1/docs.html
		protected async Task<FedexTokenResponse> RequestToken(FedexConfig config)
		{
			var url = config.BaseUrl.AppendPathSegments("oauth", "token");
			try
			{
				var token = await url.PostUrlEncodedAsync(new
					{
						grant_type = "client_credentials",
						client_id = config.ClientID,
						client_secret = config.ClientSecret
					})
					.ReceiveJson<FedexTokenResponse>();
				return token;
			}
			catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
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
