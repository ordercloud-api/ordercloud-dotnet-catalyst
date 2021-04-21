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
	public class VerifiedUserContext
	{
		/// <summary>
		/// An OrderCloud Client object for making request to the API in the context of the authenticated user.
		/// </summary>
		public IOrderCloudClient OcClient
		{
			get
			{
				if (ocClient == null)
				{
					ocClient = GetToken().BuildClient();
				}
				return ocClient;
			}
		}

		/// <summary>
		/// Full details of the authenticated user. Can be accessed after ReqestMeUserDetailsAsync() has been called.
		/// </summary>
		public MeUser MeUser => meUser ?? throw new UserContextException("MeUser is not populated. Call VerifiedUserContext.RequestMeUserAsync() to populate.");

		public string AccessToken => GetToken().AccessToken;
		public string Username => GetToken().Username;
		public bool IsAnonToken => GetToken().AnonOrderID != null;
		public bool IsPortalIssuedToken => GetToken().CompanyInteropID != null;
		public bool IsImpersonationToken => GetToken().ImpersonatingUserDatabaseID != null;
		public ImmutableList<string> AvailableRoles => ImmutableList.ToImmutableList(GetToken().Roles);
		public CommerceRole CommerceRole => GetCommerceRole(GetToken().UserType);
		public string TokenApiUrl => GetToken().ApiUrl;
		public string TokenAuthUrl => GetToken().AuthUrl;
		public string TokenClientID => GetToken().ClientID;
		public DateTime TokenExpiresUTC => GetToken().ExpiresUTC;
		public DateTime TokenNotValidBeforeUTC => GetToken().NotValidBeforeUTC;

		private MeUser meUser;
		private JwtOrderCloud token;
		private IOrderCloudClient ocClient;
		private readonly ISimpleCache _cache;
		private readonly IOrderCloudClient _oc;

		public VerifiedUserContext(ISimpleCache cache, IOrderCloudClient oc)
		{
			_cache = cache;
			_oc = oc;
		}

		/// <summary>
		/// Requests full details of the authenticated user from OrderCloud. Sets the MeDetails property.
		/// </summary>
		public async Task<MeUser> RequestMeUserAsync()
		{
			return meUser ??= await _oc.Me.GetAsync(GetToken().AccessToken);
		}

		/// <summary>
		/// Requests full details of the authenticated user from OrderCloud. Sets the MeDetails property.
		/// </summary>
		public async Task<T> RequestMeUserAsync<T>()
			where T : MeUser
		{
			meUser ??= await _oc.Me.GetAsync<T>(GetToken().AccessToken);
			return (T) meUser;
		}

		/// <summary>
		/// Verifies an HttpRequest through the OrderCloud token it contains and sets the User Context.
		/// </summary>
		public async Task VerifyAsync(HttpRequest request, List<string> requiredRoles = null)
		{
			var token = request.GetOrderCloudToken();
			await VerifyAsync(token, requiredRoles);
		}

		/// <summary>
		/// Verifies an OrderCloud token and sets the User Context.
		/// </summary>
		public async Task VerifyAsync(string token, List<string> requiredRoles = null)
		{
			if (string.IsNullOrEmpty(token))
				throw new UnAuthorizedException();

			var parsedToken = new JwtOrderCloud(token);

			if (parsedToken.ClientID == null || parsedToken.NotValidBeforeUTC > DateTime.UtcNow || parsedToken.ExpiresUTC < DateTime.UtcNow)
				throw new UnAuthorizedException();

			// we've validated the token as much as we can on this end, go make sure it's ok on OC	
			bool isValid;
			// some valid tokens - e.g. those from the portal - do not have a "kid"
			if (parsedToken.KeyID == null)
			{
				isValid = await ValidateTokenWithMeGet(parsedToken); // also sets meUser field;
			} else
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
			this.token = parsedToken;
		}

		private async Task<bool> ValidateTokenWithMeGet(JwtOrderCloud jwt)
		{
			var cacheKey = jwt.AccessToken;

			return await _cache.GetOrAddAsync(cacheKey, TimeSpan.FromHours(1), async () =>
			{
				try
				{
					meUser = await _oc.Me.GetAsync();
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

		private async Task<bool> ValidateTokenWithKeyID(JwtOrderCloud jwt)
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

		private JwtOrderCloud GetToken()
		{
			return token ?? throw new UserContextException("No user token verified in this context. Add [OrderCloudUserAuth] on the controller route or call VerifyAsync(). Also make sure VerifiedUserContext and downstream services are request-scoped.");
		}

		private static CommerceRole GetCommerceRole(string userType)
		{
			switch (userType?.ToLower())
			{
				case "buyer":
					return CommerceRole.Buyer;
				case "seller":
				case "admin":
					return CommerceRole.Seller;
				case "supplier":
					return CommerceRole.Supplier;
				default:
					throw new Exception("unknown user type: " + userType);
			}
		}
	}
}
