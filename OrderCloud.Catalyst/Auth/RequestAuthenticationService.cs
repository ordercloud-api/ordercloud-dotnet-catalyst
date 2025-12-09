using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OrderCloud.SDK;
using System.IO;
using System.Text;

namespace OrderCloud.Catalyst
{

    /// <summary>
    /// Injectable service to aid with getting, decoding, and verifying OrderCloud auth tokens on an HttpRequest. 
    /// </summary>
    public interface IRequestAuthenticationService
    {
        IOrderCloudClient BuildClient();
        DecodedToken GetDecodedToken();
        DecodedToken GetDecodedToken(HttpRequest request);
        DecodedUserInfoToken GetDecodedUserInfoToken();
        DecodedUserInfoToken GetDecodedUserInfoToken(HttpRequest request);
        string GetToken();
        string GetToken(HttpRequest request);
        Task<MeUser> GetUserAsync();
        Task<T> GetUserAsync<T>() where T : MeUser;
        string GetWebhookHash();
        string GetWebhookHash(HttpRequest request);
        Task<DecodedToken> VerifyTokenAsync(HttpRequest request, OrderCloudUserAuthOptions options, IEnumerable<string> requiredRoles = null, IEnumerable<CommerceRole> allowedUserTypes = null);
        Task<DecodedToken> VerifyTokenAsync(OrderCloudUserAuthOptions options, IEnumerable<string> requiredRoles = null, IEnumerable<CommerceRole> allowedUserTypes = null);
        Task<DecodedToken> VerifyTokenAsync(string token, OrderCloudUserAuthOptions options, IEnumerable<string> requiredRoles = null, IEnumerable<CommerceRole> allowedUserTypes = null);
        Task<DecodedUserInfoToken> VerifyUserInfoTokenAsync(IEnumerable<string> requiredRoles = null);
        Task<DecodedUserInfoToken> VerifyUserInfoTokenAsync(HttpRequest request, IEnumerable<string> requiredRoles = null);
        Task<DecodedUserInfoToken> VerifyUserInfoTokenAsync(string token, IEnumerable<string> requiredRoles = null);
        Task<bool> VerifyWebhookHashAsync(HttpRequest request, OrderCloudWebhookAuthOptions options);
        Task<bool> VerifyWebhookHashAsync(OrderCloudWebhookAuthOptions options);
        Task<bool> VerifyWebhookHashAsync(string requestHash, HttpRequest request, OrderCloudWebhookAuthOptions options);
        bool VerifyWebhookHashAsync(string requestHash, string requestBody, OrderCloudWebhookAuthOptions options);
    }

    public class RequestAuthenticationService : IRequestAuthenticationService
    {
        private readonly IOrderCloudClient _oc;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenValidator _tokenValidator;

        public RequestAuthenticationService(IOrderCloudClient oc, IHttpContextAccessor httpContextAccessor, ITokenValidator tokenValidator)
        {
            _oc = oc;
            _httpContextAccessor = httpContextAccessor;
            _tokenValidator = tokenValidator;
        }

        /// <summary>
        /// Get a raw OrderCloud token from the provided request headers
        /// </summary>
        public string GetToken(HttpRequest request)
        {
            if (!request.Headers.TryGetValue("Authorization", out var header))
                return null;

            var parts = header.FirstOrDefault()?.Split(new[] { ' ' }, 2);
            if (parts?.Length != 2)
                return null;

            if (parts[0] != "Bearer")
                return null;

            var accessToken = parts[1].Trim();
            Require.That(!string.IsNullOrEmpty(accessToken), new UnAuthorizedException());

            return accessToken;
        }

        /// <summary>
        /// Get the header "X-oc-hash" of the provided request. Used to verify the request originated from OrderCloud.
        /// </summary>
        public string GetWebhookHash(HttpRequest request)
        {
            var sentHash = request.Headers?["X-oc-hash"].FirstOrDefault();
            Require.That(!string.IsNullOrEmpty(sentHash), new WebhookUnauthorizedException());
            return sentHash;
        }

