using Microsoft.IdentityModel.Tokens;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace OrderCloud.Catalyst.Auth.UserAuth
{
	public class FakeOrderCloudToken
	{
		/// <summary>
		/// Create a fake token for unit testing. (Grants no access to the api). 
		/// </summary>
		public static string Create(
			string clientID,
			List<string> roles = null,
			DateTime? expiresUTC = null,
			DateTime? notValidBeforeUTC = null,
			string username = null,
			string keyID = null,
			string anonOrderID = null,
			string authUrl = null,
			string apiUrl = null,
			CommerceRole userType = CommerceRole.Seller,
			string userDatabaseID = null,
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
			AddClaimIfNotNull(claims, "usrtype", DecodedToken.GetUserType(userType));
			AddClaimIfNotNull(claims, "cid", clientID);
			AddClaimIfNotNull(claims, "u", userDatabaseID);
			AddClaimIfNotNull(claims, "imp", impersonatingUserDatabaseID);

			var payload = new JwtPayload(
				issuer: authUrl ?? "mockdomain.com",
				audience: apiUrl ?? "mockdomain.com",
				claims: claims,
				expires: expiresUTC ?? DateTime.UtcNow.AddMinutes(30),
				notBefore: notValidBeforeUTC ?? DateTime.UtcNow
			);

			var token = new JwtSecurityToken(header, payload);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private static void AddClaimIfNotNull(List<Claim> claims, string type, string value)
		{
			if (value != null) { claims.Add(new Claim(type, value)); }
		}
	}
}
