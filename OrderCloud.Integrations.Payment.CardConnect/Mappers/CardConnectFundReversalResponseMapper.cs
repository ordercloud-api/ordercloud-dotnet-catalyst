using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.CardConnect.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.CardConnect.Mappers
{
    public static class CardConnectFundReversalResponseMapper
    {
		public static CCTransactionResult ToIntegrationsCCFundReversalResponse(this CardConnectFundReversalResponse transaction)
		{
			return new CCTransactionResult()
            {
				Succeeded = transaction.respstat == "A",
				ResponseCode = transaction.respcode,
				Message = transaction.resptext,
				TransactionID = transaction.retref,
				Amount = Convert.ToInt64(transaction.amount),
            };
		}
	}
}
