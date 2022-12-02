using OrderCloud.Catalyst;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderCloud.Integrations.Messaging.SendInBlue
{
	public static class SendInBlueMapper
	{
		public static SendSmtpEmail ToSendInBlueSendSmtpEmail(EmailMessage message)
		{
			if (message == null) return null;
			if (!string.IsNullOrEmpty(message.Content))
			{
				message.TemplateID = null; // Content takes precendence over TemplateID
			}

			var from = new SendSmtpEmailSender(message.FromAddress?.Name, message.FromAddress?.Email);
			var attachments = message.Attachments?.Select(ToSendInBlueAttachment)?.ToList();

			var model = new SendSmtpEmail()
			{
				HtmlContent = message.Content,
				Subject = message.Subject,
				Sender = from,
				Attachment = attachments.Count > 0 ? attachments : null,
				Params = message.GlobalTemplateData
			};

			if (!string.IsNullOrEmpty(message.TemplateID))
			{
				model.TemplateId = long.Parse(message.TemplateID);
			}

			if (message.AllRecipientsVisibleOnSingleThread)
			{
				model.To = message.ToAddresses?.Select(ToSendSmtpEmailTo)?.ToList();
			}
			else
			{
				model.MessageVersions = message.ToAddresses?.Select(ToMessageVersion)?.ToList();
			}

			return model;
		}

		public static SendSmtpEmailMessageVersions ToMessageVersion(ToEmailAddress address)
		{
			if (address?.Email == null) return null;
			var to = new List<SendSmtpEmailTo1>() { new SendSmtpEmailTo1(address.Email, address.Name) };
			return new SendSmtpEmailMessageVersions(to, address.TemplateDataOverrides);
		}

		public static SendSmtpEmailTo ToSendSmtpEmailTo(ToEmailAddress address)
		{
			if (address == null) return null;
			return new SendSmtpEmailTo(address.Email, address.Name);
		}

		public static SendSmtpEmailAttachment ToSendInBlueAttachment(EmailAttachment attachment)
		{
			if (attachment == null) return null;
			return new SendSmtpEmailAttachment(
				null, 
				Convert.FromBase64String(attachment.ContentBase64Encoded), 
				attachment.FileName);
		}
	}
}
