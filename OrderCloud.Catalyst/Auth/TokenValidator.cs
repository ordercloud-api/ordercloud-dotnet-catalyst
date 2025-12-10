using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
    /// <summary>
    /// Injectable service to aid with validating OrderCloud tokens
    /// </summary>
    public interface ITokenValidator
    {
        Task<DecodedToken> ValidateAccessTokenAsync(string token, OrderCloudUserAuthOptions options, IEnumerable<string> requiredRoles = null, IEnumerable<CommerceRole> allowedUserTypes = null);
        Task<DecodedUserInfoToken> ValidateUserInfoTokenAsync(string token, IEnumerable<string> requiredRoles = null);
    }

    public class TokenValidator : ITokenValidator
    {
        private readonly ISimpleCache _cache;
        private readonly IOrderCloudClient _oc;

        public TokenValidator(ISimpleCache cache, IOrderCloudClient oc)
        {
            _cache = cache;
            _oc = oc;
        }

        /// <summary>
        /// Validate the provided OrderCloud access token. Throws 401 if invalid or 403 if insufficient roles or wrong user types
        /// </summary>
        public async Task<DecodedToken> ValidateAccessTokenAsync(string token, OrderCloudUserAuthOptions options, IEnumerable<string> requiredRoles = null, IEnumerable<CommerceRole> allowedUserTypes = null)
        {
            Require.That(!string.IsNullOrEmpty(token), new UnAuthorizedException());

            var decodedToken = new DecodedToken(token);

            Require.That(decodedToken.ClientID != null, new UnAuthorizedException());

            Require.That(options.AnyClientIDCanAccess || options.ValidClientIDs.Contains(decodedToken.ClientID, StringComparer.InvariantCultureIgnoreCase), new UnAuthorizedException());

            Require.That(decodedToken.NotValidBeforeUTC < DateTime.UtcNow && decodedToken.ExpiresUTC > DateTime.UtcNow,
                new UnAuthorizedException());

            // we've validated the token as much as we can on this end, go make sure it's ok in OC	
            bool isValid;
            // some valid tokens - e.g. those from the portal - do not have a "kid"
            if (decodedToken.KeyID == null)
            {
                isValid = await ValidateAccessTokenWithMeGet(decodedToken); // also sets meUser field;
            }
            else
            {
                isValid = await ValidateTokenWithKeyID(decodedToken.ApiUrl, decodedToken.KeyID, decodedToken.AccessToken);
            }

            if (!isValid)
            {
                Require.That(decodedToken.ApiUrl == _oc?.Config?.ApiUrl,
                    new WrongEnvironmentException(new WrongEnvironmentError()
                    {
                        ExpectedEnvironment = _oc?.Config?.ApiUrl,
                        TokenIssuerEnvironment = decodedToken.ApiUrl
                    }
                ));
            }

            Require.That(isValid, new UnAuthorizedException());

            Require.That(allowedUserTypes.IsNullOrEmpty() || allowedUserTypes.Contains(decodedToken.CommerceRole),
                new InvalidUserTypeException(new InvalidUserTypeError()
                {
                    ThisUserType = decodedToken?.CommerceRole.ToString(),
                    UserTypesThatCanAccess = allowedUserTypes?.Select(x => x.ToString())?.ToList()
                })
            );

            Require.That(requiredRoles.IsNullOrEmpty() || requiredRoles.Any(role => decodedToken.Roles.Contains(role)),
                new InsufficientRolesException(new InsufficientRolesError()
                {
                    SufficientRoles = requiredRoles?.ToList(),
                    AssignedRoles = decodedToken.Roles.ToList()
                })
            );

            return decodedToken;
        }

        /// <summary>
        /// Validate the provided UserInfo access token. Throws 401 if invalid or 403 if insufficient roles.
        /// </summary>
        public async Task<DecodedUserInfoToken> ValidateUserInfoTokenAsync(string token, IEnumerable<string> requiredRoles = null)
        {
            Require.That(!string.IsNullOrEmpty(token), new UnAuthorizedException());

            var decodedToken = new DecodedUserInfoToken(token);

            Require.That(decodedToken.NotValidBeforeUTC < DateTime.UtcNow && decodedToken.ExpiresUTC > DateTime.UtcNow,
                new UnAuthorizedException());

            // we've validated the token as much as we can on this end, go make sure it's ok in OC	
            bool isValid = await ValidateTokenWithKeyID(decodedToken.ApiUrl, decodedToken.KeyID, decodedToken.UserInfoToken);

            if (!isValid)
            {
                Require.That(decodedToken.ApiUrl == _oc?.Config?.ApiUrl,
                    new WrongEnvironmentException(new WrongEnvironmentError()
                    {
                        ExpectedEnvironment = _oc?.Config?.ApiUrl,
                        TokenIssuerEnvironment = decodedToken.ApiUrl
                    }
                ));
            }

            Require.That(isValid, new UnAuthorizedException());

            Require.That(requiredRoles.IsNullOrEmpty() || requiredRoles.Any(role => decodedToken.Roles.Contains(role)),
                new InsufficientRolesException(new InsufficientRolesError()
                {
                    SufficientRoles = requiredRoles?.ToList(),
                    AssignedRoles = decodedToken.Roles.ToList()
                })
            );

            return decodedToken;
        }

        /// <summary>
        /// Verifiy the validity of an OrderCloud token, given details about the public key.
        /// </summary>
        private static async Task<bool> ValidateTokenWithPublicKey(string token, PublicKey publicKey)
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

            var result = await new JsonWebTokenHandler().ValidateTokenAsync(token, new TokenValidationParameters
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

        private async Task<bool> ValidateAccessTokenWithMeGet(DecodedToken jwt)
        {
            var cacheKey = jwt.AccessToken;

            return await _cache.GetOrAddAsync(cacheKey, TimeSpan.FromHours(1), async () =>
            {
                try
                {
                    var meUser = await _oc.Me.GetAsync(jwt.AccessToken);
                    return meUser != null && meUser.Active;
                }
                catch (OrderCloudException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    await _cache.RemoveAsync(cacheKey); // not their fault, don't make them wait 1 hr   
                    return false;
                }
            });
        }

        private async Task<bool> ValidateTokenWithKeyID(string apiUrl, string keyId, string token)
        {
            var cacheKey = $"{apiUrl}-{keyId}";
            var publicKey = await _cache.GetOrAddAsync(cacheKey, TimeSpan.FromDays(30), async () =>
            {
                try
                {
                    return await _oc.GetPublicKeyAsync(keyId);
                }
                catch (OrderCloudException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    await _cache.RemoveAsync(cacheKey); // not their fault, don't make them wait 5 min   
                    return null; // null public key will lead to unauthorized exception; 
                }
            });
            return await ValidateTokenWithPublicKey(token, publicKey);
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
