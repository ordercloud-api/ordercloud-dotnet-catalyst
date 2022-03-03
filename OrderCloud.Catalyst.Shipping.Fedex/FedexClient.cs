using Flurl.Http;
using Flurl;
using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace OrderCloud.Catalyst.Shipping.Fedex
{
	public class FedexClient
	{
		private FedexConfig _defaultConfig;
		private FedexTokenResponse _token;

		public FedexClient(FedexConfig defaultConfig)
		{
			_defaultConfig = defaultConfig;
		}

		public async Task<FedexRateResponse> GetRates(FedexRateRequestBody request, FedexConfig optionalOverride = null)
		{
			var token = await GetToken(optionalOverride);
			var config = optionalOverride ?? _defaultConfig;
			var response = await config.BaseUrl
				.AppendPathSegments("rate", "v1", "rates", "quotes")
				.WithOAuthBearerToken(token.access_token)
				.PostJsonAsync(request)
				.ReceiveJson<FedexRateResponse>();
			return response;
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

		protected async Task<FedexTokenResponse> RequestToken(FedexConfig config)
		{
			var token = await config.BaseUrl
				.AppendPathSegments("oauth", "token")
				.PostUrlEncodedAsync(new
				{
					grant_type = "client_credentials",
					client_id = config.ClientID,
					client_secret = config.ClientSecret
				})
				.ReceiveJson<FedexTokenResponse>();
			return token;
		}
	}
}
