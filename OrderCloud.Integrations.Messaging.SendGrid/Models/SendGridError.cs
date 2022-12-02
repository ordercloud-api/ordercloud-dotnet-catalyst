using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Messaging.SendGrid
{
	public class SendGridErrorResponse
	{
		public List<SendGridError> errors { get; set; } = new List<SendGridError>();
	}


	public class SendGridError
	{
		public string message { get; set; }
		public string field { get; set; }
		public string help { get; set; }
	}
}
