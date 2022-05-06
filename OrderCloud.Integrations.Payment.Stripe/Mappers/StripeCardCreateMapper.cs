using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    public class StripeCardCreateMapper
    {
        public CardCreateOptions MapCardCreateOptions(PCISafeCardDetails card)
        {
            // How do we get this token from Stripe?
            // A token, like the ones returned by Stripe.js. Stripe will automatically validate the card.
            // https://stripe.com/docs/api/cards/create
            return new CardCreateOptions()
            {
                Source = card.Token
            };
        }

        public CardCreatedResponse MapCardCreateResponse(string customerID, Card card)
        {
            return new CardCreatedResponse()
            {
                CustomerID = customerID,
                Card = new PCISafeCardDetails()
                {
                    ExpirationMonth = card.ExpMonth.ToString(),
                    ExpirationYear = card.ExpYear.ToString(),
                    NumberLast4Digits = card.Last4,
                    SavedCardID = card.Id,
                    CardType = card.Brand,
                    CardHolderName = card.Name,
                    Token = card.Fingerprint
                }
            };
        }
    }
}
