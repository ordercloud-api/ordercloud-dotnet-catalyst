using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public static class BlueSnapVaultedShopperMapper
	{
		public static BlueSnapVaultedShopper BuildAddCardRequest(PaymentSystemCustomer customer, PCISafeCardDetails card)
		{
			return new BlueSnapVaultedShopper()
			{
				firstName = customer.FirstName,
				lastName = customer.LastName,
				email = customer.Email,
				paymentSources = new BlueSnapPaymentSources()
				{
					creditCardInfo = new List<BlueSnapCreditCardInfo>()
					{
						new BlueSnapCreditCardInfo()
						{
							pfToken = card.Token
						}
					}
				}
			};
		}

		public static BlueSnapVaultedShopper BuildDeleteCardRequest(string cardID)
		{
			var (type, last4) = DeconstructCardID(cardID);
			return new BlueSnapVaultedShopper()
			{
				paymentSources = new BlueSnapPaymentSources()
				{
					creditCardInfo = new List<BlueSnapCreditCardInfo>()
					{
						new BlueSnapCreditCardInfo()
						{
							creditCard = new BlueSnapCreditCard()
							{
								cardType = type,
								cardLastFourDigits = last4
							},
							status = "D" // for "Delete"
						}
					}
				}
			};
		}

		public static List<PCISafeCardDetails> ToCardList(BlueSnapVaultedShopper shopper)
		{
			var cards = shopper.paymentSources.creditCardInfo.Select(ccInfo =>
			{
				var card = new PCISafeCardDetails()
				{
					// Token and CardHolderName are not available
					NumberLast4Digits = ccInfo.creditCard.cardLastFourDigits,
					ExpirationMonth = ccInfo.creditCard.expirationMonth.ToString(),
					ExpirationYear = ccInfo.creditCard.expirationYear.ToString(),
					CardType = ccInfo.creditCard.cardType,
				};
				card.SavedCardID = BuildSavedCardID(card);
				return card;
			}).ToList();

			return cards;
		}

		public static PCISafeCardDetails FirstOrDefaultWithID(this IEnumerable<PCISafeCardDetails> cards, string cardID)
		{
			return cards.FirstOrDefault(c => CardMatchesID(cardID, c));
		}

		private static bool CardMatchesID(string cardID, PCISafeCardDetails card) 
		{
			var (type, last4) = DeconstructCardID(cardID);
			return card.CardType == type && card.NumberLast4Digits == last4;
		}

		public static (string type, string last4) DeconstructCardID(string cardID) 
		{
			var split = cardID.Split('-');
			return (split[0], split[1]);
		}

		public static string BuildSavedCardID(PCISafeCardDetails card) => $"{card.CardType}-{card.NumberLast4Digits}";
	}
}
