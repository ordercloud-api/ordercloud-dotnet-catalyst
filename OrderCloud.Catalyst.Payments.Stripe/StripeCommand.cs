using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrderCloud.Catalyst.Payments.Stripe.Mappers;

namespace OrderCloud.Catalyst.Payments.Stripe
{
    public class StripeCommand : OCIntegrationService, ICreditCardProcessor
    {
        public StripeCommand(StripeConfig defaultConfig) : base(defaultConfig) { }

        public async Task<CCTransactionResult> AuthorizeOnlyAsync(AuthorizeCCTransaction transaction, OCIntegrationConfig overrideConfig = null) =>
            await ConfirmPaymentIntentAsync(transaction,  overrideConfig);

        public async Task<CCTransactionResult> CapturePriorAuthorizationAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig = null) =>
            await CapturePaymentIntentAsync(transaction, overrideConfig);

        public async Task<CCTransactionResult> RefundCaptureAsync(FollowUpCCTransaction transaction, OCIntegrationConfig configOverride = null) =>
            await CreateRefundAsync(transaction, configOverride);

        // A user's middleware will have to handle creating this, loading the iframe, collection payment,
        // and sending us the transactionID for Confirm/Authorize, Capture, Refund, Void
        //public async Task<CCTransactionResult> CreatePaymentIntentAsync(InitiateCCTransaction transaction, string captureMethod, OCIntegrationConfig overrideConfig)
        //{
        //    var config = ValidateConfig<StripeConfig>(overrideConfig ?? _defaultConfig);
        //    var paymentIntentCreateOptions = StripeRequestMapper.MapPaymentIntentCreateOptions(transaction, captureMethod);
        //    var paymentIntent = await StripeClient.CreatePaymentIntentAsync(paymentIntentCreateOptions, config);
        //    return new CCTransactionResult()
        //    {
        //        CardToken = paymentIntent.ClientSecret,
        //        TransactionID = paymentIntent.Id
        //    };
        //}

        public async Task<CCTransactionResult> ConfirmPaymentIntentAsync(AuthorizeCCTransaction transaction, OCIntegrationConfig overrideConfig)
        {
            var config = ValidateConfig<StripeConfig>(overrideConfig ?? _defaultConfig);
            var paymentIntentConfirmOptions = StripeRequestMapper.MapPaymentIntentConfirmOptions(transaction);
            var confirmedPaymentIntent = await StripeClient.ConfirmPaymentIntentAsync(transaction.TransactionID, paymentIntentConfirmOptions, config);
            return new CCTransactionResult()
            {
                Message = confirmedPaymentIntent.Status,
                Succeeded = confirmedPaymentIntent.Status.ToLower() == "succeeded", // or "requires_capture"??
                TransactionID = confirmedPaymentIntent.Id,
                Amount = confirmedPaymentIntent.Amount
            };
        }

        public async Task<CCTransactionResult> CapturePaymentIntentAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig)
        {
            var config = ValidateConfig<StripeConfig>(overrideConfig ?? _defaultConfig);
            var paymentIntentCaptureOptions = StripeRequestMapper.MapPaymentIntentCaptureOptions(transaction);
            var capturedPaymentIntent = await StripeClient.CapturePaymentIntentAsync(transaction.TransactionID, paymentIntentCaptureOptions, config);
            return new CCTransactionResult()
            {
                Message = capturedPaymentIntent.Status,
                Succeeded = capturedPaymentIntent.Status.ToLower() == "succeeded",
                TransactionID = capturedPaymentIntent.Id,
                Amount = capturedPaymentIntent.Amount
            };
        }

        public async Task<CCTransactionResult> CreateRefundAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig)
        {
            var config = ValidateConfig<StripeConfig>(overrideConfig ?? _defaultConfig);
            var refundCreateOptions = StripeRequestMapper.MapRefundCreateOptions(transaction);
            var refund = await StripeClient.CreateRefundAsync(refundCreateOptions, config);
            return new CCTransactionResult()
            {
                Message = refund.Status,
                Succeeded = refund.Status.ToLower() == "succeeded",
                TransactionID = refund.Id, // this does not reflect PaymentIntentID
                Amount = refund.Amount
            };
        }
        
        public async Task<CCTransactionResult> VoidAuthorizationAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig)
        {
            var config = ValidateConfig<StripeConfig>(overrideConfig ?? _defaultConfig);
            var cancelPaymentIntentOptions = StripeRequestMapper.MapPaymentIntentCancelOptions(transaction);
            var canceledPaymentIntent = await StripeClient.CancelPaymentIntentAsync(transaction.TransactionID, cancelPaymentIntentOptions, config);
            return new CCTransactionResult()
            {
                Message = canceledPaymentIntent.Status,
                Succeeded = canceledPaymentIntent.Status.ToLower() == "canceled",
                TransactionID = canceledPaymentIntent.Id,
                Amount = canceledPaymentIntent.Amount
            };
        }
    }
}
