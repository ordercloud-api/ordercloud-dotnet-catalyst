using OrderCloud.Catalyst;
using System;

namespace OrderCloud.Integrations.Email.MailChimp
{
	public class MailChimpConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "MailChimp";

		/// <summary>
		/// https://mailchimp.com/developer/transactional/docs/fundamentals/#authentication. Used to be mandril.
		/// </summary>
		[RequiredIntegrationField]
		public string TransactionalApiKey { get; set; }
	}
}
