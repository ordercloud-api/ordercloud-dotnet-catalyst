using System.Threading.Tasks;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe
{
    public class StripeClient
    {
        private readonly StripeConfig _defaultConfig;

        public StripeClient(StripeConfig defaultConfig)
        {
            _defaultConfig = defaultConfig;
        }

        /// <summary>
        /// https://stripe.com/docs/api/payment_methods/create
        /// </summary>
        public static async Task<PaymentMethod> CreatePaymentMethodAsync(PaymentMethodCreateOptions options, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;
            var service = new PaymentMethodService();
            return await service.CreateAsync(options);
        }

        /// <summary>
        /// https://stripe.com/docs/api/payment_methods/retrieve
        /// </summary>
        public static async Task<PaymentMethod> RetrievePaymentMethodAsync(string paymentMethodID, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;
            var service = new PaymentMethodService();
            return await service.GetAsync(paymentMethodID);
        }

        /// <summary>
        /// https://stripe.com/docs/api/payment_methods/attach
        /// </summary>
        public static async Task<PaymentMethod> AttachPaymentMethodToCustomerAsync(string paymentMethodID, PaymentMethodAttachOptions attachOptions,
            StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;
            var service = new PaymentMethodService();
            return await service.AttachAsync(paymentMethodID, attachOptions);
        }

        /// <summary>
        /// https://stripe.com/docs/api/payment_methods/detach
        /// </summary>
        public static async Task<PaymentMethod> DetachPaymentMethodToCustomerAsync(string paymentMethodID,
            StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;
            var service = new PaymentMethodService();
            return await service.DetachAsync(paymentMethodID);
        }

        /// <summary>
        /// https://stripe.com/docs/api/payment_methods/list
        /// </summary>
        public static async Task<StripeList<PaymentMethod>> ListPaymentMethodsAsync(
            PaymentMethodListOptions listOptions, StripeConfig config)
        {;
            StripeConfiguration.ApiKey = config.SecretKey;
            var service = new PaymentMethodService();
            return await service.ListAsync(listOptions);
        }

        /// <summary>
        /// https://stripe.com/docs/api/customers/create
        /// </summary>
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
        /// https://stripe.com/docs/api/payment_intents/cancel
        /// </summary>
        public static async Task<PaymentIntent> CancelPaymentIntentAsync(string paymentIntentID, PaymentIntentCancelOptions options, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;
            var service = new PaymentIntentService();
            return await service.CancelAsync(paymentIntentID, options);
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
        /// NOT USED IN ICreditCardSaver OR ICreditCardProcessor
        public async Task<StripeList<Card>> ListCreditCardsAsync(string customerID, StripeConfig config)
        {
            StripeConfiguration.ApiKey = config.SecretKey;
            var service = new CardService();
            return await service.ListAsync(customerID);
        }
    }
}
