using OrderCloud.Catalyst;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace OrderCloud.Integrations.Email.SendInBlue
{
	public class SendInBlueService : OCIntegrationService, ISingleEmailSender
	{
		private readonly TransactionalEmailsApi _defaultClient; 

		public SendInBlueService(SendInBlueConfig defaultConfig) : base(defaultConfig)
		{
			_defaultClient = new TransactionalEmailsApi();
			Configuration.Default.ApiKey.Add("api-key", defaultConfig.ApiKey);
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
			await client.SendTransacEmailAsync(model);
		}
	}
}
