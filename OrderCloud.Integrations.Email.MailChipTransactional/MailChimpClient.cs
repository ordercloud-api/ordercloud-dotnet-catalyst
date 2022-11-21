using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Email.MailChimp
{
	public class MailChimpClient
	{
		protected static string BaseUrl = "https://mandrillapp.com/api/1.0";

		/// <summary>
		/// https://mailchimp.com/developer/transactional/api/messages/send-new-message/
		/// </summary>
		public static async Task<List<MailChimpSendMessageResult>> SendMessageAsync(MailChimpSendMessage message, MailChimpConfig config)
		{
			message.key = config.TransactionalApiKey;
			var results = await new FlurlRequest($"{BaseUrl}/messages/send")
				.PostJsonWithErrorHandlingAsync<MailChimpSendMessage>(message, config)
				.ReceiveJson<List<MailChimpSendMessageResult>>();
			return results;
		}

		/// <summary>
		/// https://mailchimp.com/developer/transactional/api/messages/send-using-message-template/
		/// </summary>
		public static async Task<List<MailChimpSendMessageResult>> SendTemplateMessageAsync(MailChimpSendTemplateMessage message, MailChimpConfig config)
		{
			message.key = config.TransactionalApiKey;
			var results = await new FlurlRequest($"{BaseUrl}/messages/send-template")
				.PostJsonWithErrorHandlingAsync<MailChimpSendTemplateMessage>(message, config)
				.ReceiveJson<List<MailChimpSendMessageResult>>();
			return results;
		}
	}
}
