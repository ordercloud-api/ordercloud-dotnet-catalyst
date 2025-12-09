
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
    /// <summary>
    /// Provides two RSA keypairs:
    /// - Allowed: used to sign valid tokens and returned for AllowedKid.
    /// - Denied: returned for all other kids (won't validate the allowed token).
    /// </summary>
    public static class TestRsaKeyProvider
    {
        public const string AllowedKid = "kid-allow";
        public const string DeniedKid = "kid-block";

        private static readonly RSA _allowedRsa = CreateRsa(2048);
        private static readonly RSA _deniedRsa = CreateRsa(2048);

        public static RSA AllowedRsa => _allowedRsa;
        public static RSA DeniedRsa => _deniedRsa;

        public static SigningCredentials AllowedSigningCredentials =>
            new SigningCredentials(new RsaSecurityKey(_allowedRsa) { KeyId = AllowedKid }, SecurityAlgorithms.RsaSha256);

        public static (string n, string e) ExportBase64UrlPublicParts(RSA rsa)
        {
            var p = rsa.ExportParameters(false);
            var n = Base64UrlEncoder.Encode(p.Modulus);
            var e = Base64UrlEncoder.Encode(p.Exponent);
            return (n, e);
        }

        public static PublicKey ToOrderCloudPublicKey(RSA rsa)
        {
            var (n, e) = ExportBase64UrlPublicParts(rsa);
            return new PublicKey { n = n, e = e };
        }

        private static RSA CreateRsa(int keySize)
        {
            var rsa = RSA.Create();
            rsa.KeySize = keySize;
            return rsa;
        }
    }
}
