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
			return new CCTransactionResult()
			{
				// See https://developers.bluesnap.com/v8976-JSON/docs/processing-info
				Succeeded = response.processingInfo.processingStatus == "success",
				TransactionID = response.transactionId,
				ResponseCode = response.processingInfo.processingStatus,
				AuthorizationCode = response.processingInfo.authorizationCode,
				AVSResponseCode = response.avsResponseCode,
				Message = response.processingInfo.processingStatus
			};
		}
	}
}
