using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class AvalaraTaxCode
	{
		public bool? isPhysical { get; set; }
		public int? createdUserId { get; set; }
		public DateTime? createdDate { get; set; }
		public bool? isSSTCertified { get; set; }
		public bool? isActive { get; set; }
		public string entityUseCode { get; set; }
		public long? goodsServiceCode { get; set; }
		public int? modifiedUserId { get; set; }
		public string parentTaxCode { get; set; }
		public string description { get; set; }
		public string taxCodeTypeId { get; set; }
		public string taxCode { get; set; }
		public int? companyId { get; set; }
		public int? id { get; set; }
		public DateTime? modifiedDate { get; set; }
	}
}
