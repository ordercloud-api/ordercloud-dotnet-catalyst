using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
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
        // https://www.youtube.com/watch?v=5SzRdPuZVR0
        // https://stripe.com/docs/payments/quickstart
        public async Task<StripePaymentIntentResponse> CreatePaymentIntentAsync(StripePaymentIntentRequest req, StripeConfig optionalOverride = null)
        {
            StripeConfig config = optionalOverride ?? _defaultConfig;

            var request = config.BaseUrl
                .AppendPathSegments("v1", "payment_intents")
                .WithOAuthBearerToken(config.SecretKey);

            try
            {
                return await request.PostUrlEncodedAsync(req)
                    .ReceiveJson<StripePaymentIntentResponse>();
                    // add return type to ReceiveJson()
                    // make implicit return
            }
            catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
            {
                // candidate for retry here?
                throw new IntegrationNoResponseException(config, request.Url);
            }
            catch (FlurlHttpException ex)
            {
                var status = ex?.Call?.Response?.StatusCode;
                if (status == null) // simulate by putting laptop on airplane mode
                {
                    throw new IntegrationNoResponseException(config, request.Url);
                }
                if (status == 401 || status == 403)
                {
                    throw new IntegrationAuthFailedException(config, request.Url, (int)status);
                }
                var body = await ex.Call.Response.GetJsonAsync();
                // add return type to GetJsonAsync();
                throw new IntegrationErrorResponseException(config, request.Url, (int)status, body);
            }
        }

        public async Task ConfirmPaymentIntentAsync(string paymentID, StripeConfig optionalOverride = null)
        {
            StripeConfig config = optionalOverride ?? _defaultConfig;

            var request = config.BaseUrl
                .AppendPathSegments("v1", "payment_intents", paymentID, "confirm")
                .WithOAuthBearerToken(config.SecretKey);

            try
            {
                var response = await request.PostUrlEncodedAsync(request)
                    .ReceiveJson();
                // add return type to ReceiveJson()
                // make implicit return
            }
            catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
            {
                // candidate for retry here?
                throw new IntegrationNoResponseException(config, request.Url);
            }
            catch (FlurlHttpException ex)
            {
                var status = ex?.Call?.Response?.StatusCode;
                if (status == null) // simulate by putting laptop on airplane mode
                {
                    throw new IntegrationNoResponseException(config, request.Url);
                }
                if (status == 401 || status == 403)
                {
                    throw new IntegrationAuthFailedException(config, request.Url, (int)status);
                }
                var body = await ex.Call.Response.GetJsonAsync();
                // add return type to GetJsonAsync();
                throw new IntegrationErrorResponseException(config, request.Url, (int)status, body);
            }
        }

        public async Task CapturePaymentIntentAsync(string paymentID, StripeConfig optionalOverride = null)
        {
            StripeConfig config = optionalOverride ?? _defaultConfig;

            var request = config.BaseUrl
                .AppendPathSegments("v1", "payment_intents", paymentID)
                .WithOAuthBearerToken(config.SecretKey);

            try
            {
                // request can have an optional field named amount_to_capture
                var response = await request.PostUrlEncodedAsync(request)
                    .ReceiveJson();
                // add return type to ReceiveJson()
                // make implicit return
            }
            catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
            {
                // candidate for retry here?
                throw new IntegrationNoResponseException(config, request.Url);
            }
        }
    }
}
