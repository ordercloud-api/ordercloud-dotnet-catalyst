
// FakeUserInfoToken.cs
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderCloud.Catalyst
{
    public class FakeUserInfoToken
    {
        /// <summary>
        /// Create a fake token for unit testing. (Grants no access to the API).
        /// If 'signingCredentials' is supplied, the token is signed with those credentials (e.g., RS256).
        /// Otherwise, falls back to HMAC SHA256 with a static symmetric key.
        /// </summary>
        public static string Create(
            List<string> roles = null,
            DateTime? expiresUTC = null,
            DateTime? notValidBeforeUTC = null,
            string username = null,
            string keyID = TestRsaKeyProvider.AllowedKid,
            string authUrl = null,
            string apiUrl = null,
            string companyID = null,
            List<string> groups = null,
            string marketplaceID = null,
            SigningCredentials signingCredentials = null
        )
        {
            var creds = signingCredentials ?? TestRsaKeyProvider.AllowedSigningCredentials;

            var header = new JwtHeader(creds);
            if (keyID != null)
            {
                header["kid"] = keyID;
            }

            var claims = new List<Claim>();

            foreach (var role in roles ?? new List<string>())
            {
                claims.Add(new Claim("availableroles", role));
            }

            foreach (var group in groups ?? new List<string>())
            {
                claims.Add(new Claim("groups", group));
            }

            AddClaimIfNotNull(claims, "sub", username);
            AddClaimIfNotNull(claims, "marketplaceID", marketplaceID);
            AddClaimIfNotNull(claims, "companyID", companyID);

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
