using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// A simple email sender. Intended for "Transactional emails", meaning notifications in response to an action taken in an appplication, like reset password or order submitted. This is as opposed to batches or marketing campaigns.
	/// </summary>
	public interface ISingleEmailSender
	{
		/// <summary>
		/// Send a single email, as opposed to a batch of emails or an email campaign. Multiple recipients with personalized templates supported.
		/// </summary>
		Task SendSingleEmailAsync(EmailMessage message, OCIntegrationConfig overrideConfig = null);
	}


	/// <summary>
	/// Email Address
	/// </summary>
	public class EmailAddress
	{
		public EmailAddress() { }
		public EmailAddress(string email)
		{
			Email = email;
		}

		/// <summary>
		/// The actual Email address. Required.
		/// </summary>
		public string Email { get; set; }
		/// <summary>
		/// Name of the address. Optional.
		/// </summary>
		public string Name { get; set; }
	}

	/// <summary>
	/// Email Address
	/// </summary>
	public class ToEmailAddress : EmailAddress
	{
		public ToEmailAddress() { }
		public ToEmailAddress(string email)
		{
			Email = email;
		}

		public ToEmailAddress(EmailAddress email)
		{
			Email = email.Email;
			Name = email.Name;
		}
		/// <summary>
		/// Dynamic data specific to this recipient used to populate the template. Overrides values in EmailMessage.GlobalTemplateData. Ignored if AllRecipientsVisibleOnSingleThread is true. Optional.
		/// </summary>
		public Dictionary<string, object> TemplateDataOverrides { get; set; } = new Dictionary<string, object>();
	}


	/// <summary>
	/// All the data to send an email. Supports multiple recipients, templatization by recipient, attachments, and either one shared thread or individual messages.
	/// </summary>
	public class EmailMessage
	{
		/// <summary>
		/// Subject line of the email
		/// </summary>
		public string Subject { get; set; }
		/// <summary>
		/// Contents of the email. Can be null if the TemplateID property is not null. Will override TemplateID if both are non-null. Assumed to be in HTML format, though a raw string should work. 
		/// </summary>
		public string Content { get; set; }
		/// <summary>
		/// From address of the email. 
		/// </summary>
		public EmailAddress FromAddress { get; set; }
		/// <summary>
		/// List of addresses to send the email to.
		/// </summary>
		public List<ToEmailAddress> ToAddresses { get; set; } = new List<ToEmailAddress>();
		/// <summary>
		/// Reference to an existing email content template in the email automation system. Can be null if the Content property is not null. Will be overriden by Content if both are non-null.
		/// </summary>
		public string TemplateID { get; set; }
		/// <summary>
		/// Dynamic data used to populate the template. Key value strings only. Optional.
		/// </summary>
		public Dictionary<string, object> GlobalTemplateData { get; set; } = new Dictionary<string, object>();
		/// <summary>
		/// List of files to attach to the email. Optional.
		/// </summary>
		public List<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
		/// <summary>
		/// If true, all recipients will see each other on a single thread and ToEmailAddress.TemplateDataOverrides will do nothing. If false, recipients will recieve personal emails. Defaults to false.
		/// </summary>
		public bool AllRecipientsVisibleOnSingleThread { get; set; } = false;
	}

	public class EmailAttachment
	{
		public EmailAttachment() { }

		/// <summary>
		/// Do not use with large files as all the data is put into a byte[] in memory
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public EmailAttachment(IFormFile file)
		{
			MIMEType = file.ContentType;
			FileName = file.FileName;
			ContentBase64Encoded = Convert.ToBase64String(file.ToByteArray());
		}

		/// <summary>
		/// The data for the attached file in Base64Encoded format
		/// </summary>
		public string ContentBase64Encoded { get; set; }
		/// <summary>
		/// Multipurpose Internet Mail Extensions type. For example, "text/html", "image/png", ect.
		/// </summary>
		public string MIMEType { get; set; }
		/// <summary>
		/// The name of the file to display
		/// </summary>
		public string FileName { get; set; }
	}
}
