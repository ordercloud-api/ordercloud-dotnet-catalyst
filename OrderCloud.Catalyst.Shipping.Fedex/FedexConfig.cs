using System;

namespace OrderCloud.Catalyst.Shipping.Fedex
{
	public class FedexConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "Fedex";
		public string BaseUrl { get; set; } // https://apis-sandbox.fedex.com or https://apis.fedex.com
		public string ClientID { get; set; }
		public string ClientSecret { get; set; }
		public string AccountNumber { get; set; }
	}
}
