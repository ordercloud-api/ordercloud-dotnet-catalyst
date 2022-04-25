using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class BlueSnapTransactionRequestMapper
	{
		public static BlueSnapCardTransaction ToBlueSnapCardTransaction(BlueSnapTransactionType transactionType, AuthorizeCCTransaction transaction)
		{
			var blueSnapTransaction = new BlueSnapCardTransaction()
			{
				cardTransactionType = transactionType.ToString(),
				amount = transaction.Amount,
				currency = transaction.Currency,
				pfToken = transaction.CardToken,
				merchantTransactionId = transaction.OrderID,
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
