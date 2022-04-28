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

        //public async Task<Customer> CreateCustomerAsync(CustomerCreateOptions options, StripeConfig optionalOverride = null)
        //{
        //    StripeConfig config = optionalOverride ?? _defaultConfig;

        //    StripeConfiguration.ApiKey = config.SecretKey;

        //    var service = new CustomerService();
        //    return await service.CreateAsync(options);
        //}

        // this will be done via iFrame, not handled in the integration. The FE will pass a token that we can use
        //public async Task<PaymentMethod> CreatePaymentMethodAsync(PaymentMethodCreateOptions options, StripeConfig optionalOverride = null)
        //{
        //    StripeConfig config = optionalOverride ?? _defaultConfig;

        //    StripeConfiguration.ApiKey = config.SecretKey;

        //    var service = new PaymentMethodService();
        //    return await service.CreateAsync(options);
        //}

        // this will not be part of the common interface because the return type here does not match up with other integrations
        public static async Task<PaymentIntent> CreatePaymentIntentAsync(PaymentIntentCreateOptions options, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new PaymentIntentService();
            // this should return secret and transaction ID
            return await service.CreateAsync(options);
        }

        public static async Task<PaymentIntent> ConfirmPaymentIntentAsync(string paymentIntentID, PaymentIntentConfirmOptions options, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new PaymentIntentService();
            return await service.ConfirmAsync(paymentIntentID, options);
        }

        public static async Task<PaymentIntent> CapturePaymentIntentAsync(string paymentIntentID, PaymentIntentCaptureOptions options, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new PaymentIntentService();
            return await service.CaptureAsync(paymentIntentID, options);
        }

        public static async Task<Refund> CreateRefundAsync(RefundCreateOptions options, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new RefundService();
            return await service.CreateAsync(options);
        }

        public static async Task<PaymentIntent> CancelPaymentIntentAsync(string paymentIntentID, PaymentIntentCancelOptions options, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new PaymentIntentService();
            return await service.CancelAsync(paymentIntentID, options);
        }
    }
}
