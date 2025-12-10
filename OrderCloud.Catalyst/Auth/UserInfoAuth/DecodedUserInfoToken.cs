using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;


namespace OrderCloud.Catalyst
{
    /// <summary>
	/// Represents data inside a specific OrderCloud userinfo json web token 
	/// </summary>
	public class DecodedUserInfoToken
    {
        /// <summary>
        /// The raw jwt userinfo token 
        /// </summary>
        public string UserInfoToken { get; }

        /// <summary>
        /// The signing key ID of the token. "kid" claim. Null when Portal issued the token. 
        /// </summary>
        public string KeyID { get; }

        /// <summary>
		/// MarketplaceID on the token. "marketplaceID" claim. Always non-null.
		/// </summary>
		public string MarketplaceID { get; }

        /// <summary>
		/// Username on the token. "sub" claim. Always non-null.
		/// </summary>
		public string Username { get; }

        /// <summary>
		/// OrderCloud roles on the token. "availableroles" claim. Always non-null.
		/// </summary>
		public List<string> Roles { get; } = new List<string>();

        /// <summary>
		/// Groups on the token. "groups" claim. Always non-null.
		/// </summary>
		public List<string> Groups { get; } = new List<string>();

        /// <summary>
		/// CompanyID on the token. "companyID" claim. Always non-null.
		/// </summary>
		public string CompanyID { get; }

        /// <summary>
        /// The authentication Url on the token. "iss" claim. Always non-null.
        /// </summary>
        public string AuthUrl { get; }

        /// <summary>
        /// The api Url on the token. "aud" claim. Always non-null.
        /// </summary>
        public string ApiUrl { get; }

        /// <summary>
        /// The expiry time of the token. "exp" claim. Always non-null.
        /// </summary>
        public DateTime ExpiresUTC { get; }

        /// <summary>
        /// The time the token is not valid before. "nbf" claim. Always non-null.
        /// </summary>
        public DateTime NotValidBeforeUTC { get; }

        public DecodedUserInfoToken() { }

        /// <summary>
        /// Create a UserContext from a raw json web token.
        /// </summary>
        public DecodedUserInfoToken(string token)
        {
            var jwt = new JwtSecurityToken(token);
            var lookup = jwt.Claims.ToLookup(c => c.Type, c => c.Value);

            UserInfoToken = token;
            KeyID = GetHeader(jwt, "kid");

            MarketplaceID = lookup["marketplaceID"].FirstOrDefault();
            Username = lookup["sub"].FirstOrDefault();
            Roles = lookup["availableroles"].ToList();
            Groups = lookup["groups"].ToList();
            CompanyID = lookup["companyID"].FirstOrDefault();
            AuthUrl = lookup["iss"].FirstOrDefault();
            ApiUrl = lookup["aud"].FirstOrDefault();
            ExpiresUTC = int.Parse(lookup["exp"].FirstOrDefault() ?? throw new ArgumentNullException("Token must contain \"exp\" claim")).FromUnixEpoch();
            NotValidBeforeUTC = int.Parse(lookup["nbf"].FirstOrDefault() ?? throw new ArgumentNullException("Token must contain \"nbf\" claim")).FromUnixEpoch();
        }
        private static string GetHeader(JwtSecurityToken jwt, string key)
        {
            return jwt.Header.FirstOrDefault(t => t.Key == key).Value?.ToString();
        }
    }
}
