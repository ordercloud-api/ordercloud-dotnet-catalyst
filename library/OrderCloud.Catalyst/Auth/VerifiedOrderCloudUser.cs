using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
    public class VerifiedOrderCloudUser
    {
        public ClaimsPrincipal Principal { get; set; }
        private JwtOrderCloud _token { get; set; }

        public VerifiedOrderCloudUser() { }

        public VerifiedOrderCloudUser(ClaimsPrincipal principal)
        {
            Principal = principal;
            if (Principal.Claims.Any())
                _token = new JwtOrderCloud(this.AccessToken);
        }

        public string UserType => _token.UserType;
        public string AuthUrl => _token.AuthUrl;
        public string ApiUrl => _token.ApiUrl;
        public DateTime AccessTokenExpiresUTC => _token.ExpiresUTC;
        public string UserID => GetPrincipalClaim("userid");
        public string Username => GetPrincipalClaim("username");
        public string ClientID => GetPrincipalClaim("clientid");
        public string Email => GetPrincipalClaim("email");
        public string SupplierID => GetPrincipalClaim("supplier");
        public string BuyerID => GetPrincipalClaim("buyer");
        public string SellerID => GetPrincipalClaim("seller");
        public string AccessToken => GetPrincipalClaim("accesstoken");

        private string GetPrincipalClaim(string key)
        {
            return Principal.Claims.FirstOrDefault(c => c.Type == key)?.Value;
        }
    }
}
