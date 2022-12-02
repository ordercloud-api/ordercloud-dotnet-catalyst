using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Messaging.SendGrid
{
	public class SendGridConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "SendGrid";
		[RequiredIntegrationField]
		public string ApiKey { get; set; }
	}
}