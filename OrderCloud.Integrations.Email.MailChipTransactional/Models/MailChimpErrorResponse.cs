using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Messaging.MailChimp
{
	public class MailChimpErrorResponse
	{
		public string status { get; set; }
		public int code { get; set; }
		public string name { get; set; }
		public string message { get; set; }
	}
}
