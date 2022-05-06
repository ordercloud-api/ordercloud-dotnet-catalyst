using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class BlueSnapError
	{
		public List<BlueSnapErrorDetail> message { get; set; } = new List<BlueSnapErrorDetail>();
	}

	public class BlueSnapErrorDetail
	{
		public string errorName { get; set; }
		public string code { get; set; }
		public string description { get; set; }
	}
}
