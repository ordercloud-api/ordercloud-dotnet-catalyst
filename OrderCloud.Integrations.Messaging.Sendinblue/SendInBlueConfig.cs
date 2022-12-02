using OrderCloud.Catalyst;
using System;

namespace OrderCloud.Integrations.Messaging.SendInBlue
{
	public class SendInBlueConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "Sendinblue";
		[RequiredIntegrationField]
		public string ApiKey { get; set; }
	}
}
