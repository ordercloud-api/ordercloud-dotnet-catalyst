using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class BlueSnapTransactionResponseMapper
	{
		public static CardTransactionResult ToCardTransactionResult(BlueSnapCardTransactionResponse response)
		{
			return new CardTransactionResult();
		}

		public static CardTransactionStatus ToCardTransactionStatus(BlueSnapCardTransactionResponse response)
		{
			return new CardTransactionStatus();
		}
	}
}
