using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/card-transaction#section-response
	/// </summary>
	public class BlueSnapCardTransactionResponse
	{
		public decimal amount { get; set; }
		public decimal openToCapture { get; set; }
		public int? vaultedShopperId { get; set; } = null;
		public string merchantTransactionId { get; set; }
		public BlueSnapProcessingInfo processingInfo { get; set; } = new BlueSnapProcessingInfo();
		public string softDescriptor { get; set; }
		public string descriptorPhoneNumber { get; set; }
		public string taxReference { get; set; }
		public BlueSnapCardHolderInfo cardHolderInfo { get; set; } = new BlueSnapCardHolderInfo();
		public string currency { get; set; }
		// mm/dd/yyy
		public string transactionApprovalDate { get; set; }
		// hh:MM:ss
		public string transactionApprovalTime { get; set; }
		public BlueSnapFraudResultInfo fraudResultInfo { get; set; } = new BlueSnapFraudResultInfo();
		public BlueSnapCreditCardResponse creditCard { get; set; } = new BlueSnapCreditCardResponse();
		/// <summary>
		///  AUTH_ONLY, AUTH_CAPTURE, CAPTURE, AUTH_REVERSAL, REFUND
		/// </summary>
		public string cardTransactionType { get; set; }
		public string transactionId { get; set; }
		public string originalTransactionId { get; set; }
		public BlueSnapChargeBacks chargebacks = new BlueSnapChargeBacks();
		public BlueSnapRefunds refunds = new BlueSnapRefunds();


	}

	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/processing-info
	/// </summary>
	public class BlueSnapProcessingInfo
	{
		public string processingStatus { get; set; }
		public string cvvResponseCode { get; set; }
		public string authorizationCode { get; set; }
		public string avsResponseCodeZip { get; set; }
		public string avsResponseCodeAddress { get; set; }
		public string avsResponseCodeName { get; set; }
		public string networkTransactionId { get; set; }
	}

	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/fraud-result-info
	/// </summary>
	public class BlueSnapFraudResultInfo
	{
		public string deviceDataCollector { get; set; }
	}


	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/chargebacks
	/// </summary>
	public class BlueSnapChargeBacks
	{
		public List<BlueSnapChargeBack> chargeback { get; set; } = new List<BlueSnapChargeBack> { };
	}

	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/chargeback
	/// </summary>
	public class BlueSnapChargeBack
	{
		public decimal amount { get; set; }
		public int? chargebackTransactionId { get; set; } = null;
		public string currency { get; set; }
		public string date { get; set; }
	}
}
