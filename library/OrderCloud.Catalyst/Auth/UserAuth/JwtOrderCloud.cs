using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// The raw jwt access token 
	/// </summary>
	public class JwtOrderCloud
	{
		/// <summary>
		/// The raw jwt access token 
		/// </summary>
		public string AccessToken { get; }
		/// <summary>
		/// The signing key ID of the token. "mpid" claim. Null when Portal issued the token. 
		/// </summary>
		public string KeyID { get; }
		/// <summary>
		/// Anonymous order ID on the token. "orderid" claim. Null unless the user is anonymous.
		/// </summary>
		public string AnonOrderID { get; }
		/// <summary>
		/// Username on the token. "usr" claim. Always non-null.
		/// </summary>
		public string Username { get; }
		/// <summary>
		/// OrderCloud roles on the token. "role" claim. Always non-null.
		/// </summary>
		public List<string> Roles { get; } = new List<string>();
		/// <summary>
		/// The authentication Url on the token. "iss" claim. Always non-null.
		/// </summary>
		public string AuthUrl { get; }
		/// <summary>
		/// The api Url on the token. "aud" claim. Always non-null.
		/// </summary>
		public string ApiUrl { get; }
		/// <summary>
		/// The user type ("buyer", "supplier", "admin") on the token. "usrtype" claim. Always non-null.
		/// </summary>
		public string UserType { get; }
		/// <summary>
		/// The client ID on the token. "cid" claim. Always non-null.
		/// </summary>
		public string ClientID { get; }
		/// <summary>
		/// The expiry time of the token. "exp" claim. Always non-null.
		/// </summary>
		public DateTime ExpiresUTC { get; }
		/// <summary>
		/// The time the token is not valid before. "nbf" claim. Always non-null.
		/// </summary>
		public DateTime NotValidBeforeUTC { get; }
		/// <summary>
		/// The internal database ID of the user. "u" claim from plateform, "uid" claim from portal. Always non-null.
		/// </summary>
		public string UserDatabaseID { get; }
		/// <summary>
		/// The internal database ID of the user's company. "coid" claim. Null unless Portal issued the token. 
		/// </summary>
		public string CompanyDatabaseID { get; }
		/// <summary>
		/// The internal database ID of the user's seller organization. "sid" claim. Null unless Portal issued the token. 
		/// </summary>
		public string SellerDatabaseID { get; }
		/// <summary>
		/// The public ID of the user's company (Buyer, Supplier, Seller). "coiid" claim. Null unless Portal issued the token. 
		/// </summary>
		public string CompanyInteropID { get; }
		/// <summary>
		/// The public ID of the user's seller organization. "siid" claim. Null unless Portal issued the token. 
		/// </summary>
		public string SellerInteropID { get; }
		/// <summary>
		/// The date the token was issued. "iat" claim. Null unless Portal issued the token. 
		/// </summary>
		public DateTime? IssuedAtUTC { get; }
		/// <summary>
		/// The Marketplace ID of the token. "mpid" claim. Null unless Portal issued the token. 
		/// </summary>
		public string MarketplaceID { get; }
		/// <summary>
		/// The internal database ID of the user who requested an impersonation token. "imp" claim. Null unless token is from impersonation.
		/// </summary>
		public string ImpersonatingUserDatabaseID { get; }

		public JwtOrderCloud() { }

		/// <summary>
		/// Create a JwtOrderCloud from a raw json web token.
		/// </summary>
		public JwtOrderCloud(string token) 
		{
			var jwt = new JwtSecurityToken(token);
			var lookup = jwt.Claims.ToLookup(c => c.Type, c => c.Value);

			AccessToken = token;
			KeyID = GetHeader(jwt, "kid");

			AnonOrderID = lookup["orderid"].FirstOrDefault();
			Username = lookup["usr"].FirstOrDefault();
			Roles = lookup["role"].ToList();
			AuthUrl = lookup["iss"].FirstOrDefault();
			ApiUrl = lookup["aud"].FirstOrDefault();
			UserType = lookup["usrtype"].FirstOrDefault();
			ClientID = lookup["cid"].FirstOrDefault();
			ExpiresUTC = UnixToUTCDateTime(lookup["exp"].FirstOrDefault()) ?? throw new ArgumentNullException("Token must contain \"exp\" claim");
			NotValidBeforeUTC = UnixToUTCDateTime(lookup["nbf"].FirstOrDefault()) ?? throw new ArgumentNullException("Token must contain \"nbf\" claim"); ;
			UserDatabaseID = lookup["u"].FirstOrDefault() ?? lookup["uid"].FirstOrDefault();
			CompanyDatabaseID = lookup["coid"].FirstOrDefault();
			SellerDatabaseID = lookup["sid"].FirstOrDefault();
			CompanyInteropID = lookup["coiid"].FirstOrDefault();
			IssuedAtUTC = UnixToUTCDateTime(lookup["iat"].FirstOrDefault());
			SellerInteropID = lookup["siid"].FirstOrDefault();
			MarketplaceID = lookup["mpid"].FirstOrDefault();
			ImpersonatingUserDatabaseID = lookup["imp"].FirstOrDefault();
		}

		/// <summary>
		/// Create a new IOrderCloudClient with the context of this json web token
		/// </summary>
		public IOrderCloudClient BuildClient()
		{
			var client = new OrderCloudClient(new OrderCloudClientConfig()
			{
				ApiUrl = ApiUrl,
				AuthUrl = AuthUrl,
				ClientId = ClientID,
				Roles = new[] { ApiRole.FullAccess }
			})
			{
				TokenResponse = new TokenResponse()
				{
					AccessToken = AccessToken,
					ExpiresUtc = ExpiresUTC
				}
			};
			return client;
		}

		/// <summary>
		/// Create a fake token for unit testing. (Grants no access to the api). 
		/// </summary>
		public static string CreateFake(
			string clientID = null,
			List<string> roles = null,
			DateTime? expiresUTC = null,
			DateTime? notValidBeforeUTC = null,
			string username = null,
			string keyID = null, 
			string anonOrderID = null,
			string authUrl = null,
			string apiUrl = null,
			string userType = null,
			string userDatabaseID = null,
			string companyDatabaseID = null,
			string sellerDatabaseID = null,
			string companyInteropID = null,
			DateTime? issuedAtUTC = null,
			string sellerInteropID = null,
			string marketplaceID = null,
			string impersonatingUserDatabaseID = null
		)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("blahblahblahblahblahblahblahblahblahblah"));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var header = new JwtHeader(creds);
			if (keyID != null)
			{
				header["kid"] = keyID;
			}
			var claims = (roles ?? new List<string>()).Select(r => new Claim("role", r)).ToList();

			AddClaimIfNotNull(claims, "orderid", anonOrderID);
			AddClaimIfNotNull(claims, "usr", username);
			AddClaimIfNotNull(claims, "usrtype", userType);
			AddClaimIfNotNull(claims, "cid", clientID);
			AddClaimIfNotNull(claims, "u", userDatabaseID);
			AddClaimIfNotNull(claims, "coid", companyDatabaseID);
			AddClaimIfNotNull(claims, "sid", sellerDatabaseID);
			AddClaimIfNotNull(claims, "coiid", companyInteropID);
			AddClaimIfNotNull(claims, "iat", UTCDateTimeToUnix(issuedAtUTC));
			AddClaimIfNotNull(claims, "siid", sellerInteropID);
			AddClaimIfNotNull(claims, "mpid", marketplaceID);
			AddClaimIfNotNull(claims, "imp", impersonatingUserDatabaseID);

			var payload = new JwtPayload(
				issuer: authUrl ?? "mockdomain.com",
				audience: apiUrl ?? "mockdomain.com",
				claims: claims,
				expires: expiresUTC ?? DateTime.Now.AddMinutes(30),
				notBefore: notValidBeforeUTC ?? DateTime.Now
			);

			var token = new JwtSecurityToken(header, payload);
			
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		/// <summary>
		/// Verifiy the validity of an OrderCloud token, given details about the public key.
		/// </summary>
		public bool IsTokenCryptoValid(PublicKey publicKey)
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

			var result = new JsonWebTokenHandler().ValidateToken(AccessToken, new TokenValidationParameters
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

		private static void AddClaimIfNotNull(List<Claim> claims, string type, string value)
		{
			if (value != null) { claims.Add(new Claim(type, value)); }
		}

		private static string GetHeader(JwtSecurityToken jwt, string key)
		{
			return jwt.Header.FirstOrDefault(t => t.Key == key).Value?.ToString();
		}

		private static DateTime? UnixToUTCDateTime(string unix)
		{
			if (unix == null) { return null; }
			var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return dtDateTime.AddSeconds(int.Parse(unix));
		}

		private static string UTCDateTimeToUnix(DateTime? utc)
		{
			if (utc == null) { return null; }
			var span = utc - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return ((int)span.Value.TotalSeconds).ToString();
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
