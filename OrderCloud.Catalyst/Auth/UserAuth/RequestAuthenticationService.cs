using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// Injectable service to aid with getting, decoding, and verifying OrderCloud auth tokens on an HttpRequest. 
	/// </summary>
	public class RequestAuthenticationService
	{
		private readonly ISimpleCache _cache;
		private readonly IOrderCloudClient _oc;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public RequestAuthenticationService(ISimpleCache cache, IOrderCloudClient oc, IHttpContextAccessor httpContextAccessor)
		{
			_cache = cache;
			_oc = oc;
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// Get a raw OrderCloud token from the provided request headers
		/// </summary>
		public static string GetToken(HttpRequest request)
		{
			if (!request.Headers.TryGetValue("Authorization", out var header))
				return null;

			var parts = header.FirstOrDefault()?.Split(new[] { ' ' }, 2);
			if (parts?.Length != 2)
				return null;

			if (parts[0] != "Bearer")
				return null;

			var accessToken = parts[1].Trim();
			Require.That(!string.IsNullOrEmpty(accessToken), new UnAuthorizedException());

			return accessToken;
		}

		/// <summary>
		/// Get a raw OrderCloud token from the current HttpContext request headers
		/// </summary>
		public string GetToken()
		{
			return GetToken(_httpContextAccessor.HttpContext.Request);
		}

		/// <summary>
		/// Get a strongly typed model of the OrderCloud token from the provided request headers
		/// </summary>
		public static DecodedToken GetDecodedToken(HttpRequest request)
		{
			var token = GetToken(request);
			return new DecodedToken(token);
		}

		/// <summary>
		/// Get a strongly typed model of the OrderCloud token from the current HttpContext request headers
		/// </summary>
		public DecodedToken GetDecodedToken()
		{
			return GetDecodedToken(_httpContextAccessor.HttpContext.Request);
		}

		/// <summary>
		/// Verifies the provided OrderCloud token. Throws 401 if invalid or 403 if insufficient roles.
		/// </summary>
		public async Task<DecodedToken> VerifyTokenAsync(string token, IEnumerable<string> requiredRoles = null, IEnumerable<CommerceRole> allowedUserTypes = null)
		{
			Require.That(!string.IsNullOrEmpty(token), new UnAuthorizedException());

			var decodedToken = new DecodedToken(token);

			Require.That(decodedToken.NotValidBeforeUTC < DateTime.UtcNow && decodedToken.ExpiresUTC > DateTime.UtcNow,
				new UnAuthorizedException());

			// we've validated the token as much as we can on this end, go make sure it's ok on OC	
			bool isValid;
			// some valid tokens - e.g. those from the portal - do not have a "kid"
			if (decodedToken.KeyID == null)
			{
				isValid = await VerifyTokenWithMeGet(decodedToken); // also sets meUser field;
			}
			else
			{
				isValid = await VerifyTokenWithKeyID(decodedToken);
			}

			if (!isValid)
			{
				Require.That(decodedToken.ApiUrl == _oc?.Config?.ApiUrl, 
					new WrongEnvironmentException(new WrongEnvironmentError()
					{
						ExpectedEnvironment = _oc?.Config?.ApiUrl,
						TokenIssuerEnvironment = decodedToken.ApiUrl
					}
				));
			}

			Require.That(isValid, new UnAuthorizedException());

			Require.That(allowedUserTypes.IsNullOrEmpty() || allowedUserTypes.Contains(decodedToken.CommerceRole),
				new InvalidUserTypeException(new InvalidUserTypeError()
				{
					ThisUserType = decodedToken?.CommerceRole.ToString(),
					UserTypesThatCanAccess = allowedUserTypes?.Select(x => x.ToString())?.ToList()
				})
			);

			Require.That(requiredRoles.IsNullOrEmpty() || requiredRoles.Any(role => decodedToken.Roles.Contains(role)),
				new InsufficientRolesException(new InsufficientRolesError()
				{
					SufficientRoles = requiredRoles?.ToList(),
					AssignedRoles = decodedToken.Roles.ToList()
				})
			);

			return decodedToken;
		}

		/// <summary>
		/// Verifies the provided HttpRequest's OrderCloud Token. Throws 401 if invalid or 403 if insufficient roles.
		/// </summary>
		public async Task<DecodedToken> VerifyTokenAsync(HttpRequest request, IEnumerable<string> requiredRoles = null, IEnumerable<CommerceRole> allowedUserTypes = null)
		{
			var token = GetToken(request);
			return await VerifyTokenAsync(token, requiredRoles, allowedUserTypes);
		}


		/// <summary>
		/// Verifies the current HttpContext request's OrderCloud token. Throws 401 if invalid or 403 if insufficient roles.
		/// </summary>
		public async Task<DecodedToken> VerifyTokenAsync(IEnumerable<string> requiredRoles = null, IEnumerable<CommerceRole> allowedUserTypes = null)
		{
			return await VerifyTokenAsync(_httpContextAccessor.HttpContext.Request, requiredRoles, allowedUserTypes);
		}


		/// <summary>
		/// Get the full details of the currently authenticated user based on the HttpContext request token
		/// </summary>
		public async Task<T> GetUserAsync<T>()
			where T : MeUser
		{
			var token = GetToken();
			return await _oc.Me.GetAsync<T>(token);
		}


		/// <summary>
		/// Get the full details of the currently authenticated user based on the HttpContext request token
		/// </summary>
		public async Task<MeUser> GetUserAsync()
		{
			var token = GetToken();
			return await _oc.Me.GetAsync(token);
		}

		/// <summary>
		/// Get an IOrderCloudClient with token set based on the HttpContext request
		/// </summary>
		public IOrderCloudClient BuildClient()
		{
			return GetDecodedToken().BuildClient();
		}

		/// <summary>
		/// Verifiy the validity of an OrderCloud token, given details about the public key.
		/// </summary>
		public static bool VerifyTokenCryptoValid(string accessToken, PublicKey publicKey)
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

		private async Task<bool> VerifyTokenWithMeGet(DecodedToken jwt)
		{
			var cacheKey = jwt.AccessToken;

			return await _cache.GetOrAddAsync(cacheKey, TimeSpan.FromHours(1), async () =>
			{
				try
				{
					var meUser = await _oc.Me.GetAsync(jwt.AccessToken);
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

		private async Task<bool> VerifyTokenWithKeyID(DecodedToken jwt)
		{
			var cacheKey = jwt.KeyID;

			return await _cache.GetOrAddAsync(cacheKey, TimeSpan.FromDays(30), async () =>
			{
				try
				{
					var publicKey = await _oc.Certs.GetPublicKeyAsync(jwt.KeyID);
					return VerifyTokenCryptoValid(jwt.AccessToken, publicKey);
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
