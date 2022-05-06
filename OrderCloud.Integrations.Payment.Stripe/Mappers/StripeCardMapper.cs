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

        public CardCreatedResponse MapStripeCardCreateResponse(string customerID, Card card)
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

        public PCISafeCardDetails MapStripeCardGetResponse(Card card)
        {
            return new PCISafeCardDetails()
            {
                ExpirationMonth = card.ExpMonth.ToString(),
                ExpirationYear = card.ExpYear.ToString(),
                NumberLast4Digits = card.Last4,
                SavedCardID = card.Id,
                CardType = card.Brand,
                CardHolderName = card.Name,
                Token = card.Fingerprint
            };
        }

        public List<PCISafeCardDetails> MapStripeCardListResponse(StripeList<Card> cardsList)
        {
            var savedCardsList = new List<PCISafeCardDetails>();
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
}