using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OrderCloud.Catalyst.Tax.Avalara
{
	public class AvalaraTransactionModel
	{
		public decimal? totalTax { get; set; }
		public AvalaraAdjustmentReason? adjustmentReason { get; set; }
		public string adjustmentDescription { get; set; }
		public bool? locked { get; set; }
		public string region { get; set; }
		public string country { get; set; }
		public int? version { get; set; }
		public string softwareVersion { get; set; }
		public long? originAddressId { get; set; }
		public long? destinationAddressId { get; set; }
		public DateTime? exchangeRateEffectiveDate { get; set; }
		public decimal? exchangeRate { get; set; }
		public bool? isSellerImporterOfRecord { get; set; }
		public string description { get; set; }
		public string email { get; set; }
		public string businessIdentificationNo { get; set; }
		public DateTime? modifiedDate { get; set; }
		public int? modifiedUserId { get; set; }
		public DateTime? taxDate { get; set; }
		public List<AvalaraTransactionLineModel> lines { get; set; }
		public List<AvalaraTransactionAddressModel> addresses { get; set; }
		public List<AvalaraTransactionLocationTypeModel> locationTypes { get; set; }
		public List<AvalaraTransactionSummary> summary { get; set; }
		public List<AvalaraTaxDetailsByTaxType> taxDetailsByTaxType { get; set; }
		public List<AvalaraTransactionParameterModel> parameters { get; set; }
		public decimal? totalTaxCalculated { get; set; }
		public decimal? totalTaxable { get; set; }
		public List<AvalaraInvoiceMessageModel> invoiceMessages { get; set; }
		public decimal? totalDiscount { get; set; }
		public long? id { get; set; }
		public string code { get; set; }
		public int? companyId { get; set; }
		public DateTime? date { get; set; }
		public DateTime? paymentDate { get; set; }
		public AvalaraDocumentStatus? status { get; set; }
		public AvalaraDocumentType? type { get; set; }
		public string batchCode { get; set; }
		public string currencyCode { get; set; }
		public string customerUsageType { get; set; }
		public string entityUseCode { get; set; }
		public string customerVendorCode { get; set; }
		public string customerCode { get; set; }
		public string exemptNo { get; set; }
		public bool? reconciled { get; set; }
		public string locationCode { get; set; }
		public string reportingLocationCode { get; set; }
		public string purchaseOrderNo { get; set; }
		public string referenceCode { get; set; }
		public string salespersonCode { get; set; }
		public AvalaraTaxOverrideType? taxOverrideType { get; set; }
		public decimal? taxOverrideAmount { get; set; }
		public string taxOverrideReason { get; set; }
		public decimal? totalAmount { get; set; }
		public decimal? totalExempt { get; set; }
		public List<AvalaraTaxMessage> messages { get; set; }
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum AvalaraAdjustmentReason
	{
		NotAdjusted = 0,
		SourcingIssue = 1,
		ReconciledWithGeneralLedger = 2,
		ExemptCertApplied = 3,
		PriceAdjusted = 4,
		ProductReturned = 5,
		ProductExchanged = 6,
		BadDebt = 7,
		Other = 8,
		Offline = 9
	}

	public class AvalaraTaxMessage
	{
		public string summary { get; set; }
		public string details { get; set; }
		public string refersTo { get; set; }
		public string severity { get; set; }
		public string source { get; set; }
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum AvalaraDocumentStatus
	{
		Any = -1,
		Temporary = 0,
		Saved = 1,
		Posted = 2,
		Committed = 3,
		Cancelled = 4,
		Adjusted = 5,
		Queued = 6,
		PendingApproval = 7
	}

	public class AvalaraInvoiceMessageModel
	{
		public string content { get; set; }
		public List<string> lineNumbers { get; set; }
	}

	public class AvalaraTaxDetailsByTaxType
	{
		public string taxType { get; set; }
		public decimal? totalTaxable { get; set; }
		public decimal? totalExempt { get; set; }
		public decimal? totalNonTaxable { get; set; }
		public decimal? totalTax { get; set; }
		public List<AvalaraTaxDetailsByTaxSubType> taxSubTypeDetails { get; set; }
	}

	public class AvalaraTaxDetailsByTaxSubType
	{
		public string taxSubType { get; set; }
		public decimal? totalTaxable { get; set; }
		public decimal? totalExempt { get; set; }
		public decimal? totalNonTaxable { get; set; }
		public decimal? totalTax { get; set; }
	}

	public class AvalaraTransactionSummary
	{
		public string taxSubType { get; set; }
		public decimal? taxCalculated { get; set; }
		public decimal? tax { get; set; }
		public decimal? rate { get; set; }
		public decimal? taxable { get; set; }
		public string rateTypeCode { get; set; }
		public AvalaraRateType? rateType { get; set; }
		public string taxGroup { get; set; }
		public string taxName { get; set; }
		public decimal? exemption { get; set; }
		public string taxType { get; set; }
		public string stateAssignedNo { get; set; }
		public int? taxAuthorityType { get; set; }
		public string jurisName { get; set; }
		public string jurisCode { get; set; }
		public AvalaraJurisdictionType? jurisType { get; set; }
		public string region { get; set; }
		public string country { get; set; }
		public decimal? nonTaxable { get; set; }
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum AvalaraRateType
	{
		ReducedA = 65,
		ReducedB = 66,
		Food = 70,
		General = 71,
		IncreasedStandard = 73,
		LinenRental = 76,
		Medical = 77,
		Parking = 80,
		SuperReduced = 81,
		ReducedR = 82,
		Standard = 83,
		Services = 88,
		Zero = 90
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum AvalaraJurisdictionType
	{
		Country = 0,
		State = 1,
		County = 2,
		City = 3,
		Special = 4
	}

	public class AvalaraTransactionLocationTypeModel
	{
		public long? documentLocationTypeId { get; set; }
		public long? documentId { get; set; }
		public long? documentAddressId { get; set; }
		public string locationTypeCode { get; set; }
	}
}
