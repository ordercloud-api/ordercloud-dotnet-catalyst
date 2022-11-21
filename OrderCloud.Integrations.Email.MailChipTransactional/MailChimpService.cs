using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Integrations.Email.MailChimp
{
	public class MailChimpService : OCIntegrationService, ISingleEmailSender
	{
		public MailChimpService(MailChimpConfig defaultConfig) : base(defaultConfig) { }

		public async Task SendSingleEmailAsync(EmailMessage message, OCIntegrationConfig overrideConfig)
		{
			var config = ValidateConfig<MailChimpConfig>(overrideConfig ?? _defaultConfig);

			if (message.Content != null)
			{
				var mailChimpModel = MailChimpSendMessageMapper.ToMailChimpSendMessage(message);
				await MailChimpClient.SendMessageAsync(mailChimpModel, config);
			}
			else
			{
				var mailChimpModel = MailChimpSendMessageMapper.ToMailChimpSendTemplateMessage(message);
				await MailChimpClient.SendTemplateMessageAsync(mailChimpModel, config);
			}
		}
	}
}
