using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Integrations.Messaging.MailChimp
{
	public static class MailChimpSendMessageMapper
	{
		public static MailChimpSendMessage ToMailChimpSendMessage(EmailMessage message)
		{
			return new MailChimpSendMessage()
			{
				message = ToMailChimpTransactionalMessage(message)
			};
		}

		public static MailChimpSendTemplateMessage ToMailChimpSendTemplateMessage(EmailMessage message)
		{
			return new MailChimpSendTemplateMessage()
			{
				template_name = message.TemplateID,
				message = ToMailChimpTransactionalMessage(message)
			};
		}

		public static MailChimpTransactionalMessage ToMailChimpTransactionalMessage(EmailMessage message)
		{
			var mailChimpModel = new MailChimpTransactionalMessage()
			{
				html = message.Content,
				subject = message.Subject,
				from_email = message.FromAddress.Email,
				from_name = message.FromAddress.Name,
				preserve_recipients = message.AllRecipientsVisibleOnSingleThread,
				attachments = message.Attachments.Select(ToMailChimpAttachment).ToList(),
				to = message.ToAddresses.Select(ToMailChimpEmailAddress).ToList(),
				global_merge_vars = ToMailChimpMergeVars(message.GlobalTemplateData)
			};
			if (!message.AllRecipientsVisibleOnSingleThread)
			{
				mailChimpModel.merge_vars = message.ToAddresses.Select(ToMailChimpPersonalMergeVars).ToList();
			}
			return mailChimpModel;
		}

		public static MailChimpAttachment ToMailChimpAttachment(EmailAttachment attachment)
		{
			return new MailChimpAttachment()
			{
				type = attachment.MIMEType,
				name = attachment.FileName,
				content = attachment.ContentBase64Encoded
			};
		}

		public static MailChimpEmailAddress ToMailChimpEmailAddress(ToEmailAddress address)
		{
			return new MailChimpEmailAddress()
			{
				type = "to",
				email = address.Email,
				name = address.Name
			};
		}

		public static List<MailChimpMergeVar> ToMailChimpMergeVars(Dictionary<string, string> templateData)
		{
			return templateData
				.Select(x => new MailChimpMergeVar() { name = x.Key, content = x.Value })
				.ToList();
		}

		public static MailChimpPersonalMergeVars ToMailChimpPersonalMergeVars(ToEmailAddress address)
		{
			return new MailChimpPersonalMergeVars()
			{
				rcpt = address.Email,
				vars = ToMailChimpMergeVars(address.TemplateDataOverrides)
			};
		}
	}
}
