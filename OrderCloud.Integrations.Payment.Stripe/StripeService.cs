using System.Collections.Generic;
using System.Threading.Tasks;
using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.Stripe.Mappers;

namespace OrderCloud.Integrations.Payment.Stripe
{
    public class StripeService : OCIntegrationService, ICreditCardProcessor, ICreditCardSaver
    {
        public StripeService(StripeConfig defaultConfig) : base(defaultConfig) { }

        #region ICreditCardProcessor

        public async Task<CCTransactionResult> AuthorizeOnlyAsync(AuthorizeCCTransaction transaction,
            OCIntegrationConfig configOverride = null)
        {
            {
                var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
                var paymentIntentMapper = new StripePaymentIntentMapper();
                var paymentIntentCreateOptions = paymentIntentMapper.MapPaymentIntentCreateAndConfirmOptions(transaction);
                var createdPaymentIntent = await StripeClient.CreateAndConfirmPaymentIntentAsync(paymentIntentCreateOptions, config);
                return paymentIntentMapper.MapPaymentIntentCreateAndConfirmResponse(createdPaymentIntent);
            }
        }

        public async Task<CCTransactionResult> CapturePriorAuthorizationAsync(FollowUpCCTransaction transaction, 
            OCIntegrationConfig configOverride = null)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            var paymentIntentMapper = new StripePaymentIntentMapper();
            var paymentIntentCaptureOptions = paymentIntentMapper.MapPaymentIntentCaptureOptions(transaction);
            var capturedPaymentIntent = await StripeClient.CapturePaymentIntentAsync(transaction.TransactionID, paymentIntentCaptureOptions, config);
            return paymentIntentMapper.MapPaymentIntentCaptureResponse(capturedPaymentIntent);
        }

        public async Task<CCTransactionResult> VoidAuthorizationAsync(FollowUpCCTransaction transaction,
            OCIntegrationConfig configOverride = null)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            var paymentIntentMapper = new StripePaymentIntentMapper();
            var cancelPaymentIntentOptions = paymentIntentMapper.MapPaymentIntentCancelOptions(transaction);
            var canceledPaymentIntent = await StripeClient.CancelPaymentIntentAsync(transaction.TransactionID, cancelPaymentIntentOptions, config);
            return paymentIntentMapper.MapPaymentIntentCancelResponse(canceledPaymentIntent);
        }

        public async Task<CCTransactionResult> RefundCaptureAsync(FollowUpCCTransaction transaction,
            OCIntegrationConfig configOverride = null)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            var refundMapper = new StripeRefundMapper();
            var refundCreateOptions = refundMapper.MapRefundCreateOptions(transaction);
            var refund = await StripeClient.CreateRefundAsync(refundCreateOptions, config);
            return refundMapper.MapRefundCreateResponse(refund);
        }
        #endregion

        #region ICreditCardSaver

        public async Task<CardCreatedResponse> CreateSavedCardAsync(PaymentSystemCustomer customer,
            PCISafeCardDetails card, OCIntegrationConfig configOverride = null)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            var paymentMethodMapper = new StripePaymentMethodMapper();

            if (!customer.CustomerAlreadyExists)
            {
                var stripeCustomerOptions = StripeCustomerCreateMapper.MapCustomerOptions(customer);
                var stripeCustomer = await StripeClient.CreateCustomerAsync(stripeCustomerOptions, config);
                customer.ID = stripeCustomer.Id;
            }

            var paymentMethodCreateOptions = paymentMethodMapper.MapPaymentMethodCreateOptions(customer.ID, card);
            var paymentMethod = await StripeClient.CreatePaymentMethodAsync(paymentMethodCreateOptions, config);
            var paymentMethodAttachOptions = paymentMethodMapper.MapPaymentMethodAttachOptions(customer.ID);
            paymentMethod = await StripeClient.AttachPaymentMethodToCustomerAsync(paymentMethod.Id, paymentMethodAttachOptions, config);
            return paymentMethodMapper.MapPaymentMethodCreateResponse(customer.ID, paymentMethod);
        }

        public async Task<List<PCISafeCardDetails>> ListSavedCardsAsync(string customerID,
            OCIntegrationConfig configOverride = null)
        {
            {
                var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
                var paymentMethodMapper = new StripePaymentMethodMapper();
                var listPaymentMethodsOptions = paymentMethodMapper.MapPaymentMethodListOptions(customerID);
                var paymentMethodList = await StripeClient.ListPaymentMethodsAsync(listPaymentMethodsOptions, config);
                return paymentMethodMapper.MapStripePaymentMethodListResponse(paymentMethodList);
            }
        }

        public async Task<PCISafeCardDetails> GetSavedCardAsync(string customerID, string paymentMethodID,
            OCIntegrationConfig configOverride = null)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            var cardMapper = new StripeCardMapper();
            var paymentMethod = await StripeClient.RetrievePaymentMethodAsync(paymentMethodID, config);
            return cardMapper.MapStripeCardGetResponse(paymentMethod);
        }

        public async Task DeleteSavedCardAsync(string customerID, string paymentMethodID, OCIntegrationConfig configOverride = null)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            await StripeClient.DetachPaymentMethodToCustomerAsync(paymentMethodID, config);
        }
        #endregion
    }
}
