using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.CardConnect.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.CardConnect.Mappers
{
    public static class CardConnectCaptureResponseMapper
    {
		public static CCTransactionResult ToIntegrationsCCTransactionResponse(this CardConnectCaptureResponse transaction)
		{
			return new CCTransactionResult()
            {
				//AddressVerificationResponseCode = transaction.avsresp,
				AuthorizationCode = transaction.authcode,
                Message = transaction.setlstat,
                //ResponseCode = transaction.respcode,
                Succeeded = transaction.respstat == "A",
                TransactionID = transaction.retref,
				Amount = Convert.ToDecimal(transaction.amount)
            };
		}
	}
}
