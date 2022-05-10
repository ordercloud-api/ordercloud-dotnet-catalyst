using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    public class StripePaymentMethodMapper
    {

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
