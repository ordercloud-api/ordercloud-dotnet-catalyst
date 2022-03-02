using System;
using System.Collections.Generic;

namespace OrderCloud.Catalyst.Shipping.EasyPost
{
	public class EasyPostConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "EasyPost";
		[RequiredIntegrationField]
		public string BaseUrl { get; set; } // https://api.easypost.com/v2 
		[RequiredIntegrationField]
		public string ApiKey { get; set; }
		public List<string> CarrierAccountIDs { get; set; }
	}
}
