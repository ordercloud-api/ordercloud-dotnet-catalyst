using OrderCloud.Catalyst;
using System;

namespace OrderCloud.Integrations.Payment.CardConnect
{
	public class CardConnectConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "CardConnect";
		[RequiredIntegrationField]
		public string BaseUrl { get; set; } // https://<site>.cardconnect.com/cardconnect/rest or https://<site>-uat.cardconnect.com/cardconnect/rest
		[RequiredIntegrationField]
		public string APIUsername { get; set; }
		[RequiredIntegrationField]
		public string APIPassword { get; set; }
		[RequiredIntegrationField]
		public string MerchantId { get; set; }
	}
}