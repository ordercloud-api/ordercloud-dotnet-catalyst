using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public class UserContextProvider
	{
		private readonly ISimpleCache _cache;
		private readonly IOrderCloudClient _oc;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserContextProvider(ISimpleCache cache, IOrderCloudClient oc, IHttpContextAccessor httpContextAccessor)
		{
			_cache = cache;
			_oc = oc;
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// Get a raw OrderCloud token from the current HttpRequest headers
		/// </summary>
		public string GetOAuthToken()
		{
			return GetOAuthToken(_httpContextAccessor.HttpContext.Request);
		}

		/// <summary>
		/// Get a raw OrderCloud token from the provided request headers
		/// </summary>
		public static string GetOAuthToken(HttpRequest request)
		{
			if (!request.Headers.TryGetValue("Authorization", out var header))
				return null;

			var parts = header.FirstOrDefault()?.Split(new[] { ' ' }, 2);
			if (parts?.Length != 2)
				return null;

			if (parts[0] != "Bearer")
				return null;

			var accessToken = parts[1].Trim();
			if (string.IsNullOrEmpty(accessToken))
				throw new UnAuthorizedException();
			return accessToken;
		}

		/// <summary>
		/// Get a parsed model of the OrderCloud token from the current HttpRequest headers
		/// </summary>
		public UserContext GetUserContext()
		{
			return GetUserContext(_httpContextAccessor.HttpContext.Request);
		}

		/// <summary>
		/// Get a parsed model of the OrderCloud token from the provided request headers
		/// </summary>
		public static UserContext GetUserContext(HttpRequest request)
		{
			var token = GetOAuthToken(request);
			return new UserContext(token);
		}

		/// <summary>
		/// Verifies the OrderCloud token on the current HttpRequest
		/// </summary>
		public async Task<UserContext> VerifyTokenAsync(List<string> requiredRoles = null)
		{
			return await VerifyTokenAsync(_httpContextAccessor.HttpContext.Request, requiredRoles);
		}

		/// <summary>
		/// Verifies the OrderCloud token on the provided HttpRequest
		/// </summary>
		public async Task<UserContext> VerifyTokenAsync(HttpRequest request, List<string> requiredRoles = null)
		{
			var token = GetOAuthToken(request);
			return await VerifyTokenAsync(token, requiredRoles);
		}

		/// <summary>
		/// Verifies the provided OrderCloud token
		/// </summary>
		public async Task<UserContext> VerifyTokenAsync(string token, List<string> requiredRoles = null)
		{
			if (string.IsNullOrEmpty(token))
				throw new UnAuthorizedException();

			var parsedToken = new UserContext(token);

			if (parsedToken.ClientID == null || parsedToken.NotValidBeforeUTC > DateTime.UtcNow || parsedToken.ExpiresUTC < DateTime.UtcNow)
				throw new UnAuthorizedException();

			// we've validated the token as much as we can on this end, go make sure it's ok on OC	
			bool isValid;
			// some valid tokens - e.g. those from the portal - do not have a "kid"
			if (parsedToken.KeyID == null)
			{
				isValid = await VerifyTokenWithMeGet(parsedToken); // also sets meUser field;
			}
			else
			{
				isValid = await VerifyTokenWithKeyID(parsedToken);
			}

			if (!isValid)
				throw new UnAuthorizedException();

			if (requiredRoles != null && requiredRoles.Count > 0 && !requiredRoles.Any(role => parsedToken.Roles.Contains(role)))
			{
				throw new InsufficientRolesException(new InsufficientRolesError()
				{
					SufficientRoles = requiredRoles,
					AssignedRoles = parsedToken.Roles.ToList()
				});
			}
			return parsedToken;
		}

		/// <summary>
		/// Get the full details of the currently authenticated user
		/// </summary>
		public async Task<T> GetMeAsync<T>(string accessToken = null)
			where T : MeUser
		{
			var token = accessToken ?? GetOAuthToken();
			return await _oc.Me.GetAsync<T>(token);
		}


		/// <summary>
		/// Get the full details of the currently authenticated user
		/// </summary>
		public async Task<MeUser> GetMeAsync(string accessToken = null)
		{
			var token = accessToken ?? GetOAuthToken();
			return await _oc.Me.GetAsync(token);
		}

		/// <summary>
		/// Get an IOrderCloudClient with token set based on the token for the HttpRequest
		/// </summary>
		public IOrderCloudClient BuildClient()
		{
			return GetUserContext().BuildClient();
		}

		/// <summary>
		/// Verifiy the validity of an OrderCloud token, given details about the public key.
		/// </summary>
		public static bool IsTokenCryptoValid(string accessToken, PublicKey publicKey)
		{
			if (publicKey == null)
			{
				return false;
			}
			var rsa = new RSACryptoServiceProvider(2048);
			rsa.ImportParameters(new RSAParameters
			{
				Modulus = FromBase64Url(publicKey.n),
				Exponent = FromBase64Url(publicKey.e)
			});
			var rsaSecurityKey = new RsaSecurityKey(rsa);

			var result = new JsonWebTokenHandler().ValidateToken(accessToken, new TokenValidationParameters
			{
				IssuerSigningKey = rsaSecurityKey,
				RequireSignedTokens = true,
				ValidateIssuerSigningKey = true,
				ValidateLifetime = true,
				LifetimeValidator = (nbf, exp, _, __) => nbf < DateTime.UtcNow && exp > DateTime.UtcNow,
				ValidateIssuer = false,
				RequireExpirationTime = true,
				ValidateAudience = false
			});
			return result.IsValid;
		}

		private async Task<bool> VerifyTokenWithMeGet(UserContext jwt)
		{
			var cacheKey = jwt.AccessToken;

			return await _cache.GetOrAddAsync(cacheKey, TimeSpan.FromHours(1), async () =>
			{
				try
				{
					var meUser = await _oc.Me.GetAsync();
					return meUser != null && meUser.Active;
				}
				catch (OrderCloudException ex)
				{
					throw ex;
				}
				catch (Exception ex)
				{
					await _cache.RemoveAsync(cacheKey); // not their fault, don't make them wait 1 hr   
					return false;
				}
			});
		}

		private async Task<bool> VerifyTokenWithKeyID(UserContext jwt)
		{
			var cacheKey = jwt.KeyID;

			return await _cache.GetOrAddAsync(cacheKey, TimeSpan.FromDays(30), async () =>
			{
				try
				{
					var publicKey = await _oc.Certs.GetPublicKeyAsync(jwt.KeyID);
					return IsTokenCryptoValid(jwt.AccessToken, publicKey);
				}
				catch (OrderCloudException ex)
				{
					throw ex;
				}
				catch (Exception ex)
				{
					await _cache.RemoveAsync(cacheKey); // not their fault, don't make them wait 5 min   
					return false;
				}
			});
		}

		private static byte[] FromBase64Url(string base64Url)
		{
			string padded = base64Url.Length % 4 == 0
				? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
			string base64 = padded.Replace("_", "/")
								  .Replace("-", "+");
			return Convert.FromBase64String(base64);
		}
	}
}
