using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/refunds
	/// </summary>
	public class BlueSnapRefunds
	{
		public List<BlueSnapRefund> refund { get; set; } = new List<BlueSnapRefund> { };
		public decimal balanceAmount { get; set; }
		public decimal taxBalanceAmount { get; set; }
		public decimal vendorBalanceAmount { get; set; }
		public BlueSnapVendorsBalanceInfo vendorsBalanceInfo { get; set; }

	}


	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/refund
	/// </summary>
	public class BlueSnapRefund
	{
		public decimal amount { get; set; }
		public decimal taxAmount { get; set; }
		public decimal reason { get; set; }
		public bool cancelSubscriptions { get; set; }
		public BlueSnapVendorsRefundInfo vendorsRefundInfo { get; set; } = new BlueSnapVendorsRefundInfo();
		public BlueSnapTransactionMetaData transactionMetaData { get; set; } = new BlueSnapTransactionMetaData();
	}

	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/refund-1#section-response
	/// </summary>
	public class BlueSnapRefundResponse
	{
		public decimal amount { get; set; }
		public decimal taxAmount { get; set; }
		public string currency { get; set; }
		public string date { get; set; }
		public string refundTransactionId { get; set; }
		public decimal vendorAmount { get; set; }
		public BlueSnapVendorsRefundInfo vendorsRefundInfo { get; set; } = new BlueSnapVendorsRefundInfo();
		public BlueSnapTransactionMetaData transactionMetaData { get; set; } = new BlueSnapTransactionMetaData();
		public decimal reason { get; set; }
		public bool cancelSubscriptions { get; set; }
	}

	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/vendorsrefundinfo
	/// </summary>
	public class BlueSnapVendorsBalanceInfo
	{
		public List<BlueSnapVendorRefundInfo> vendorBalanceInfo { get; set; } = new List<BlueSnapVendorRefundInfo> { };

	}

	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/vendorsrefundinfo
	/// </summary>
	public class BlueSnapVendorsRefundInfo
	{
		public List<BlueSnapVendorRefundInfo> vendorRefundInfo { get; set; } = new List<BlueSnapVendorRefundInfo> { };

	}

	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/vendorrefundinfo
	/// </summary>
	public class BlueSnapVendorRefundInfo
	{
		public int vendorId { get; set; }
		public decimal vendorAmount { get; set; }
	}

	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/transaction-meta-data
	/// </summary>
	public class BlueSnapTransactionMetaData
	{
		public List<BlueSnapMetadata> metaData { get; set; } = new List<BlueSnapMetadata> { };
	}

	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/meta-data
	/// </summary>
	public class BlueSnapMetadata
	{
		public string metaKey { get; set; }
		public string metaValue { get; set; }
		public string metaDescription { get; set; }
		// "Y" or "N"
		public string isVisible { get; set; } 

	}
}
