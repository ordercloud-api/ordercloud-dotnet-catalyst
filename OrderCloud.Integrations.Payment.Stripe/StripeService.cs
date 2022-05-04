using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.Stripe.Mappers;

namespace OrderCloud.Integrations.Payment.Stripe
{
    public class StripeService : OCIntegrationService, ICreditCardProcessor, ICreditCardSaver
    {
        public StripeService(StripeConfig defaultConfig) : base(defaultConfig) { }

        #region ICreditCardProcessor

        public async Task<CCTransactionResult> AuthorizeOnlyAsync(AuthorizeCCTransaction transaction, OCIntegrationConfig configOverride = null) =>
            await CreateAndConfirmPaymentIntentAsync(transaction, configOverride);

        public async Task<CCTransactionResult> CapturePriorAuthorizationAsync(FollowUpCCTransaction transaction, OCIntegrationConfig configOverride = null) =>
            await CapturePaymentIntentAsync(transaction, configOverride);

        public async Task<CCTransactionResult> VoidAuthorizationAsync(FollowUpCCTransaction transaction, OCIntegrationConfig configOverride = null) => 
            await CancelPaymentIntentAsync(transaction, configOverride);

        public async Task<CCTransactionResult> RefundCaptureAsync(FollowUpCCTransaction transaction, OCIntegrationConfig configOverride = null) =>
            await CreateRefundAsync(transaction, configOverride);
        #endregion

        #region ICreditCardSaver

        public async Task<PCISafeCardDetails> CreateSavedCardAsync(PaymentSystemCustomer customer, PCISafeCardDetails card, OCIntegrationConfig configOverride = null) =>
            await CreateSavedCreditCardAsync(customer, card, configOverride);
        
        public Task<List<PCISafeCardDetails>> ListSavedCardsAsync(string customerID, OCIntegrationConfig configOverride = null)
        {
            throw new NotImplementedException();
        }

        public Task<PCISafeCardDetails> GetSavedCardAsync(string customerID, string cardID, OCIntegrationConfig configOverride = null)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSavedCardAsync(string customerID, string cardID, OCIntegrationConfig configOverride = null)
        {
            throw new NotImplementedException();
        }
        #endregion
        
        public async Task<CCTransactionResult> CreateAndConfirmPaymentIntentAsync(AuthorizeCCTransaction transaction, OCIntegrationConfig configOverride)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            var paymentIntentCreateOptions = new StripePaymentIntentCreateMapper().MapPaymentIntentCreateAndConfirmOptions(transaction);
            var createdPaymentIntent = await StripeClient.CreateAndConfirmPaymentIntentAsync(paymentIntentCreateOptions, config);
            return new CCTransactionResult()
            {
                Message = createdPaymentIntent.Status,
                Succeeded = createdPaymentIntent.Status.ToLower() == "requires_capture",
                TransactionID = createdPaymentIntent.Id,
                Amount = createdPaymentIntent.Amount
            };
        }

        public async Task<CCTransactionResult> CapturePaymentIntentAsync(FollowUpCCTransaction transaction, OCIntegrationConfig configOverride)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            var paymentIntentCaptureOptions = new StripePaymentIntentCaptureMapper().MapPaymentIntentCaptureOptions(transaction);
            var capturedPaymentIntent = await StripeClient.CapturePaymentIntentAsync(transaction.TransactionID, paymentIntentCaptureOptions, config);
            return new CCTransactionResult()
            {
                Message = capturedPaymentIntent.Status,
                Succeeded = capturedPaymentIntent.Status.ToLower() == "succeeded",
                TransactionID = capturedPaymentIntent.Id,
                Amount = capturedPaymentIntent.Amount
            };
        }

        public async Task<CCTransactionResult> CancelPaymentIntentAsync(FollowUpCCTransaction transaction, OCIntegrationConfig configOverride)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            var cancelPaymentIntentOptions = StripePaymentIntentCancelMapper.MapPaymentIntentCancelOptions(transaction);
            var canceledPaymentIntent = await StripeClient.CancelPaymentIntentAsync(transaction.TransactionID, cancelPaymentIntentOptions, config);
            return new CCTransactionResult()
            {
                Message = canceledPaymentIntent.Status,
                Succeeded = canceledPaymentIntent.Status.ToLower() == "canceled",
                TransactionID = canceledPaymentIntent.Id,
                Amount = canceledPaymentIntent.Amount
            };
        }

        public async Task<CCTransactionResult> CreateRefundAsync(FollowUpCCTransaction transaction, OCIntegrationConfig configOverride)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            var refundCreateOptions = new StripeRefundCreateMapper().MapRefundCreateOptions(transaction);
            var refund = await StripeClient.CreateRefundAsync(refundCreateOptions, config);
            return new CCTransactionResult()
            {
                Message = refund.Status,
                Succeeded = refund.Status.ToLower() == "succeeded",
                TransactionID = refund.Id, // this does not reflect PaymentIntentID
                Amount = refund.Amount
            };
        }
        

        public async Task<PCISafeCardDetails> CreateSavedCreditCardAsync(PaymentSystemCustomer customer, PCISafeCardDetails card, OCIntegrationConfig configOverride)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            var cardCreateOptions = new StripeCardCreateMapper().MapCardCreateOptions(card);
            var createdCard = await StripeClient.CreateCardAsync(customer.ID, cardCreateOptions, config);
            // https://stripe.com/docs/api/payment_methods/attach

            return new PCISafeCardDetails()
            {
                ExpirationMonth = createdCard.ExpMonth.ToString(),
                ExpirationYear = createdCard.ExpYear.ToString(),
                NumberLast4Digits = createdCard.Last4,
                SavedCardID = createdCard.Id,
                CardType = createdCard.Brand,
                CardHolderName = createdCard.Name,
                Token = createdCard.Fingerprint
            };
        }
    }
}
