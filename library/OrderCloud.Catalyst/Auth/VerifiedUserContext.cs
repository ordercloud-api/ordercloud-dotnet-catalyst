using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
    public class VerifiedUserContext
    {
        private ClaimsPrincipal Principal { get; set; }
        private JwtOrderCloud ParsedToken { get; set; }
        private MeUser User { get; set; }

		public VerifiedUserContext(ClaimsPrincipal principal)
		{
			Principal = principal;
			if (Principal.Claims.Any())
			{
                ParsedToken = new JwtOrderCloud(this.AccessToken);
                var userJson = GetPrincipalClaim("userrecordjson");
                User = JsonConvert.DeserializeObject<MeUser>(userJson);
			}
		}

        private string GetPrincipalClaim(string key)
        {
			var claim = Principal.Claims.FirstOrDefault(c => c.Type == key)?.Value;
            if (claim == null) 
            { 
                throw new CatalystBaseException("MissingAuthClaim", $"Claim with name \"{key}\" is missing from user context"); 
            }
            return claim;
        }

        public string AccessToken => GetPrincipalClaim("accesstoken");
        public IReadOnlyList<string> AvailableRoles => User.AvailableRoles;
        public dynamic xp => User.xp;
        public bool Active => User.Active;
        public DateTimeOffset? TermsAccepted => User.TermsAccepted;
        public string Phone => User.Phone;
        public string Email => User.Email;
        public DateTimeOffset? DateCreated => User.DateCreated;
        public string LastName => User.LastName;
        public string Password => User.Password;
        public string Username => User.Username;
        public string ID => User.ID;
        public MeSeller Seller => User.Seller;
        public MeSupplier Supplier => User.Supplier;
        public MeBuyer Buyer => User.Buyer;
        public string FirstName => User.FirstName;
        public DateTimeOffset? PasswordLastSetDate => User.PasswordLastSetDate;
		public string UserType => ParsedToken.UserType;
        public string CompanyDatabaseID => ParsedToken.CompanyID;
        public string SellerDatabaseID => ParsedToken.SellerID;
        public string AnonOrderID => ParsedToken.AnonOrderID;
        public bool IsAnon => ParsedToken.IsAnon;   
        // From here down they are really features of the token, not the user.
        public string TokenAuthUrl => ParsedToken.AuthUrl;
        public string TokenApiUrl => ParsedToken.ApiUrl;
        public string TokenClientID => ParsedToken.ClientID;
        public DateTime TokenExpiresUTC => ParsedToken.ExpiresUTC;
        public DateTime TokenIssuedAtUTC => ParsedToken.IssuedAtUTC;
        public DateTime TokenNotValidBeforeUTC => ParsedToken.NotValidBeforeUTC;
    }
}
