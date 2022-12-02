using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Messaging.MailChimp
{
	public class MailChimpSendMessage
	{
		public string key { get; set; }
		public bool async { get; set; }
		public string ip_pool { get; set; }
		/// <summary>
		/// YYYY-MM-DD HH:MM:SS format
		/// </summary>
		public string send_at { get; set; }
		public MailChimpTransactionalMessage message { get; set; }
	}

	public class MailChimpTransactionalMessage
	{
		public string html { get; set; }
		public string text { get; set; }
		public string subject { get; set; }
		public string from_email { get; set; }
		public string from_name { get; set; }
		public List<MailChimpEmailAddress> to { get; set; } = new List<MailChimpEmailAddress>();
		public object headers { get; set; }
		public bool important { get; set; }
		public bool track_clicks { get; set; }
		public bool auto_text { get; set; }
		public bool auto_html { get; set; }
		public bool inline_css { get; set; }
		public bool url_strip_qs { get; set; }
		public bool preserve_recipients { get; set; }
		public bool view_content_link { get; set; }
		public string bcc_address { get; set; }
		public string tracking_domain { get; set; }
		public string signing_domain { get; set; }
		public string return_path_domain { get; set; }
		public bool merge { get; set; }
		public string merge_language { get; set; }
		public List<MailChimpMergeVar> global_merge_vars { get; set; } = new List<MailChimpMergeVar>();
		public List<MailChimpPersonalMergeVars> merge_vars { get; set; } = new List<MailChimpPersonalMergeVars>();
		public List<string> tags { get; set; } = new List<string>();
		public string subaccount { get; set; }
		public List<string> google_analytics_domains { get; set; } = new List<string>();
		public string google_analytics_campaign { get; set; }
		public object metadata { get; set; }
		public List<MailChimpPersonalMetadata> recipient_matadata { get; set; } = new List<MailChimpPersonalMetadata>();
		public List<MailChimpAttachment> attachments { get; set; } = new List<MailChimpAttachment>();
		public List<MailChimpAttachment> images { get; set; } = new List<MailChimpAttachment>();
	}
}
