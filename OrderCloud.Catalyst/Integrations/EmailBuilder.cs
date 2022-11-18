
using System.Collections.Generic;
using System.Linq;

namespace OrderCloud.Catalyst
{
	public static class EmailBuiler
	{
		public static EmailMessage BuildEmail(string to, string from, string subject, string content, List<EmailAttachment> attachments = null)
		{
			return BuildEmail(new ToEmailAddress(to), new EmailAddress(from), subject, content, attachments);
		}

		public static EmailMessage BuildEmail(ToEmailAddress to, EmailAddress from, string subject, string content, List<EmailAttachment> attachments = null)
		{
			var list = new List<ToEmailAddress>() { to };
			return BuildEmailToMany(list, from, subject, content, attachments);
		}

		public static EmailMessage BuildTemplateEmail(string to, string from, string subject, string templateID, Dictionary<string, string> templateData, List<EmailAttachment> attachments = null)
		{
			return BuildTemplateEmail(new ToEmailAddress(to), new EmailAddress(from), subject, templateID, templateData, attachments);
		}

		public static EmailMessage BuildTemplateEmail(ToEmailAddress to, EmailAddress from, string subject, string templateID, Dictionary<string, string> templateData, List<EmailAttachment> attachments = null)
		{
			var list = new List<ToEmailAddress>() { to };
			return BuildTemplateEmailToMany(list, from, subject, templateID, templateData, attachments);
		}

		public static EmailMessage BuildEmailToMany(List<string> to, string from, string subject, string content, List<EmailAttachment> attachments = null)
		{
			var list = to.Select(x => new ToEmailAddress(x)).ToList();
			return BuildEmailToMany(list, new EmailAddress(from), subject, content, attachments);
		}

		public static EmailMessage BuildEmailToMany(List<ToEmailAddress> to, EmailAddress from, string subject, string content, List<EmailAttachment> attachments = null)
		{
			return new EmailMessage()
			{
				Subject = subject,
				Content = content,
				FromAddress = from,
				ToAddresses = to,
				Attachments = attachments ?? new List<EmailAttachment>()
			};
		}

		public static EmailMessage BuildTemplateEmailToMany(List<string> to, string from, string subject, string templateID, Dictionary<string, string> templateData, List<EmailAttachment> attachments = null)
		{
			var list = to.Select(x => new ToEmailAddress(x)).ToList();
			return BuildTemplateEmailToMany(list, new EmailAddress(from), subject, templateID, templateData, attachments);
		}

		public static EmailMessage BuildTemplateEmailToMany(List<ToEmailAddress> to, EmailAddress from, string subject, string templateID, Dictionary<string, string> templateData, List<EmailAttachment> attachments = null)
		{
			var message = BuildEmailToMany(to, from, subject, null, attachments);
			message.TemplateID = templateID;
			message.GlobalTemplateData = templateData;
			return message;
		}

		public static EmailMessage BuildSharedThreadEmailToMany(List<string> to, string from, string subject, string content, List<EmailAttachment> attachments = null)
		{
			var list = to.Select(x => new EmailAddress(x)).ToList();
			return BuildSharedThreadEmailToMany(list, new EmailAddress(from), subject, content, attachments);
		}

		public static EmailMessage BuildSharedThreadEmailToMany(List<EmailAddress> to, EmailAddress from, string subject, string content, List<EmailAttachment> attachments = null)
		{
			var list = to.Select(x => new ToEmailAddress(x)).ToList();
			var message = BuildEmailToMany(list, from, subject, content, attachments);
			message.AllRecipientsVisibleOnSingleThread = true;
			return message;
		}

		public static EmailMessage BuildSharedThreadTemplateEmailToMany(List<string> to, string from, string subject, string templateID, Dictionary<string, string> templateData, List<EmailAttachment> attachments = null)
		{
			var list = to.Select(x => new EmailAddress(x)).ToList();
			return BuildSharedThreadTemplateEmailToMany(list, new EmailAddress(from), subject, templateID, templateData, attachments);
		}

		public static EmailMessage BuildSharedThreadTemplateEmailToMany(List<EmailAddress> to, EmailAddress from, string subject, string templateID, Dictionary<string, string> templateData, List<EmailAttachment> attachments = null)
		{
			var list = to.Select(x => new ToEmailAddress(x)).ToList();
			var message = BuildTemplateEmailToMany(list, from, subject, templateID, templateData, attachments);
			message.AllRecipientsVisibleOnSingleThread = true;
			return message;
		}
	}
}
