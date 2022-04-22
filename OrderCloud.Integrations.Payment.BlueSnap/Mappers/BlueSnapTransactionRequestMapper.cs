using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class CreateTransactionMapper
	{
		public static BlueSnapCardTransaction ToBlueSnapCardTransaction(BlueSnapTransactionType transactionType, CreateCardTransaction transaction)
		{
			return new BlueSnapCardTransaction()
			{
				cardTransactionType = transactionType.ToString(),
				amount = transaction.Amount,
				currency = transaction.Currency,
				pfToken = transaction.CardToken,
				merchantTransactionId = transaction.OrderID
			};
		}

		public static BlueSnapCardTransaction ToBlueSnapCardTransaction(BlueSnapTransactionType transactionType, ModifyCardTransaction transaction)
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
