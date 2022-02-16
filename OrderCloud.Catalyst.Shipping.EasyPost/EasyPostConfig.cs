using System;

namespace OrderCloud.Catalyst.Shipping.EasyPost
{
	public class EasyPostConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "EasyPost";
		public string BaseUrl { get; set; } // https://api.easypost.com/v2 
		public string ApiKey { get; set; }
		public List<string> CarrierAccountIDs { get; set; }
	}
}
