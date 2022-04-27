using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrderCloud.Catalyst.Integrations.Interfaces;
using OrderCloud.Catalyst.Payments.Stripe.Mappers;

namespace OrderCloud.Catalyst.Payments.Stripe
{
    public class StripeCommand : OCIntegrationService, ICreditCardProcessor
    {
        public StripeCommand(StripeConfig defaultConfig) : base(defaultConfig) { }

        public async Task<CCTransactionResult> GetIFrameCredentialsAsync(InitiateCCTransaction transaction = null, OCIntegrationConfig overrideConfig = null) =>
            await CreatePaymentIntentAsync(transaction, "manual", overrideConfig);
        // captureMethod: "automatic" would be used for Authorize and Capture

        public async Task<CCTransactionResult> AuthorizeOnlyAsync(AuthorizeCCTransaction transaction, OCIntegrationConfig overrideConfig = null) =>
            await ConfirmPaymentIntentAsync(transaction,  overrideConfig);

        public async Task<CCTransactionResult> CapturePriorAuthorizationAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig = null) =>
            await CapturePaymentIntentAsync(transaction, overrideConfig);

        public async Task<CCTransactionResult> RefundCaptureAsync(FollowUpCCTransaction transaction, OCIntegrationConfig configOverride = null) =>
            await CreateRefundAsync(transaction, configOverride);

        public async Task<CCTransactionResult> CreatePaymentIntentAsync(InitiateCCTransaction transaction, string captureMethod, OCIntegrationConfig overrideConfig)
        {
            var config = ValidateConfig<StripeConfig>(overrideConfig ?? _defaultConfig);
            var paymentIntentCreateOptions = StripeRequestMapper.MapPaymentIntentCreateOptions(transaction, captureMethod);
            var paymentIntent = await StripeClient.CreatePaymentIntentAsync(paymentIntentCreateOptions, config);
            return new CCTransactionResult()
            {
                CardToken = paymentIntent.ClientSecret,
                TransactionID = paymentIntent.Id
            };
        }

        public async Task<CCTransactionResult> ConfirmPaymentIntentAsync(AuthorizeCCTransaction transaction, OCIntegrationConfig overrideConfig)
        {
            var config = ValidateConfig<StripeConfig>(overrideConfig ?? _defaultConfig);
            var paymentIntentConfirmOptions = StripeRequestMapper.MapPaymentIntentConfirmOptions(transaction);
            var confirmedPaymentIntent = await StripeClient.ConfirmPaymentIntentAsync(transaction.TransactionID, paymentIntentConfirmOptions, config);
            // map Stripe PaymentIntent back to OC Model
            return new CCTransactionResult()
            {
                CardToken = confirmedPaymentIntent.ClientSecret,
                Message = confirmedPaymentIntent.Status,
                Succeeded = confirmedPaymentIntent.Status.ToLower() == "succeeded", // or "requires_capture"??
                TransactionID = confirmedPaymentIntent.Id
            };
        }

        public async Task<CCTransactionResult> CapturePaymentIntentAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig)
        {
            var config = ValidateConfig<StripeConfig>(overrideConfig ?? _defaultConfig);
            var paymentIntentCaptureOptions = StripeRequestMapper.MapPaymentIntentCaptureOptions(transaction);
            var capturedPaymentIntent = await StripeClient.CapturePaymentIntentAsync(transaction.TransactionID, paymentIntentCaptureOptions, config);
            // map Stripe PaymentIntent back to OC Model
            return new CCTransactionResult()
            {
                CardToken = capturedPaymentIntent.ClientSecret,
                Message = capturedPaymentIntent.Status,
                Succeeded = capturedPaymentIntent.Status.ToLower() == "succeeded",
                TransactionID = capturedPaymentIntent.Id
            };
        }

        public async Task<CCTransactionResult> CreateRefundAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig)
        {
            var config = ValidateConfig<StripeConfig>(overrideConfig ?? _defaultConfig);
            var refundCreateOptions = StripeRequestMapper.MapRefundCreateOptions(transaction);
            var refund = await StripeClient.CreateRefundAsync(refundCreateOptions, config);
            // map Stripe Refund back to OC Model
            return new CCTransactionResult();
        }

        // TODO: IMPLEMENT THESE
        Task<CCTransactionResult> ICreditCardProcessor.VoidAuthorizationAsync(FollowUpCCTransaction transaction, OCIntegrationConfig configOverride)
        {
            throw new NotImplementedException();
        }
    }
}
