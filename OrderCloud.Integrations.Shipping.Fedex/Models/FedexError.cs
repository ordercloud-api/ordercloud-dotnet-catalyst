using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.Fedex
{
	public class FedexError
	{
		public string transactionId { get; set; }
		public List<FedexErrorDetails> errors { get; set; } = new List<FedexErrorDetails>();
	}

	public class FedexErrorDetails
	{
		public string code { get; set; }
		public string message { get; set; }
	}
}