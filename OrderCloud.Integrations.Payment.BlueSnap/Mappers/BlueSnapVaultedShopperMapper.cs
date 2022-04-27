using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class BlueSnapVaultedShopperMapper
	{
		public static BlueSnapVaultedShopper BuildCreateShopperRequest(PaymentSystemCustomer customer, PCISafeCardDetails card)
		{
			return new BlueSnapVaultedShopper()
			{

			};
		}

		public static BlueSnapVaultedShopper BuildAddCardRequest(PaymentSystemCustomer customer, PCISafeCardDetails card)
		{
			return new BlueSnapVaultedShopper()
			{

			};
		}

		public static BlueSnapVaultedShopper BuildDeleteCardRequest(string cardID)
		{
			return new BlueSnapVaultedShopper()
			{

			};
		}

		public static List<PCISafeCardDetails> ToCardList(BlueSnapVaultedShopper shopper)
		{
			return new List<PCISafeCardDetails>()
			{

			};
		}

		public static bool CardMatchesID(PCISafeCardDetails card) => true;
	}
}
