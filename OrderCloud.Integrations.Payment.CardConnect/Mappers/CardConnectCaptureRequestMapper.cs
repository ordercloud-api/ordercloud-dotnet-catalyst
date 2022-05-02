using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.CardConnect.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.CardConnect.Mappers
{
    public static class CardConnectCaptureRequestMapper
    {
		public static CardConnectCaptureRequest ToCardConnectCaptureRequest(this FollowUpCCTransaction transaction, CardConnectConfig config)
		{
			return new CardConnectCaptureRequest()
            {
				merchid = config.MerchantId,
				retref = transaction.TransactionID
            };
		}
	}
}
