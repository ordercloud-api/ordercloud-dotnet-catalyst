using System;
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
            var cardMapper = new StripeCardMapper();
            var cardCreateOptions = cardMapper.MapStripeCardCreateOptions(card);

            if (customer.CustomerAlreadyExists)
            {
                // do we need to do anything here?
                // https://stripe.com/docs/api/payment_methods/attach
            }
            else
            {
                var stripeCustomerOptions = StripeCustomerCreateMapper.MapCustomerOptions(customer);
                var stripeCustomer = await StripeClient.CreateCustomerAsync(stripeCustomerOptions, config);
                customer.ID = stripeCustomer.Id;
            }
            var createdCard = await StripeClient.CreateCardAsync(customer.ID, cardCreateOptions, config);

            return cardMapper.MapStripeCardCreateResponse(customer.ID, createdCard);
        }

        public async Task<List<PCISafeCardDetails>> ListSavedCardsAsync(string customerID,
            OCIntegrationConfig configOverride = null)
        {
            {
                var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
                var cardMapper = new StripeCardMapper();
                var cardList = await StripeClient.ListCreditCardsAsync(customerID, config);
                return cardMapper.MapStripeCardListResponse(cardList);
            }
        }

        public async Task<PCISafeCardDetails> GetSavedCardAsync(string customerID, string cardID,
            OCIntegrationConfig configOverride = null)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            var cardMapper = new StripeCardMapper();
            var card = await StripeClient.GetCreditCardAsync(customerID, cardID, config);
            return cardMapper.MapStripeCardGetResponse(card);
        }

        public Task DeleteSavedCardAsync(string customerID, string cardID, OCIntegrationConfig configOverride = null)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
