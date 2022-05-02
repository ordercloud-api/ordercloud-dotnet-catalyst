using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/card-holder-info
	/// </summary>
	public class BlueSnapCardHolderInfo
	{
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string email { get; set; }
		public string country { get; set; }
		public string state { get; set; }
		public string address { get; set; }
		public string address2 { get; set; }
		public string city { get; set; }
		public string zip { get; set; }
		public string phone { get; set; }
		public string merchantShopperId { get; set; }
		public string personalIdentificationNumber { get; set; }
		public string companyName { get; set; }
	}
}
