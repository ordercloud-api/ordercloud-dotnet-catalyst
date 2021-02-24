using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
    public class VerifiedUserContext
    {
        private ClaimsPrincipal Principal { get; set; }
        public JwtOrderCloud ParsedToken { get; set; }
        public MeUser User { get; set; }
        public string RawToken => GetPrincipalClaim("accesstoken");

        public VerifiedUserContext() { }

		public VerifiedUserContext(ClaimsPrincipal principal)
		{
			Principal = principal;
			if (Principal.Claims.Any())
			{
                ParsedToken = new JwtOrderCloud(this.RawToken);
                var userJson = GetPrincipalClaim("userrecordjson");
                User = JsonConvert.DeserializeObject<MeUser>(userJson);
			}
		}

        private string GetPrincipalClaim(string key)
        {
            return Principal.Claims.FirstOrDefault(c => c.Type == key)?.Value;
        }
    }
}
