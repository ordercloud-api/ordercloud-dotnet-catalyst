using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class BlueSnapTransactionMapper
	{
		public static CCTransactionResult ToCardTransactionResult(BlueSnapCardTransactionResponse response)
		{
			return new CCTransactionResult()
			{
				// See https://developers.bluesnap.com/v8976-JSON/docs/processing-info
				Succeeded = response.processingInfo.processingStatus.ToLower() == "success",
				Amount = response.amount,
				TransactionID = response.transactionId,
				ResponseCode = response.processingInfo.processingStatus,
				AuthorizationCode = response.processingInfo.authorizationCode,
				AVSResponseCode = response.avsResponseCode,
				Message = response.processingInfo.processingStatus,
			};
		}

		public static BlueSnapCardTransaction ToBlueSnapCardTransaction(BlueSnapTransactionType transactionType, AuthorizeCCTransaction transaction)
		{
			var blueSnapTransaction = new BlueSnapCardTransaction()
			{
				cardTransactionType = transactionType.ToString(),
				amount = transaction.Amount,
				currency = transaction.Currency,
				pfToken = transaction.CardDetails.Token,
				merchantTransactionId = transaction.OrderID,
				cardHolderInfo = new BlueSnapCardHolderInfo()
				{
					firstName = transaction.CardDetails.CardHolderName.Split(' ').First(),
					lastName = transaction.CardDetails.CardHolderName.Split(' ').Last(),
					zip = transaction.AddressVerification.Zip
				}, 
				transactionFraudInfo = new BlueSnapTransactionFraudInfo()
				{
					shopperIpAddress = transaction.CustomerIPAddress,
					shippingContactInfo = new BlueSnapShippingContactInfo()
					{
						address1 = transaction.AddressVerification.Street1,
						address2 = transaction.AddressVerification.Street2,
						city = transaction.AddressVerification.City,
						zip = transaction.AddressVerification.Zip,
						state = transaction.AddressVerification.State,
						country = transaction.AddressVerification.Country
					}
				}
			};
			return blueSnapTransaction;
		}

		public static BlueSnapCardTransaction ToBlueSnapCardTransaction(BlueSnapTransactionType transactionType, FollowUpCCTransaction transaction)
		{
			return new BlueSnapCardTransaction()
			{
				cardTransactionType = transactionType.ToString(),
				amount = transaction.Amount,
				merchantTransactionId = transaction.TransactionID
			};
		}
	}
}
