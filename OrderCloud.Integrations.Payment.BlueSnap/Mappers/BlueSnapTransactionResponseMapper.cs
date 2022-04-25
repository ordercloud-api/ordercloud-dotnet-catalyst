using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class BlueSnapTransactionResponseMapper
	{
		public static CCTransactionResult ToCardTransactionResult(BlueSnapCardTransactionResponse response)
		{
			return new CCTransactionResult();
		}
	}
}
