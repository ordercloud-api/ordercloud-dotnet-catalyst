using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Email.SendGrid
{
	public class SendGridConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "SendGrid";
		[RequiredIntegrationField]
		public string ApiKey { get; set; }
	}
}