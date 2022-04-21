using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Util;
using OrderCloud.Catalyst.Payments.Stripe.Models;
using Stripe;

namespace OrderCloud.Catalyst.Payments.Stripe
{
    public class StripeClient
    {
        private StripeConfig _defaultConfig;

        public StripeClient(StripeConfig defaultConfig)
        {
            _defaultConfig = defaultConfig;
        }

        // https://stripe.com/docs/development/quickstart
        // https://stripe.com/docs/payments/payment-intents
        // https://stripe.com/docs/api/payment_intents

        public async Task<Customer> CreateCustomerAsync(CustomerCreateOptions options, StripeConfig optionalOverride = null)
        {
            StripeConfig config = optionalOverride ?? _defaultConfig;

            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new CustomerService();
            return await service.CreateAsync(options);
        }

        public async Task<PaymentMethod> CreatePaymentMethodAsync(PaymentMethodCreateOptions options, StripeConfig optionalOverride = null)
        {
            StripeConfig config = optionalOverride ?? _defaultConfig;

            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new PaymentMethodService();
            return await service.CreateAsync(options);
        }

        public async Task<PaymentIntent> CreatePaymentIntentAsync(PaymentIntentCreateOptions options, StripeConfig optionalOverride = null)
        {
            StripeConfig config = optionalOverride ?? _defaultConfig;

            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new PaymentIntentService();
            return await service.CreateAsync(options);
        }

        public async Task<PaymentIntent> ConfirmPaymentIntentAsync(string paymentMethodID, PaymentIntentConfirmOptions options, StripeConfig optionalOverride = null)
        {
            StripeConfig config = optionalOverride ?? _defaultConfig;

            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new PaymentIntentService();
            return await service.ConfirmAsync(paymentMethodID, options);
        }

        public async Task<PaymentIntent> CapturePaymentIntentAsync(string paymentIntentID, PaymentIntentCaptureOptions options, StripeConfig optionalOverride = null)
        {
            // instead of passing in paymentID here can we use stripeRequest.client_secret?
            StripeConfig config = optionalOverride ?? _defaultConfig;

            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new PaymentIntentService();
            return await service.CaptureAsync(paymentIntentID, options);
        }
    }
}
