using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OrderCloud.Catalyst.Tax.Avalara
{
	public class AvalaraTransactionLineModel
	{
		public string ref2 { get; set; }
		public AvalaraSourcing? sourcing { get; set; }
		public decimal? tax { get; set; }
		public decimal? taxableAmount { get; set; }
		public decimal? taxCalculated { get; set; }
		public string taxCode { get; set; }
		public int? taxCodeId { get; set; }
		public DateTime? taxDate { get; set; }
		public string taxEngine { get; set; }
		public AvalaraTaxOverrideType? taxOverrideType { get; set; }
		public string businessIdentificationNo { get; set; }
		public decimal? taxOverrideAmount { get; set; }
		public string taxOverrideReason { get; set; }
		public bool? taxIncluded { get; set; }
		public List<AvalaraTransactionLineDetailModel> details { get; set; }
		public List<AvalaraTransactionLineDetailModel> nonPassthroughDetails { get; set; }
		public List<AvalaraTransactionLocationTypeModel> lineLocationTypes { get; set; }
		public List<AvalaraTransactionParameterModel> parameters { get; set; }
		public string hsCode { get; set; }
		public decimal? costInsuranceFreight { get; set; }
		public string revAccount { get; set; }
		public DateTime? reportingDate { get; set; }
		public int? vatNumberTypeId { get; set; }
		public string ref1 { get; set; }
		public long? id { get; set; }
		public long? transactionId { get; set; }
		public string lineNumber { get; set; }
		public int? boundaryOverrideId { get; set; }
		public string customerUsageType { get; set; }
		public string entityUseCode { get; set; }
		public string description { get; set; }
		public long? destinationAddressId { get; set; }
		public long? originAddressId { get; set; }
		public string vatCode { get; set; }
		public decimal? discountAmount { get; set; }
		public decimal? exemptAmount { get; set; }
		public int? exemptCertId { get; set; }
		public string certificateId { get; set; }
		public string exemptNo { get; set; }
		public bool? isItemTaxable { get; set; }
		public bool? isSSTP { get; set; }
		public string itemCode { get; set; }
		public decimal? lineAmount { get; set; }
		public decimal? quantity { get; set; }
		public int? discountTypeId { get; set; }
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum AvalaraSourcing
	{
		Mixed = 42,
		Destination = 68,
		Origin = 79
	}
}
