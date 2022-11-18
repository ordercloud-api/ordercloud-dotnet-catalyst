
using System.Collections.Generic;

namespace OrderCloud.Catalyst
{
	public static class SingleEmailMessageBuiler
	{
		public static EmailMessage BuildSingleEmail(ToEmailAddress to, EmailAddress from, string subject, string content) 
		{
			var list = new List<ToEmailAddress>() { to };
			return BuildSingleEmailToMulitpleRecipients(list, from, subject, content);	
		}

		public static EmailMessage BuildSingleTemplateEmail(ToEmailAddress to, EmailAddress from, string subject, string templateID, Dictionary<string, string> templateData)
		{
			var list = new List<ToEmailAddress>() { to };
			return BuildSingleTemplateEmailToMultipleRecipients(list, from, subject, templateID, templateData);
		}

		public static EmailMessage BuildSingleEmailToMulitpleRecipients(List<ToEmailAddress> to, EmailAddress from, string subject, string content)
		{
			return new EmailMessage()
			{
				Subject = subject,
				Content = content,
				FromAddress = from,
				ToAddresses = to
			};
		}

		public static EmailMessage BuildSingleTemplateEmailToMultipleRecipients(List<ToEmailAddress> to, EmailAddress from, string subject, string templateID, Dictionary<string, string> templateData)
		{
			var message = BuildSingleEmailToMulitpleRecipients(to, from, subject, null);
			message.TemplateID = templateID;
			message.GlobalTemplateData = templateData;
			return message;
		}
	}
}
