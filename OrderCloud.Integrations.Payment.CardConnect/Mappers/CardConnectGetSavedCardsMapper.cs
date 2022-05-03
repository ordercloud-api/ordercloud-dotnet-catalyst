using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.CardConnect.Models;

namespace OrderCloud.Integrations.Payment.CardConnect.Mappers
{
    public static class CardConnectGetSavedCardsMapper
    {
        public static List<PCISafeCardDetails> ToIntegrationsGetSavedCardsResponse(
            this CardConnectGetProfileResponse response)
        {
            List<PCISafeCardDetails> savedCards = new List<PCISafeCardDetails>();
            foreach (CardConnectProfile card in response.profiles)
            {
                savedCards.Add(new PCISafeCardDetails()
                {
                    SavedCardID = card.acctid,
                    CardType = card.accttype,
                    ExpirationMonth = card.expiry.Substring(0, 2),
                    ExpirationYear = card.expiry.Substring(2,2),
                    CardHolderName = card.name,
                    Token = card.token,
                    NumberLast4Digits = card.token.Substring(card.token.Length - 5, 4)
                });
            }

            return savedCards;
        }
        public static PCISafeCardDetails ToIntegrationsGetSavedCardResponse(
            this CardConnectProfile card)
        {
            return new PCISafeCardDetails()
            {
                SavedCardID = card.acctid,
                CardType = card.accttype,
                ExpirationMonth = card.expiry.Substring(0, 2),
                ExpirationYear = card.expiry.Substring(2, 2),
                CardHolderName = card.name,
                Token = card.token,
                NumberLast4Digits = card.token.Substring(card.token.Length - 5, 4)
            };
        }
        public static CardConnectCreateUpdateProfileRequest ToCardConnectCreateUpdateProfileRequest(
            this PCISafeCardDetails card, PaymentSystemCustomer customer)
        {
            return new CardConnectCreateUpdateProfileRequest()
            {
                accttype = card.CardType,
                expiry = $"{card.ExpirationMonth}{card.ExpirationYear}",
                name = card.CardHolderName,
                account = card.Token,
                profile = customer.ID,
            };
        }
        public static CardCreatedResponse ToIntegrationsCardCreatedResponse(
            this CardConnectCreateUpdateProfileResponse card)
        {
            return new CardCreatedResponse()
            {
                Card = new PCISafeCardDetails()
                {
                    SavedCardID = card.acctid,
                    CardType = card.accttype,
                    ExpirationMonth = card.expiry.Substring(0, 2),
                    ExpirationYear = card.expiry.Substring(2, 2),
                    CardHolderName = card.name,
                    Token = card.token,
                    NumberLast4Digits = card.token.Substring(card.token.Length - 5, 4)
                },
                CustomerID = card.profileid
            };
        }
    }
}
