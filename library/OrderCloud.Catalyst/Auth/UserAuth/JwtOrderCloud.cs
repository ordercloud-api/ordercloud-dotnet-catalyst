using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst.Auth.UserAuth
{
	public class JwtOrderCloud : JwtSecurityToken
	{
		public string AccessToken { get; private set; }
		public string BuyerID => CommerceRole == CommerceRole.Buyer ? CompanyID : null;
		public string SupplierID => CommerceRole == CommerceRole.Supplier ? CompanyID : null;
		private string CompanyID => GetPayloadClaim("coiid");  // really, this is the interopID. See "coid" for database ID.
		public string SellerID => GetPayloadClaim("siid"); // really, this is the interopID. See "sid" for database ID.
		public string UserID => GetPayloadClaim("uid");
		public string Username => GetPayloadClaim("usr");
		public string AnonOrderID => GetPayloadClaim("orderid");
		public bool IsAnon => AnonOrderID != null;
		public IReadOnlyList<string> AvailableRoles => (IReadOnlyList<string>)this.Payload.FirstOrDefault(c => c.Key == "role").Value;
		public CommerceRole CommerceRole => GetCommerceRole(GetPayloadClaim("usrtype"));
		public string AuthUrl => GetPayloadClaim("iss");
		public string ApiUrl => GetPayloadClaim("aud");
		public string ClientID => GetPayloadClaim("cid");
		public DateTime ExpiresUTC => UnixToDateTime(GetPayloadClaim("exp"));
		public DateTime IssuedAtUTC => UnixToDateTime(GetPayloadClaim("iat"));
		public DateTime NotValidBeforeUTC => UnixToDateTime(GetPayloadClaim("nbf"));
		public string KeyID => GetHeader("kid");

		public JwtOrderCloud(string token) : base(token) 
		{

		}

		private string GetPayloadClaim(string key)
		{
			return Payload.FirstOrDefault(t => t.Key == key).Value?.ToString();
		}

		private string GetHeader(string key)
		{
			return Header.FirstOrDefault(t => t.Key == key).Value?.ToString();
		}

		private static DateTime UnixToDateTime(string unix)
		{
			var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return dtDateTime.AddSeconds(int.Parse(unix)).ToLocalTime();
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
