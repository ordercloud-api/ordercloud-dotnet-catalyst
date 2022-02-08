using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OrderCloud.Catalyst
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum AvalaraDocumentType
	{
		Any = -1,
		SalesOrder = 0,
		SalesInvoice = 1,
		PurchaseOrder = 2,
		PurchaseInvoice = 3,
		ReturnOrder = 4,
		ReturnInvoice = 5,
		InventoryTransferOrder = 6,
		InventoryTransferInvoice = 7,
		ReverseChargeOrder = 8,
		ReverseChargeInvoice = 9
	}

	public class AvalaraCreateTransactionModel
	{
		public List<AvalaraTransactionParameterModel> parameters { get; set; }
		public string description { get; set; }
		public bool? isSellerImporterOfRecord { get; set; }
		public string businessIdentificationNo { get; set; }
		public string posLaneCode { get; set; }
		public DateTime? exchangeRateEffectiveDate { get; set; }
		public decimal? exchangeRate { get; set; }
		public AvalaraServiceMode? serviceMode { get; set; }
		public string currencyCode { get; set; }
		public AvalaraTaxOverrideModel taxOverride { get; set; }
		public string batchCode { get; set; }
		public bool? commit { get; set; }
		public string reportingLocationCode { get; set; }
		public string referenceCode { get; set; }
		public AvalaraTaxDebugLevel? debugLevel { get; set; }
		public AvalaraAddressLocationInfo addresses { get; set; }
		public string exemptionNo { get; set; }
		public string purchaseOrderNo { get; set; }
		public decimal? discount { get; set; }
		public string entityUseCode { get; set; }
		public string customerUsageType { get; set; }
		public string customerCode { get; set; }
		public string salespersonCode { get; set; }
		public DateTime date { get; set; }
		public string companyCode { get; set; }
		public AvalaraDocumentType? type { get; set; }
		public List<AvalaraLineItemModel> lines { get; set; }
		public string code { get; set; }
		public string email { get; set; }
	}

	public class AvalaraTaxOverrideModel
	{
		public AvalaraTaxOverrideType? type { get; set; }
		public decimal? taxAmount { get; set; }
		public DateTime? taxDate { get; set; }
		public string reason { get; set; }
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum AvalaraTaxOverrideType
	{
		None = 0,
		TaxAmount = 1,
		Exemption = 2,
		TaxDate = 3,
		AccruedTaxAmount = 4,
		DeriveTaxable = 5,
		OutOfHarbor = 6
	}

	public class AvalaraTransactionParameterModel
	{
		public string name { get; set; }
		public string value { get; set; }
		public string unit { get; set; }
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum AvalaraServiceMode
	{
		Automatic = 0,
		Local = 1,
		Remote = 2
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum AvalaraTaxDebugLevel
	{
		Normal = 0,
		Diagnostic = 1
	}
}
