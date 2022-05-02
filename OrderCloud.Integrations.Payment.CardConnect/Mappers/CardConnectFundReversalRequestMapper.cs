using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.CardConnect.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.CardConnect.Mappers
{
    public static class CardConnectFundReversalRequestMapper
    {
		public static CardConnectFundReversalRequest ToCardConnectFundReversalRequest(this FollowUpCCTransaction transaction, CardConnectConfig config)
		{
			return new CardConnectFundReversalRequest()
            {
				merchid = config.MerchantId,
				retref = transaction.TransactionID,
				amount = transaction.Amount,
            };
		}
	}
}