        /// <summary>
        /// Get the header "X-oc-hash" of the current HttpContext Request. Used to verify the request originated from OrderCloud.
        /// </summary>
        public string GetWebhookHash()
        {
            return GetWebhookHash(_httpContextAccessor.HttpContext.Request);
        }

        /// <summary>
        /// Get a raw OrderCloud token from the current HttpContext request headers
        /// </summary>
        public string GetToken()
        {
            return GetToken(_httpContextAccessor.HttpContext.Request);
        }

        /// <summary>
        /// Get a strongly typed model of the OrderCloud token from the provided request headers
        /// </summary>
        public DecodedToken GetDecodedToken(HttpRequest request)
        {
            var token = GetToken(request);
            return new DecodedToken(token);
        }

        /// <summary>
        /// Get a strongly typed model of the OrderCloud token from the current HttpContext request headers
        /// </summary>
        public DecodedToken GetDecodedToken()
        {
            return GetDecodedToken(_httpContextAccessor.HttpContext.Request);
        }

        /// <summary>
        /// Get a strongly typed model of the OrderCloud token from the provided request headers
        /// </summary>
        public DecodedUserInfoToken GetDecodedUserInfoToken(HttpRequest request)
        {
            var token = GetToken(request);
            return new DecodedUserInfoToken(token);
        }

        /// <summary>
        /// Get a strongly typed model of the OrderCloud token from the current HttpContext request headers
        /// </summary>
        public DecodedUserInfoToken GetDecodedUserInfoToken()
        {
            return GetDecodedUserInfoToken(_httpContextAccessor.HttpContext.Request);
        }

        /// <summary>
        /// Verify the provided OrderCloud access token. Throws 401 if invalid or 403 if insufficient roles.
        /// </summary>
        public async Task<DecodedToken> VerifyTokenAsync(string token, OrderCloudUserAuthOptions options, IEnumerable<string> requiredRoles = null, IEnumerable<CommerceRole> allowedUserTypes = null)
        {
            return await _tokenValidator.ValidateAccessTokenAsync(token, options, requiredRoles, allowedUserTypes);
        }

        /// <summary>
        /// Verify the provided HttpRequest's OrderCloud access Token. Throws 401 if invalid or 403 if insufficient roles.
        /// </summary>
        public async Task<DecodedToken> VerifyTokenAsync(HttpRequest request, OrderCloudUserAuthOptions options, IEnumerable<string> requiredRoles = null, IEnumerable<CommerceRole> allowedUserTypes = null)
        {
            var token = GetToken(request);
            return await VerifyTokenAsync(token, options, requiredRoles, allowedUserTypes);
        }


        /// <summary>
        /// Verify the current HttpContext request's OrderCloud access token. Throws 401 if invalid or 403 if insufficient roles.
        /// </summary>
        public async Task<DecodedToken> VerifyTokenAsync(OrderCloudUserAuthOptions options, IEnumerable<string> requiredRoles = null, IEnumerable<CommerceRole> allowedUserTypes = null)
        {
            return await VerifyTokenAsync(_httpContextAccessor.HttpContext.Request, options, requiredRoles, allowedUserTypes);
        }

        /// <summary>
        /// Verify the provided OrderCloud userinfo token. Throws 401 if invalid or 403 if insufficient roles.
        /// </summary>
        public async Task<DecodedUserInfoToken> VerifyUserInfoTokenAsync(string token, IEnumerable<string> requiredRoles = null)
        {
            return await _tokenValidator.ValidateUserInfoTokenAsync(token, requiredRoles);
        }

        /// <summary>
        /// Verify the provided HttpRequest's OrderCloud userinfo Token. Throws 401 if invalid or 403 if insufficient roles.
        /// </summary>
        public async Task<DecodedUserInfoToken> VerifyUserInfoTokenAsync(HttpRequest request, IEnumerable<string> requiredRoles = null)
        {
            var token = GetToken(request);
            return await VerifyUserInfoTokenAsync(token, requiredRoles);
        }


