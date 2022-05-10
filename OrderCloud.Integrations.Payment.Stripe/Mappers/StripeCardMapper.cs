using System.Collections.Generic;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    /// <summary>
    /// https://stripe.com/docs/api/cards
    /// </summary>
    public class StripeCardMapper
    {

        public CardCreateOptions MapStripeCardCreateOptions(PCISafeCardDetails card)
        {
            return new CardCreateOptions()
            {
                Source = card.Token
            };
        }

        public PCISafeCardDetails MapStripeCardGetResponse(PaymentMethod paymentMethod)
        {
            return new PCISafeCardDetails()
            {
                ExpirationMonth = paymentMethod.Card.ExpMonth.ToString(),
                ExpirationYear = paymentMethod.Card.ExpYear.ToString(),
                NumberLast4Digits = paymentMethod.Card.Last4,
                SavedCardID = paymentMethod.Id,
                CardType = paymentMethod.Card.Brand,
                Token = paymentMethod.Card.Fingerprint
            };
        }
    }
}