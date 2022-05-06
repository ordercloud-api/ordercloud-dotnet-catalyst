using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public enum BlueSnapTransactionType
	{
		AUTH_ONLY,
		AUTH_CAPTURE,
		CAPTURE,
		AUTH_REVERSAL,
		REFUND
	}
}
