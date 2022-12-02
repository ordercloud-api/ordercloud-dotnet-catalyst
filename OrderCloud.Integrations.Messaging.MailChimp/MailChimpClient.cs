using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Messaging.MailChimp
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
			var url = $"{BaseUrl}/messages/send";
			var results = await PostJsonWithErrorHandlingAsync(new FlurlRequest(url), message, config)
				.ReceiveJson<List<MailChimpSendMessageResult>>();

			if (results.Exists(r => r.status == "rejected" || r.status == "invalid"))
			{
				throw new IntegrationErrorResponseException(config, url, 200, results);
			}

			return results;
		}

		/// <summary>
		/// https://mailchimp.com/developer/transactional/api/messages/send-using-message-template/
		/// </summary>
		public static async Task<List<MailChimpSendMessageResult>> SendTemplateMessageAsync(MailChimpSendTemplateMessage message, MailChimpConfig config)
		{
			message.key = config.TransactionalApiKey;
			var url = $"{BaseUrl}/messages/send-template";
			var results = await PostJsonWithErrorHandlingAsync(new FlurlRequest(url), message, config)
				.ReceiveJson<List<MailChimpSendMessageResult>>();

			if (results.Exists(r => r.status == "rejected" || r.status == "invalid"))
			{
				throw new IntegrationErrorResponseException(config, url, 200, results);
			}

			return results;
		}

		protected static async Task<IFlurlResponse> PostJsonWithErrorHandlingAsync(IFlurlRequest request, object data, OCIntegrationConfig config)
		{
			try
			{
				return await request.PostJsonAsync(data);
			}
			catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
			{
				// candidate for retry here?
				throw new IntegrationNoResponseException(config, request.Url);
			}
			catch (FlurlHttpException ex)
			{
				var status = ex?.Call?.Response?.StatusCode;
				if (status == null) // simulate by putting laptop on airplane mode
				{
					throw new IntegrationNoResponseException(config, request.Url);
				}
				var body = await ex.Call.Response.GetJsonAsync<MailChimpErrorResponse>();
				if (body.message == "You must specify a key value" || body.message == "Invalid API key")
				{
					throw new IntegrationAuthFailedException(config, request.Url, (int)status);
				}
				throw new IntegrationErrorResponseException(config, request.Url, (int)status, body);
			}
		}
	}
}
