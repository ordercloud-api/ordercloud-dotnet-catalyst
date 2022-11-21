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
			var oneThread = message.AllRecipientsVisibleOnSingleThread;
			List<Personalization> personalizations;
			if (oneThread)
			{
				personalizations = new List<Personalization>() 
				{ 
					new Personalization() 
					{ 
						Tos = message.ToAddresses.Select(ToSendGridEmailAddress).ToList() 
					} 
				}; 
			} else
			{
				personalizations = message.ToAddresses.Select(ToSendGridPersonalization).ToList();
			}

			return new SendGridMessage()
			{
				Personalizations = personalizations,
				From = ToSendGridEmailAddress(message.FromAddress),
				Subject = message.Subject,
				HtmlContent = message.Content,
				Attachments = message.Attachments.Select(ToSendGridAttachment).ToList(),
				TemplateId = message.TemplateID,
			};
		}

		private static Personalization ToSendGridPersonalization(ToEmailAddress email)
		{
			return new Personalization()
			{
				Tos = new List<SendGridEmailAddress> { ToSendGridEmailAddress(email) },
				TemplateData = email.TemplateDataOverrides
			};
		}


		private static SendGridEmailAddress ToSendGridEmailAddress(EmailAddress address)
		{
			return new SendGridEmailAddress()
			{
				Email = address.Email,
				Name = address.Name,
			};
		}

		private static Attachment ToSendGridAttachment(EmailAttachment attachment)
		{
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
