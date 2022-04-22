using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/card-transaction
	/// </summary>
	public class BlueSnapCardTransaction
	{
		public long? walletId { get; set; } = null;
		public decimal amount { get; set; }
		public int? vaultedShopperId { get; set; } = null;
		public string merchantTransactionId { get; set; }
		public string softDescriptor { get; set; }
		public string descriptorPhoneNumber { get; set; }
		public string taxReference { get; set; }
		public string currency { get; set; }
		/// <summary>
		///  AUTH_ONLY, AUTH_CAPTURE, CAPTURE, AUTH_REVERSAL, REFUND
		/// </summary>
		public string cardTransactionType { get; set; }
		public string pfToken { get; set; }
		public string transactionId { get; set; }
		public string transactionOrderSource { get; set; }
		public string transactionInitiator { get; set; }
		public BlueSnapCardHolderInfo cardHolderInfo { get; set; }
		public BlueSnapCreditCard creditCard { get; set; }
		public BlueSnapWallet wallet { get; set; }
	}

	public class BlueSnapWallet
	{
		/// <summary>
		/// GOOGLE_PAY, APPLE_PAY
		/// </summary>
		public string walletType { get; set; }
		public string encodedPaymentToken { get; set; }
	}
}
