using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Util;
using OrderCloud.Catalyst.Payments.Stripe.Models;

namespace OrderCloud.Catalyst.Payments.Stripe
{
    // https://stripe.com/docs/api/authentication?lang=dotnet
    public class StripeClient
    {
        private StripeConfig _defaultConfig;

        public StripeClient(StripeConfig defaultConfig)
        {
            _defaultConfig = defaultConfig;
        }

        // create customer

        // create payment intent
        // confirm payment intent
        // capture payment intent
        // accept payment?
        // https://stripe.com/docs/payments/payment-intents
        // https://stripe.com/docs/api/payment_intents
        public async Task<StripePaymentIntentResponse> CreatePaymentIntentAsync(StripePaymentIntentRequest stripeRequest, StripeConfig optionalOverride = null)
        {
            StripeConfig config = optionalOverride ?? _defaultConfig;

            var flurlRequest = config.BaseUrl
                .AppendPathSegments("v1", "payment_intents")
                .WithOAuthBearerToken(config.SecretKey);

            return await MakeStripeRequest(flurlRequest, stripeRequest, config);
        }

        public async Task<StripePaymentIntentResponse> ConfirmPaymentIntentAsync(string paymentID, StripePaymentIntentRequest stripeRequest, StripeConfig optionalOverride = null)
        {
            StripeConfig config = optionalOverride ?? _defaultConfig;

            var flurlRequest = config.BaseUrl
                .AppendPathSegments("v1", "payment_intents", paymentID, "confirm")
                .WithOAuthBearerToken(config.SecretKey);

            return await MakeStripeRequest(flurlRequest, stripeRequest, config);
        }

        //public async Task<StripePaymentIntentResponse> CapturePaymentIntentAsync(string paymentID, StripePaymentIntentRequest stripeRequest, StripeConfig optionalOverride = null)
        //{
        //    // instead of passing in paymentID here can we use stripeRequest.client_secret?
        //    StripeConfig config = optionalOverride ?? _defaultConfig;

        //    var flurlRequest = config.BaseUrl
        //        .AppendPathSegments("v1", "payment_intents", paymentID)
        //        .WithOAuthBearerToken(config.SecretKey);

        //    return await MakeStripeRequest(flurlRequest, stripeRequest, config);
        //}

        internal async Task<StripePaymentIntentResponse> MakeStripeRequest(IFlurlRequest flurlReq, StripePaymentIntentRequest stripeReq, StripeConfig config)
        {
            var requestBody = new List<KeyValuePair<string, string>>();
            var keyValuePairs = stripeReq.ToKeyValuePairs();
            foreach (var pair in keyValuePairs.Select((value, index) => new { index, value }))
            {
                if (pair.value.Key != "payment_method_types")
                    // what if value is an object?
                    requestBody.Add(new KeyValuePair<string, string>(pair.value.Key, pair.value.Value?.ToString()));
            }

            if (stripeReq.payment_method_types != null && stripeReq.payment_method_types.Any())
            {
                foreach (var method in stripeReq.payment_method_types.Select((value, index) => new { index, value }))
                {
                    //https://stackoverflow.com/questions/67785824/list-of-strings-converts-to-single-string-in-post-rest-api-dart-flutter
                    requestBody.Add(new KeyValuePair<string, string>($"payment_method_types[{method.index}]", method.value));
                }
            }

            var formattedRequestBody = (object)requestBody;

            try
            {
                return await flurlReq.PostUrlEncodedAsync(formattedRequestBody)
                    .ReceiveJson<StripePaymentIntentResponse>();
            }
            catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
            {
                // candidate for retry here?
                throw new IntegrationNoResponseException(config, flurlReq.Url);
            }
            catch (FlurlHttpException ex)
            {
                var status = ex?.Call?.Response?.StatusCode;
                if (status == null) // simulate by putting laptop on airplane mode
                {
                    throw new IntegrationNoResponseException(config, flurlReq.Url);
                }
                if (status == 401 || status == 403)
                {
                    throw new IntegrationAuthFailedException(config, flurlReq.Url, (int)status);
                }
                var body = await ex.Call.Response.GetJsonAsync();
                // add return type to GetJsonAsync();
                throw new IntegrationErrorResponseException(config, flurlReq.Url, (int)status, body);
            }
        }
    }
}
