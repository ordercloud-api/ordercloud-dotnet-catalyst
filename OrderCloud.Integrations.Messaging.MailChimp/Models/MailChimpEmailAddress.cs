using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Messaging.MailChimp
{
	public class MailChimpEmailAddress
	{
		public string email { get; set; }
		public string name { get; set; }
		/// <summary>
		/// "to", "cc", or "bcc"
		/// </summary>
		public string type { get; set; }
	}
}
