using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Shipping.Fedex
{
	public class FedexServiceDescription
	{
		public string serviceId { get; set; }
		public string serviceType { get; set; }
		public string code { get; set; }
		public List<FedexServiceName> name { get; set; } = new List<FedexServiceName>();
		public List<string> operatingOrgCodes { get; set; } = new List<string>();
		public string serviceCategory { get; set; }
		public string description { get; set; }
		public string astraDescription { get; set; }
	}

	public class FedexServiceName
	{
		public string type { get; set; }
		public string encoding { get; set; }
		public string value { get; set; }
	}
}
