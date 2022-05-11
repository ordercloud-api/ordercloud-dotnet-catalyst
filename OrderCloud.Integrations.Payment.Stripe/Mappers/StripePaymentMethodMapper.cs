using System.Collections.Generic;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    /// <summary>
    /// https://stripe.com/docs/api/payment_methods
    /// </summary>
    public class StripePaymentMethodMapper
    {
        public PaymentMethodCreateOptions MapPaymentMethodCreateOptions(string customerID, PCISafeCardDetails card) =>
            new PaymentMethodCreateOptions()
            {
                Type = "card",
                Card = new PaymentMethodCardOptions()
                {
                    Token = card.Token
                }
            };

        public CardCreatedResponse MapPaymentMethodCreateResponse(string customerID, PaymentMethod paymentMethod) =>
            new CardCreatedResponse()
            {
                CustomerID = customerID,
                Card = new PCISafeCardDetails()
                {
                    ExpirationMonth = paymentMethod.Card.ExpMonth.ToString(),
                    ExpirationYear = paymentMethod.Card.ExpYear.ToString(),
                    NumberLast4Digits = paymentMethod.Card.Last4,
                    SavedCardID = paymentMethod.Id,
                    CardType = paymentMethod.Card.Brand,
                    Token = paymentMethod.Card.Fingerprint
                }
            };

        public PaymentMethodAttachOptions MapPaymentMethodAttachOptions(string customerID) =>
            new PaymentMethodAttachOptions()
            {
                Customer = customerID
            };


        public PaymentMethodListOptions MapPaymentMethodListOptions(string customerID) =>
            new PaymentMethodListOptions()
            {
                Customer = customerID,
                Type = "card"
            };

        public List<PCISafeCardDetails> MapStripePaymentMethodListResponse(StripeList<PaymentMethod> cardsList)
        {
            var savedCardsList = new List<PCISafeCardDetails>();
            foreach (var method in cardsList.Data)
            {
                savedCardsList.Add(new PCISafeCardDetails()
                {
                    ExpirationMonth = method.Card.ExpMonth.ToString(),
                    ExpirationYear = method.Card.ExpYear.ToString(),
                    NumberLast4Digits = method.Card.Last4,
                    SavedCardID = method.Id,
                    CardType = method.Card.Brand,
                    Token = method.Card.Fingerprint
                });
            }

            return savedCardsList;
        }
    }
}
