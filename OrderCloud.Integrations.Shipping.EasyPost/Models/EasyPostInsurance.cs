using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Shipping.EasyPost
{
	// https://www.easypost.com/docs/api#insurance-object
	public class EasyPostInsurance
	{
		public string id { get; set; }
		public string mode { get; set; }
		public string reference { get; set; }
		public string amount { get; set; }
		public string provider { get; set; }
		public string provider_id { get; set; }
		public string shipment_id { get; set; }
		public string tracking_code { get; set; }
		public string status { get; set; }
		// public tracker
		public EasyPostAddress to_address { get; set; }
		public EasyPostAddress from_address { get; set; }
		public EasyPostFee fee { get; set; }
		public List<string> messages { get; set; }
		public DateTime created_at { get; set; }
		public DateTime updated_at { get; set; }
	}

	public class EasyPostFee
	{
		public string type { get; set; }
		public string amount { get; set; }
		public bool charged { get; set; }
		public bool refunded { get; set; }
	}
}
