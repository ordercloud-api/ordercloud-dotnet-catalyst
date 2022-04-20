using System;
using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Shipping.UPS
{
	public class UPSConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "UPS";
		[RequiredIntegrationField]
		public string BaseUrl { get; set; } // "https://onlinetools.ups.com/ship/v1"
		[RequiredIntegrationField]
		public string ApiKey { get; set; }
	}
}
