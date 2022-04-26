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

        public async Task<CardTransactionResult> AuthorizeOnlyAsync(CreateCardTransaction transaction, OCIntegrationConfig overrideConfig = null) =>
            await CreatePaymentIntentAsync(transaction, "manual", overrideConfig);

        public async Task<CardTransactionResult> AuthorizeAndCaptureAsync(CreateCardTransaction transaction, OCIntegrationConfig overrideConfig = null) =>
            await CreatePaymentIntentAsync(transaction, "automatic", overrideConfig);

        public async Task<CardTransactionResult> CapturePriorAuthorizeAsync(ModifyCardTransaction transaction, OCIntegrationConfig overrideConfig = null) =>
            await CapturePaymentIntentAsync(transaction, overrideConfig);



        public async Task<CardTransactionResult> CreatePaymentIntentAsync(CreateCardTransaction transaction, string captureMethod, OCIntegrationConfig overrideConfig)
        {
            var config = ValidateConfig<StripeConfig>(overrideConfig ?? _defaultConfig);
            var paymentIntentCreateOptions = StripeRequestMapper.MapPaymentIntentCreateOptions(transaction, captureMethod);
            var paymentIntent = await StripeClient.CreatePaymentIntentAsync(paymentIntentCreateOptions, config);
            // map Stripe PaymentIntentCreateOptions back to OC Model
            return new CardTransactionResult();
        }

        public async Task<CardTransactionResult> CapturePaymentIntentAsync(ModifyCardTransaction transaction, OCIntegrationConfig overrideConfig)
        {
            var config = ValidateConfig<StripeConfig>(overrideConfig ?? _defaultConfig);
            //var paymentIntentCaptureOptions =
            return new CardTransactionResult();
        }

        // TODO: IMPLEMENT THESE
        Task<CardTransactionResult> ICreditCardProcessor.VoidAuthorizationAsync(ModifyCardTransaction transactionID, OCIntegrationConfig configOverride = null)
        {
            throw new NotImplementedException();
        }

        Task<CardTransactionResult> ICreditCardProcessor.RefundCaptureAsync(ModifyCardTransaction transactionID, OCIntegrationConfig configOverride = null)
        {
            throw new NotImplementedException();
        }

        Task<CardTransactionStatus> ICreditCardProcessor.GetTransactionAsync(string transactionID, OCIntegrationConfig configOverride = null)
        {
            throw new NotImplementedException();
        }
    }
}
