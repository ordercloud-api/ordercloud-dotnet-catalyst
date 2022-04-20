using System;

namespace OrderCloud.Catalyst.Shipping.Fedex
{
	public class FedexConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "Fedex";
		[RequiredIntegrationField]
		public string BaseUrl { get; set; } // https://apis-sandbox.fedex.com or https://apis.fedex.com
		[RequiredIntegrationField]
		public string ClientID { get; set; }
		[RequiredIntegrationField]
		public string ClientSecret { get; set; }
		[RequiredIntegrationField]
		public string AccountNumber { get; set; }
	}
}
