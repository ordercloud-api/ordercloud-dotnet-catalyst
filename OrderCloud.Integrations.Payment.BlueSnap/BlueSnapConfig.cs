using OrderCloud.Catalyst;
using System;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class BlueSnapConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "BlueSnap";
		[RequiredIntegrationField]
		public string BaseUrl { get; set; } // https://sandbox.bluesnap.com or https://ws.bluesnap.com
		[RequiredIntegrationField]
		public string APIUsername { get; set; }
		[RequiredIntegrationField]
		public string APIPassword { get; set; }
	}
}
