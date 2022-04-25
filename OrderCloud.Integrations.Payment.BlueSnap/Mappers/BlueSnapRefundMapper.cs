using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class BlueSnapRefundMapper
	{
		public static BlueSnapRefund ToBlueSnapRefund(FollowUpCCTransaction transaction)
		{
			return new BlueSnapRefund();
		}

		public static CCTransactionResult ToCardTransactionResult(BlueSnapRefundResponse refund)
		{
			return new CCTransactionResult();
		}
	}
}
