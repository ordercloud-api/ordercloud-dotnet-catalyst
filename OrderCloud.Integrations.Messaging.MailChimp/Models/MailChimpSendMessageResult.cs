using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Messaging.MailChimp
{
	public class MailChimpSendMessageResult
	{
		public string email { get; set; }
		/// <summary>
		/// "sent", "queued", "scheduled", "rejected", or "invalid"
		/// </summary>
		public string status { get; set; }
		public string reject_reason { get; set; }
		public string queued_reason { get; set; }
		public string _id { get; set; }
	}
}
