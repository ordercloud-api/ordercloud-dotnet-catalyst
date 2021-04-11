using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class JwtOrderCloud
	{
		/// <summary>
		/// The raw jwt access token 
		/// </summary>
		public string AccessToken { get; set; }
		/// <summary>
		/// The signing key ID of the token. "mpid" claim. Null when Portal issued the token. 
		/// </summary>
		public string KeyID { get; set; }
		/// <summary>
		/// Anonymous order ID on the token. "orderid" claim. Null unless the user is anonymous.
		/// </summary>
		public string AnonOrderID { get; set; }
		/// <summary>
		/// Username on the token. "usr" claim. Always non-null.
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// OrderCloud roles on the token. "role" claim. Always non-null.
		/// </summary>
		public List<string> Roles { get; set; } = new List<string>();
		/// <summary>
		/// The authentication Url on the token. "iss" claim. Always non-null.
		/// </summary>
		public string AuthUrl { get; set; }
		/// <summary>
		/// The api Url on the token. "aud" claim. Always non-null.
		/// </summary>
		public string ApiUrl { get; set; }
		/// <summary>
		/// The user type ("buyer", "supplier", "admin") on the token. "usrtype" claim. Always non-null.
		/// </summary>
		public string UserType { get; set; }
		/// <summary>
		/// The client ID on the token. "cid" claim. Always non-null.
		/// </summary>
		public string ClientID { get; set; }
		/// <summary>
		/// The expiry time of the token. "exp" claim. Always non-null.
		/// </summary>
		public DateTime? ExpiresUTC { get; set; }
		/// <summary>
		/// The time the token is not valid before. "nbf" claim. Always non-null.
		/// </summary>
		public DateTime? NotValidBeforeUTC { get; set; }
		/// <summary>
		/// The internal database ID of the user. "u" claim from plateform, "uid" claim from portal. Always non-null.
		/// </summary>
		public string UserDatabaseID { get; set; }
		/// <summary>
		/// The internal database ID of the user's company. "coid" claim. Null unless Portal issued the token. 
		/// </summary>
		public string CompanyDatabaseID { get; set; }
		/// <summary>
		/// The internal database ID of the user's seller organization. "sid" claim. Null unless Portal issued the token. 
		/// </summary>
		public string SellerDatabaseID { get; set; }
		/// <summary>
		/// The public ID of the user's company (Buyer, Supplier, Seller). "coiid" claim. Null unless Portal issued the token. 
		/// </summary>
		public string CompanyInteropID { get; set; }
		/// <summary>
		/// The public ID of the user's seller organization. "siid" claim. Null unless Portal issued the token. 
		/// </summary>
		public string SellerInteropID { get; set; }
		/// <summary>
		/// The date the token was issued. "iat" claim. Null unless Portal issued the token. 
		/// </summary>
		public DateTime? IssuedAtUTC { get; set; }
		/// <summary>
		/// The Marketplace ID of the token. "mpid" claim. Null unless Portal issued the token. 
		/// </summary>
		public string MarketplaceID { get; set; }
		/// <summary>
		/// The internal database ID of the user who requested an impersonation token. "imp" claim. Null unless token is from impersonation.
		/// </summary>
		public string ImpersonatingUserDatabaseID { get; set; }

		public JwtOrderCloud() { }

		/// <summary>
		/// Create a JwtOrderCloud from a raw jwt string.
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
			ExpiresUTC = UnixToUTCDateTime(lookup["exp"].FirstOrDefault());
			NotValidBeforeUTC = UnixToUTCDateTime(lookup["nbf"].FirstOrDefault());
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
		/// Creates a fake token (grants no access to the api) from the fields of a JwtOrderCloud. Intended for unit testing. 
		/// </summary>
		public string CreateFake()
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("blahblahblahblahblahblahblahblahblahblah"));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var header = new JwtHeader(creds)
			{
				{ "kid", KeyID }
			};
			var claims = Roles.Select(r => new Claim("role", r)).ToList();

			AddClaimIfNotNull(claims, "orderid", AnonOrderID);
			AddClaimIfNotNull(claims, "usr", Username);
			AddClaimIfNotNull(claims, "usrtype", UserType);
			AddClaimIfNotNull(claims, "cid", ClientID);
			AddClaimIfNotNull(claims, "u", UserDatabaseID);
			AddClaimIfNotNull(claims, "coid", CompanyDatabaseID);
			AddClaimIfNotNull(claims, "sid", SellerDatabaseID);
			AddClaimIfNotNull(claims, "coiid", CompanyInteropID);
			AddClaimIfNotNull(claims, "iat", UTCDateTimeToUnix(IssuedAtUTC));
			AddClaimIfNotNull(claims, "siid", SellerInteropID);
			AddClaimIfNotNull(claims, "mpid", MarketplaceID);
			AddClaimIfNotNull(claims, "imp", ImpersonatingUserDatabaseID);

			var payload = new JwtPayload(
				issuer: AuthUrl ?? "mockdomain.com",
				audience: ApiUrl ?? "mockdomain.com",
				claims: claims,
				expires: ExpiresUTC ?? DateTime.Now.AddMinutes(30),
				notBefore: NotValidBeforeUTC ?? DateTime.Now
			);

			var token = new JwtSecurityToken(header, payload);
			
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private void AddClaimIfNotNull(List<Claim> claims, string type, string value)
		{
			if (value != null) { claims.Add(new Claim(type, value)); }
		}

		private string GetHeader(JwtSecurityToken jwt, string key)
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
			var seconds = utc - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return seconds?.ToString();
		}
	}
}
