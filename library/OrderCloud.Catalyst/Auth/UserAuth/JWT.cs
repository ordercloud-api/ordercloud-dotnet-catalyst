using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OrderCloud.SDK;
using System;
using System.Security.Cryptography;

namespace OrderCloud.Catalyst
{
	public static class JWT
	{
		public static bool IsTokenCryptoValid(string token, PublicKey publicKey)
		{
			if (publicKey == null)
			{
				return false;
			}
			var rsa = new RSACryptoServiceProvider(2048);
			rsa.ImportParameters(new RSAParameters
			{
				Modulus = FromBase64Url(publicKey.n),
				Exponent = FromBase64Url(publicKey.e)
			});
			var rsaSecurityKey = new RsaSecurityKey(rsa);

			var result = new JsonWebTokenHandler().ValidateToken(token, new TokenValidationParameters
			{
				IssuerSigningKey = rsaSecurityKey,
				RequireSignedTokens = true,
				ValidateIssuerSigningKey = true,
				ValidateLifetime = true,
				LifetimeValidator = (nbf, exp, _, __) => nbf < DateTime.UtcNow && exp > DateTime.UtcNow,
				ValidateIssuer = false,
				RequireExpirationTime = true,
				ValidateAudience = false
			});
			return result.IsValid;
		}

		private static byte[] FromBase64Url(string base64Url)
		{
			string padded = base64Url.Length % 4 == 0
				? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
			string base64 = padded.Replace("_", "/")
								  .Replace("-", "+");
			return Convert.FromBase64String(base64);
		}
	}
}
