using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Tax.Avalara
{ 
	public class AvalaraLineItemModel
	{
		public string exemptionCode { get; set; }
		public AvalaraTaxOverrideModel taxOverride { get; set; }
		public string businessIdentificationNo { get; set; }
		public string description { get; set; }
		public string ref2 { get; set; }
		public string ref1 { get; set; }
		public string revenueAccount { get; set; }
		public bool? taxIncluded { get; set; }
		public bool? discounted { get; set; }
		public string hsCode { get; set; }
		public string itemCode { get; set; }
		public string entityUseCode { get; set; }
		public string customerUsageType { get; set; }
		public string taxCode { get; set; }
		public AvalaraAddressesModel addresses { get; set; }
		public decimal amount { get; set; }
		public decimal? quantity { get; set; }
		public string number { get; set; }
		public List<AvalaraTransactionParameterModel> parameters { get; set; }
	}
}