        /// <summary>
        /// Verify the current HttpContext request's OrderCloud userinfo token. Throws 401 if invalid or 403 if insufficient roles.
        /// </summary>
        public async Task<DecodedUserInfoToken> VerifyUserInfoTokenAsync(IEnumerable<string> requiredRoles = null)
        {
            return await VerifyUserInfoTokenAsync(_httpContextAccessor.HttpContext.Request, requiredRoles);
        }

        /// <summary>
        /// Verify the provided webhook hash. Proves the request originated from OrderCloud.
        /// </summary>
        public bool VerifyWebhookHashAsync(string requestHash, string requestBody, OrderCloudWebhookAuthOptions options)
        {
            Require.That(!string.IsNullOrEmpty(options.HashKey),
                new InvalidOperationException("OrderCloudWebhookAuthOptions.HashKey was not configured."));

            Require.That(!string.IsNullOrEmpty(requestBody), new WebhookUnauthorizedException());
            Require.That(!string.IsNullOrEmpty(requestHash), new WebhookUnauthorizedException());

            var bodyBytes = Encoding.UTF8.GetBytes(requestBody);
            var keyBytes = Encoding.UTF8.GetBytes(options.HashKey);
            var hash = new HMACSHA256(keyBytes).ComputeHash(bodyBytes);
            var computed = Convert.ToBase64String(hash);

            Require.That(requestHash == computed, new WebhookUnauthorizedException());
            return true;
        }


        /// <summary>
        /// Verify the provided webhook hash. Proves the request originated from OrderCloud.
        /// </summary>
        public async Task<bool> VerifyWebhookHashAsync(string requestHash, HttpRequest request, OrderCloudWebhookAuthOptions options)
        {
            var requestBody = await GetHttpRequestBody(request);
            return VerifyWebhookHashAsync(requestHash, requestBody, options);
        }

        /// <summary>
        /// Verify the provided HttpContext request's webhook hash. Proves the request came from OrderCloud.
        /// </summary>
        public async Task<bool> VerifyWebhookHashAsync(HttpRequest request, OrderCloudWebhookAuthOptions options)
        {
            var requestHash = GetWebhookHash(request);
            return await VerifyWebhookHashAsync(requestHash, request, options);
        }

        /// <summary>
        /// Verify the current HttpContext request's webhook hash. Proves the request came from OrderCloud.
        /// </summary>
        public async Task<bool> VerifyWebhookHashAsync(OrderCloudWebhookAuthOptions options)
        {
            return await VerifyWebhookHashAsync(_httpContextAccessor.HttpContext.Request, options);

        }

        /// <summary>
        /// Get the full details of the currently authenticated user based on the HttpContext request token
        /// </summary>
        public async Task<T> GetUserAsync<T>()
            where T : MeUser
        {
            var token = GetToken();
            return await _oc.Me.GetAsync<T>(token);
        }


        /// <summary>
        /// Get the full details of the currently authenticated user based on the HttpContext request token
        /// </summary>
        public async Task<MeUser> GetUserAsync()
        {
            var token = GetToken();
            return await _oc.Me.GetAsync(token);
        }

        /// <summary>
        /// Get an IOrderCloudClient with token set based on the HttpContext request
        /// </summary>
        public IOrderCloudClient BuildClient()
        {
            return GetDecodedToken().BuildClient();
        }

        /// <summary>
        /// This still won't work inside a controller unless there's middleware to run request.EnableBuffering();
        /// See https://stackoverflow.com/questions/59185410/request-body-from-is-empty-in-net-core-3-0
        /// </summary>
        private async Task<string> GetHttpRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            request.Body.Position = 0;
            try
            {
                return await new StreamReader(request.Body).ReadToEndAsync();
            }
            finally
            {
                request.Body.Position = 0;
            }
        }
    }
}
