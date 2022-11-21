using OrderCloud.Catalyst;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderCloud.Integrations.Email.SendInBlue
{
	public static class SendInBlueMapper
	{
		public static SendSmtpEmail ToSendInBlueSendSmtpEmail(EmailMessage email)
		{
			var model = new SendSmtpEmail()
			{
				HtmlContent = email.Content,
				Subject = email.Subject,
				Sender = new SendSmtpEmailSender()
				{
					Name = email.FromAddress.Name,
					Email = email.FromAddress.Email
				},
				Attachment = email.Attachments.Select(ToSendInBlueAttachment).ToList(),
				TemplateId = long.Parse(email.TemplateID),
				Params = email.GlobalTemplateData
			};

			if (email.AllRecipientsVisibleOnSingleThread)
			{
				model.To = email.ToAddresses.Select(ToSendSmtpEmailTo).ToList();
			}
			else
			{
				model.MessageVersions = email.ToAddresses.Select(ToMessageVersion).ToList();
			}

			return model;
		}

		public static SendSmtpEmailMessageVersions ToMessageVersion(ToEmailAddress address)
		{
			return new SendSmtpEmailMessageVersions()
			{
				To = new List<SendSmtpEmailTo1>() { new SendSmtpEmailTo1(address.Email, address.Name) },
				Params = address.TemplateDataOverrides.ToDictionary(pair => pair.Key, pair => (object)pair.Value)
			};
		}

		public static SendSmtpEmailTo ToSendSmtpEmailTo(ToEmailAddress address)
		{
			return new SendSmtpEmailTo(address.Email, address.Name);
		}

		public static SendSmtpEmailAttachment ToSendInBlueAttachment(EmailAttachment attachment)
		{

			return new SendSmtpEmailAttachment(
				null, 
				Convert.FromBase64String(attachment.ContentBase64Encoded), 
				attachment.FileName);
		}
	}
}
