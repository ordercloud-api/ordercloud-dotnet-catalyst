using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.CardConnect.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.CardConnect.Mappers
{
    public static class CardConnectAuthorizationResponseMapper
    {
		public static CCTransactionResult ToIntegrationsCCAuthorizationResponse(this CardConnectAuthorizationResponse transaction)
		{
			return new CCTransactionResult()
            {
				AddressVerificationResponseCode = transaction.avsresp,
				AuthorizationCode = transaction.authcode,
				Message = transaction.resptext,
				ResponseCode = transaction.respstat,
				Succeeded = transaction.respstat != "C", // C is declined, A is Approved, B is Retry
				TransactionID = transaction.retref,
            };
		}
	}
}
