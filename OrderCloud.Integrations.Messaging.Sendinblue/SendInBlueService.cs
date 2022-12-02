using Newtonsoft.Json;
using OrderCloud.Catalyst;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace OrderCloud.Integrations.Messaging.SendInBlue
{
	public class SendInBlueService : OCIntegrationService, ISingleEmailSender
	{
		protected readonly TransactionalEmailsApi _defaultClient;
		protected readonly string URL = "https://api.sendinblue.com/v3/smtp/email";

		public SendInBlueService(SendInBlueConfig defaultConfig) : base(defaultConfig)
		{
			_defaultClient = new TransactionalEmailsApi();
			Configuration.Default.ApiKey["api-key"] = defaultConfig.ApiKey;
		}

		/// <summary>
		/// See https://developers.sendinblue.com/reference/sendtransacemail
		/// </summary>
		public async Task SendSingleEmailAsync(EmailMessage message, OCIntegrationConfig overrideConfig = null)
		{
			var client = _defaultClient;
			if (overrideConfig != null)
			{
				var config = ValidateConfig<SendInBlueConfig>(overrideConfig);
				client = new TransactionalEmailsApi(new Configuration()
				{
					ApiKey = new Dictionary<string, string>()
					{
						{  "api-key", config.ApiKey }
					}
				});
			}
			
			var model = SendInBlueMapper.ToSendInBlueSendSmtpEmail(message);
			try
			{
				await client.SendTransacEmailAsync(model);
			} catch(ApiException ex) 
			{          
				if (ex.ErrorCode == 401)
				{
					throw new IntegrationAuthFailedException(overrideConfig ?? _defaultConfig, URL, ex.ErrorCode);
				}
				else
				{
					var jsonString = (string)ex.ErrorContent;
					var body = JsonConvert.DeserializeObject<object>(jsonString);
					throw new IntegrationErrorResponseException(overrideConfig ?? _defaultConfig, URL, ex.ErrorCode, body);
				}
			}
		}
	}
}
