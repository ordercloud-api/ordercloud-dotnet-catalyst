using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{	
	public class OrderCloudUserAuthProvider
	{
		private readonly ISimpleCache _cache;
		private readonly IOrderCloudClient _oc;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public OrderCloudUserAuthProvider(ISimpleCache cache, IOrderCloudClient oc, IHttpContextAccessor httpContextAccessor)
		{
			_cache = cache;
			_oc = oc;
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// Get a raw OrderCloud token
		/// </summary>
		public string GetOAuthToken()
		{
			return GetOAuthToken(_httpContextAccessor.HttpContext.Request);
		}

		/// <summary>
		/// Get a raw OrderCloud token
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

			return parts[1].Trim();
		}

		/// <summary>
		/// Get a parsed model of the OrderCloud token for the HttpRequest
		/// </summary>
		public OrderCloudToken GetToken()
		{
			return GetToken(_httpContextAccessor.HttpContext.Request);
		}

		/// <summary>
		/// Get a parsed model of the OrderCloud token
		/// </summary>
		public static OrderCloudToken GetToken(HttpRequest request)
		{
			var token = GetOAuthToken(request);
			if (string.IsNullOrEmpty(token))
				throw new UnAuthorizedException();
			return new OrderCloudToken(token);
		}

		/// <summary>
		/// Verifies an HttpRequest through the OrderCloud token it contains.
		/// </summary>
		public async Task<OrderCloudToken> VerifyTokenAsync(List<string> requiredRoles = null)
		{
			return await VerifyTokenAsync(_httpContextAccessor.HttpContext.Request, requiredRoles);
		}

		/// <summary>
		/// Verifies an HttpRequest through the OrderCloud token it contains.
		/// </summary>
		public async Task<OrderCloudToken> VerifyTokenAsync(HttpRequest request, List<string> requiredRoles = null)
		{
			var token = GetOAuthToken(request);
			return await VerifyTokenAsync(token, requiredRoles);
		}

		/// <summary>
		/// Verifies an OrderCloud token
		/// </summary>
		public async Task<OrderCloudToken> VerifyTokenAsync(string token, List<string> requiredRoles = null)
		{
			if (string.IsNullOrEmpty(token))
				throw new UnAuthorizedException();

			var parsedToken = new OrderCloudToken(token);

			if (parsedToken.ClientID == null || parsedToken.NotValidBeforeUTC > DateTime.UtcNow || parsedToken.ExpiresUTC < DateTime.UtcNow)
				throw new UnAuthorizedException();

			// we've validated the token as much as we can on this end, go make sure it's ok on OC	
			bool isValid;
			// some valid tokens - e.g. those from the portal - do not have a "kid"
			if (parsedToken.KeyID == null)
			{
				isValid = await ValidateTokenWithMeGet(parsedToken); // also sets meUser field;
			}
			else
			{
				isValid = await ValidateTokenWithKeyID(parsedToken);
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
		public async Task<T> GetMeUserAsync<T>()
			where T : MeUser
		{
			var token = GetOAuthToken();
			if (string.IsNullOrEmpty(token))
				throw new UnAuthorizedException();
			return await _oc.Me.GetAsync<T>(token);
		}


		/// <summary>
		/// Get the full details of the currently authenticated user
		/// </summary>
		public async Task<MeUser> GetMeUserAsync()
		{
			var token = GetOAuthToken();
			if (string.IsNullOrEmpty(token))
				throw new UnAuthorizedException();
			return await _oc.Me.GetAsync(token);
		}

		/// <summary>
		/// Get an IOrderCloudClient with token set based on the token for the HttpRequest
		/// </summary>
		public IOrderCloudClient BuildClient()
		{
			return GetToken().BuildClient();
		}

		private async Task<bool> ValidateTokenWithMeGet(OrderCloudToken jwt)
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

		private async Task<bool> ValidateTokenWithKeyID(OrderCloudToken jwt)
		{
			var cacheKey = jwt.KeyID;

			return await _cache.GetOrAddAsync(cacheKey, TimeSpan.FromDays(30), async () =>
			{
				try
				{
					var publicKey = await _oc.Certs.GetPublicKeyAsync(jwt.KeyID);
					return jwt.IsTokenCryptoValid(publicKey);
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
	}
}
