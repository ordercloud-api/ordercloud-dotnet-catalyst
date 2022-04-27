using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class BlueSnapShippingContactInfo
	{
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string address1 { get; set; }
		public string address2 { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string zip { get; set; }
		public string country { get; set; }
	}
}
