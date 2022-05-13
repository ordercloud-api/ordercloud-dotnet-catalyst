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
            this List<CardConnectProfile> response)
        {
            List<PCISafeCardDetails> savedCards = new List<PCISafeCardDetails>();
            foreach (CardConnectProfile card in response)
            {
                savedCards.Add(new PCISafeCardDetails()
                {
                    SavedCardID = card.acctid,
                    CardType = card.accttype,
                    ExpirationMonth = card.expiry.Substring(0, 2),
                    ExpirationYear = card.expiry.Substring(2, 2),
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
        public static CardConnectCreateUpdateProfileRequest ToCardConnectUpdateProfileRequest(this PCISafeCardDetails card, PaymentSystemCustomer customer, string merchantid)
        {
            return new CardConnectCreateUpdateProfileRequest()
            {
                accttype = card.CardType,
                expiry = $"{card.ExpirationMonth}{card.ExpirationYear}",
                name = card.CardHolderName,
                profile = customer.ID,
                merchid = merchantid
            };
        }
        public static CardConnectCreateUpdateProfileRequest ToCardConnectCreateProfileRequest(
            this PCISafeCardDetails card, string merchantid)
        {
            return new CardConnectCreateUpdateProfileRequest()
            {
                accttype = card.CardType,
                expiry = $"{card.ExpirationMonth}{card.ExpirationYear}",
                name = card.CardHolderName,
                account = card.Token,
                merchid = merchantid
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
                    NumberLast4Digits = card.token.Substring(card.token.Length - 4, 4)
                },
                CustomerID = card.profileid
            };
        }
    }
}
