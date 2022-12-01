using OrderCloud.Catalyst;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmailAddress = OrderCloud.Catalyst.EmailAddress;
using SendGridEmailAddress = SendGrid.Helpers.Mail.EmailAddress;

namespace OrderCloud.Integrations.Messaging.SendGrid
{
	public static class SendGridSingleEmailMessageMapper
	{
		public static SendGridMessage ToSendGridMessage(EmailMessage message)
		{
			if (message == null)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(message.Content))
			{
				message.TemplateID = null; // content overrides template
			}
			var isTemplateMessage = !string.IsNullOrEmpty(message.TemplateID);
			var oneThread = message.AllRecipientsVisibleOnSingleThread;
			List<Personalization> personalizations;
			if (oneThread)
			{
				personalizations = new List<Personalization>()
				{
					new Personalization()
					{
						Tos = message.ToAddresses?.Select(ToSendGridEmailAddress)?.ToList(),
						TemplateData = isTemplateMessage ? message.GlobalTemplateData : null,
					}
				};
			} else
			{
				personalizations = message.ToAddresses?.Select(to => ToSendGridPersonalization(isTemplateMessage, message.GlobalTemplateData, to))?.ToList();
			}

			var attachments = message.Attachments?.Select(ToSendGridAttachment)?.ToList();

			var sendGridMessage = new SendGridMessage()
			{
				Personalizations = personalizations,
				From = ToSendGridEmailAddress(message.FromAddress),
				Subject = message.Subject,
				HtmlContent = message.Content,
				Attachments = attachments.Count > 0 ? attachments : null,
				TemplateId = message.TemplateID,
			};

			return sendGridMessage;
		}

		private static Personalization ToSendGridPersonalization(bool isTemplateMessage, Dictionary<string, object> globalTemplateData, ToEmailAddress email)
		{
			if (email == null) return null;
			if (isTemplateMessage)
			{
				foreach (var globalEntry in globalTemplateData)
				{
					if (!email.TemplateDataOverrides.TryGetValue(globalEntry.Key, out var value))
					{
						email.TemplateDataOverrides[globalEntry.Key] = globalEntry.Value;
					}
				} 
			} else
			{
				email.TemplateDataOverrides = null;
			}

			return new Personalization()
			{
				Tos = new List<SendGridEmailAddress> { ToSendGridEmailAddress(email) },
				TemplateData = email.TemplateDataOverrides
			};
		}


		private static SendGridEmailAddress ToSendGridEmailAddress(EmailAddress address)
		{
			if (address == null) return null;
			return new SendGridEmailAddress()
			{
				Email = address.Email,
				Name = address.Name,
			};
		}

		private static Attachment ToSendGridAttachment(EmailAttachment attachment)
		{
			if (attachment == null) return null;
			return new Attachment()
			{
				Content	= attachment.ContentBase64Encoded,
				Type = attachment.MIMEType,
				Filename = attachment.FileName,
				Disposition = "attachment"
			};
		}
	}
}
