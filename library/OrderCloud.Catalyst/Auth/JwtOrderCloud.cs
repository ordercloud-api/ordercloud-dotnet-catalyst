using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class JwtOrderCloud : JwtSecurityToken
	{
		public JwtOrderCloud(string token) : base(token) { }

		public string UserType => GetClaim("usrtype");
		public string AuthUrl => GetClaim("iss");
		public string ApiUrl => GetClaim("aud");
		public string ClientID => GetClaim("cid");
		public string CompanyID => GetClaim("coid");
		public string CompanyInteropID => GetClaim("coiid");
		public string SellerInteropID => GetClaim("siid");
		public string SellerID => GetClaim("sid");
		public string UserID => GetClaim("uid");
		public string Username => GetClaim("usr");
		public string AnonOrderID => GetClaim("orderid");
		public bool IsAnon => AnonOrderID != null;
		public DateTime ExpiresUTC => UnixToDateTime(GetClaim("exp"));
		public DateTime IssuedAtUTC => UnixToDateTime(GetClaim("iat"));
		public DateTime NotValidBeforeUTC => UnixToDateTime(GetClaim("nbf"));
		public List<string> Roles => Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();

		private string GetClaim(string key)
		{
			return Payload.FirstOrDefault(t => t.Key == key).Value?.ToString();
		}

		private static DateTime UnixToDateTime(string unix)
		{
			var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return dtDateTime.AddSeconds(int.Parse(unix)).ToLocalTime();
		}
	}
}
