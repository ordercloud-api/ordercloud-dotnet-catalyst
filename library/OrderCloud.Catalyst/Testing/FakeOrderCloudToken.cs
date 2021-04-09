using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Flurl.Http;
using Microsoft.IdentityModel.Tokens;

namespace OrderCloud.Catalyst
{
    public static class FakeOrderCloudToken
    {
	    public static string Create(string clientID, params string[] roles) {

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("blahblahblahblahblahblahblahblahblahblah"));
		    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var claims = roles.Select(r => new Claim("role", r)).ToList();
			claims.Add(new Claim("cid", clientID));

		    var token = new JwtSecurityToken(
			    issuer: "mydomain.com",
			    audience: "mydomain.com",
			    claims: claims,
			    expires: DateTime.Now.AddMinutes(30),
			    signingCredentials: creds);

		    return new JwtSecurityTokenHandler().WriteToken(token);
	    }

	    public static IFlurlClient WithFakeOrderCloudToken(this IFlurlClient fc, string clientId) {
			return fc.WithOAuthBearerToken(Create(clientId));
	    }
	}
}
