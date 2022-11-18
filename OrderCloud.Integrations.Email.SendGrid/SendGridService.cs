using OrderCloud.Catalyst;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Integrations.Email.SendGrid
{
	public class SendGridService : OCIntegrationService, IEmailSender
	{
		private readonly SendGridClient _client;  

		public SendGridService(SendGridConfig defaultConfig) : base(defaultConfig) 
		{
			_client = new SendGridClient(defaultConfig.ApiKey);
		}

		/// <summary>
		/// See https://docs.sendgrid.com/api-reference/mail-send/mail-send
		/// </summary>
		public async Task SendEmailAsync(EmailMessage message, OCIntegrationConfig overrideConfig = null)
		{
			var client = _client;
			if (overrideConfig != null)
			{
				var config = ValidateConfig<SendGridConfig>(overrideConfig);
				client = new SendGridClient(config.ApiKey);
			}

			var sendGridModel = SendGridSingleEmailMessageMapper.ToSendGridMessage(message);
			await client.SendEmailAsync(sendGridModel);

			// TODO error handling
		}
	}
}
