using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OrderCloud.Integrations.Tax.Avalara
{
	public class AvalaraTransactionLineDetailModel
	{
		public AvalaraTaxRuleTypeId? nonTaxableType { get; set; }
		public int? rateSourceId { get; set; }
		public string serCode { get; set; }
		public AvalaraSourcing? sourcing { get; set; }
		public decimal? tax { get; set; }
		public decimal? taxableAmount { get; set; }
		public string taxType { get; set; }
		public string taxSubTypeId { get; set; }
		public string taxTypeGroupId { get; set; }
		public string taxName { get; set; }
		public int? taxAuthorityTypeId { get; set; }
		public int? taxRegionId { get; set; }
		public decimal? taxCalculated { get; set; }
		public decimal? taxOverride { get; set; }
		public AvalaraRateType? rateType { get; set; }
		public string rateTypeCode { get; set; }
		public decimal? taxableUnits { get; set; }
		public decimal? nonTaxableUnits { get; set; }
		public decimal? exemptUnits { get; set; }
		public string unitOfBasis { get; set; }
		public int? rateRuleId { get; set; }
		public decimal? rate { get; set; }
		public bool? isFee { get; set; }
		public int? nonTaxableRuleId { get; set; }
		public long? id { get; set; }
		public long? transactionLineId { get; set; }
		public long? transactionId { get; set; }
		public long? addressId { get; set; }
		public string country { get; set; }
		public string region { get; set; }
		public string countyFIPS { get; set; }
		public string stateFIPS { get; set; }
		public decimal? exemptAmount { get; set; }
		public int? exemptReasonId { get; set; }
		public bool? inState { get; set; }
		public string jurisCode { get; set; }
		public string jurisName { get; set; }
		public int? jurisdictionId { get; set; }
		public string signatureCode { get; set; }
		public string stateAssignedNo { get; set; }
		public AvalaraJurisTypeId? jurisType { get; set; }
		public AvalaraJurisdictionType? jurisdictionType { get; set; }
		public decimal? nonTaxableAmount { get; set; }
		public bool? isNonPassThru { get; set; }
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum TaxRuleTypeId
	{
		RateRule = 0,
		RateOverrideRule = 1,
		BaseRule = 2,
		ExemptEntityRule = 3,
		ProductTaxabilityRule = 4,
		NexusRule = 5
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum AvalaraJurisTypeId
	{
		STA = 1,
		CTY = 2,
		CIT = 3,
		STJ = 4,
		CNT = 5
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum AvalaraTaxRuleTypeId
	{
		RateRule = 0,
		RateOverrideRule = 1,
		BaseRule = 2,
		ExemptEntityRule = 3,
		ProductTaxabilityRule = 4,
		NexusRule = 5
	}
}
