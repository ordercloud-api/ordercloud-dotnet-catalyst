using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Util;
using Microsoft.Extensions.Options;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe
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

        // this will be done via iFrame, not handled in the integration. The FE will pass a token that we can use
        public async Task<PaymentMethod> CreatePaymentMethodAsync(PaymentMethodCreateOptions options, StripeConfig optionalOverride = null)
        {
            StripeConfig config = optionalOverride ?? _defaultConfig;

            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new PaymentMethodService();
            return await service.CreateAsync(options);
        }

        public static async Task<Customer> CreateCustomerAsync(CustomerCreateOptions options, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;
            var service = new CustomerService();
            return await service.CreateAsync(options);
        }

        /// <summary>
        /// https://stripe.com/docs/api/payment_intents/create
        /// </summary>
        public static async Task<PaymentIntent> CreateAndConfirmPaymentIntentAsync(PaymentIntentCreateOptions options, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;
            // should we move this to the mapper method?
            if (options.PaymentMethod == null)
            {
                throw new StripeException("Payment Method required");
            }
            
            var service = new PaymentIntentService();
            return await service.CreateAsync(options);
        }

        /// <summary>
        /// https://stripe.com/docs/api/payment_intents/capture
        /// </summary>
        public static async Task<PaymentIntent> CapturePaymentIntentAsync(string paymentIntentID, PaymentIntentCaptureOptions options, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new PaymentIntentService();
            return await service.CaptureAsync(paymentIntentID, options);
        }

        /// <summary>
        /// https://stripe.com/docs/api/refunds/create
        /// </summary>
        public static async Task<Refund> CreateRefundAsync(RefundCreateOptions options, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new RefundService();
            return await service.CreateAsync(options);
        }

        /// <summary>
        /// https://stripe.com/docs/api/payment_intents/cancel
        /// </summary>
        public static async Task<PaymentIntent> CancelPaymentIntentAsync(string paymentIntentID, PaymentIntentCancelOptions options, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;

            var service = new PaymentIntentService();
            return await service.CancelAsync(paymentIntentID, options);
        }

        /// <summary>
        /// https://stripe.com/docs/api/cards/create
        /// </summary>
        public static async Task<Card> CreateCardAsync(string customerID, CardCreateOptions options, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;
            var service = new CardService();
            return await service.CreateAsync(customerID, options);
        }

        /// <summary>
        /// https://stripe.com/docs/api/cards/list
        /// </summary>
        public static async Task<StripeList<Card>> ListCreditCardsAsync(string customerID, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;
            var service = new CardService();
            return await service.ListAsync(customerID);
        }

        /// <summary>
        /// https://stripe.com/docs/api/cards/retrieve
        /// </summary>
        public static async Task<Card> GetCreditCardAsync(string customerID, string creditCardID, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;
            var service = new CardService();
            return await service.GetAsync(customerID, creditCardID);
        }
    }
}
