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

        public async Task<CCTransactionResult> AuthorizeOnlyAsync(AuthorizeCCTransaction transaction,
            OCIntegrationConfig configOverride = null)
        {
            {
                var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
                var paymentIntentCreateOptions = new StripePaymentIntentCreateMapper().MapPaymentIntentCreateAndConfirmOptions(transaction);
                var createdPaymentIntent = await StripeClient.CreateAndConfirmPaymentIntentAsync(paymentIntentCreateOptions, config);
                return new CCTransactionResult()
                {
                    Message = createdPaymentIntent.Status,
                    Succeeded = createdPaymentIntent.Status.ToLower() == "requires_capture",
                    TransactionID = createdPaymentIntent.Id, // transaction.TransactionID represents PaymentMethodID, this now represents PaymentIntentID
                    Amount = createdPaymentIntent.Amount
                };
            }
        }

        public async Task<CCTransactionResult> CapturePriorAuthorizationAsync(FollowUpCCTransaction transaction, 
            OCIntegrationConfig configOverride = null)
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

        public async Task<CCTransactionResult> VoidAuthorizationAsync(FollowUpCCTransaction transaction,
            OCIntegrationConfig configOverride = null)
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

        public async Task<CCTransactionResult> RefundCaptureAsync(FollowUpCCTransaction transaction,
            OCIntegrationConfig configOverride = null)
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
        #endregion

        #region ICreditCardSaver

        public async Task<PCISafeCardDetails> CreateSavedCardAsync(PaymentSystemCustomer customer,
            PCISafeCardDetails card, OCIntegrationConfig configOverride = null)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            var cardCreateOptions = new StripeCardCreateMapper().MapCardCreateOptions(card);

            if (customer.CustomerAlreadyExists)
            {
                // do we need to do anything here?
            }
            else
            {
                var stripeCustomerOptions = StripeCustomerCreateMapper.MapCustomerOptions(customer);
                var stripeCustomer = await StripeClient.CreateCustomerAsync(stripeCustomerOptions, config);
                customer.ID = stripeCustomer.Id;
            }
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

        public async Task<List<PCISafeCardDetails>> ListSavedCardsAsync(string customerID,
            OCIntegrationConfig configOverride = null)
        {
            {
                var savedCardsList = new List<PCISafeCardDetails>();
                var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
                var cardsList = await StripeClient.ListCreditCardsAsync(customerID, config);
                foreach (var card in cardsList.Data)
                {
                    savedCardsList.Add(new PCISafeCardDetails()
                    {
                        ExpirationMonth = card.ExpMonth.ToString(),
                        ExpirationYear = card.ExpYear.ToString(),
                        NumberLast4Digits = card.Last4,
                        SavedCardID = card.Id,
                        CardType = card.Brand,
                        CardHolderName = card.Name,
                        Token = card.Fingerprint
                    });
                }
                return savedCardsList;
            }
        }

        public async Task<PCISafeCardDetails> GetSavedCardAsync(string customerID, string cardID,
            OCIntegrationConfig configOverride = null)
        {
            var config = ValidateConfig<StripeConfig>(configOverride ?? _defaultConfig);
            var creditCard = await StripeClient.GetCreditCardAsync(customerID, cardID, config);
            return new PCISafeCardDetails()
            {
                ExpirationMonth = creditCard.ExpMonth.ToString(),
                ExpirationYear = creditCard.ExpYear.ToString(),
                NumberLast4Digits = creditCard.Last4,
                SavedCardID = creditCard.Id,
                CardType = creditCard.Brand,
                CardHolderName = creditCard.Name,
                Token = creditCard.Fingerprint
            };
        }

        public Task DeleteSavedCardAsync(string customerID, string cardID, OCIntegrationConfig configOverride = null)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
