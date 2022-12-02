using Newtonsoft.Json;
using OrderCloud.Catalyst;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Integrations.Messaging.SendGrid
{
	public class SendGridService : OCIntegrationService, ISingleEmailSender
	{
		protected readonly SendGridClient _client;
		protected readonly string URL = "https://api.sendgrid.com/v3/mail/send";

		public SendGridService(SendGridConfig defaultConfig) : base(defaultConfig) 
		{
			_client = new SendGridClient(defaultConfig.ApiKey);
		}

		/// <summary>
		/// See https://docs.sendgrid.com/api-reference/mail-send/mail-send
		/// </summary>
		public async Task SendSingleEmailAsync(EmailMessage message, OCIntegrationConfig overrideConfig = null)
		{
			var client = _client;
			if (overrideConfig != null)
			{
				var config = ValidateConfig<SendGridConfig>(overrideConfig);
				client = new SendGridClient(config.ApiKey);
			}

			var sendGridModel = SendGridSingleEmailMessageMapper.ToSendGridMessage(message);
			var response = await client.SendEmailAsync(sendGridModel);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new IntegrationAuthFailedException(overrideConfig ?? _defaultConfig, URL, 401);
			}
			else if (!response.IsSuccessStatusCode)
			{
				var jsonString = await response.Body.ReadAsStringAsync();
				var body = JsonConvert.DeserializeObject<SendGridErrorResponse>(jsonString);
				throw new IntegrationErrorResponseException(overrideConfig ?? _defaultConfig, URL, (int)response.StatusCode, body);
			}
		}
	}
}
