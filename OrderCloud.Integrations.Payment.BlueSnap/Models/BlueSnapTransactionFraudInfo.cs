using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class BlueSnapTransactionFraudInfo
	{
		public string fraudSessionId { get; set; }
		public string shopperIpAddress { get; set; }
		public string company { get; set; }
		public string enterpriseSiteId { get; set; }
		public string customerId { get; set; }
		public string customerCreationDate { get; set; }
		public BlueSnapShippingContactInfo shippingContactInfo { get; set; }
		public List<BlueSnapFraudProduct> fraudProducts {get; set;}
	}

	public class BlueSnapFraudProduct
	{
		public string fraudProductName { get; set; }
		public string fraudProductDesc { get; set; }
		public string fraudProductType { get; set; }
		public int fraudProductQuantity { get; set; }
		public decimal fraudProductPrice { get; set; }
	}
}
