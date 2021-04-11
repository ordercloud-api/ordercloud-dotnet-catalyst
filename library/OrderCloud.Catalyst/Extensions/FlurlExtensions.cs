using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Flurl.Http;
using Microsoft.IdentityModel.Tokens;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
    public static class FlurlExtensions
    {
		public static IFlurlClient WithFakeOrderCloudToken(this IFlurlClient fc, string clientID, List<string> roles = null)
		{
			roles ??= new List<string>();
			var token = new JwtOrderCloud() { ClientID = clientID, Roles = roles };
			return fc.WithFakeOrderCloudToken(token);
		}

		public static IFlurlClient WithFakeOrderCloudToken(this IFlurlClient fc, JwtOrderCloud token) {
			return fc.WithOAuthBearerToken(token.CreateFake());
	    }
	}
}
