using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Messaging.MailChimp
{
	public class MailChimpMergeVar
	{
		public string name { get; set; }
		public string content { get; set; }
	}

	public class MailChimpPersonalMergeVars
	{
		public string rcpt { get; set; }
		public List<MailChimpMergeVar> vars { get; set; } = new List<MailChimpMergeVar>();
	}

	public class MailChimpPersonalMetadata
	{
		public string rcpt { get; set; }
		public object values { get; set; }
	}
}
