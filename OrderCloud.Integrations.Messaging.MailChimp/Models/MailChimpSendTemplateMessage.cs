using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Messaging.MailChimp
{
	public class MailChimpSendTemplateMessage
	{
		public string key { get; set; }
		public string template_name { get; set; }
		public List<MailChimpPersonalMergeVars> template_content { get; set; } = new List<MailChimpPersonalMergeVars>();
		public bool async { get; set; }
		public string ip_pool { get; set; }
		/// <summary>
		/// YYYY-MM-DD HH:MM:SS format
		/// </summary>
		public string send_at { get; set; }
		public MailChimpTransactionalMessage message { get; set; }
	}
}
