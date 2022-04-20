using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.Fedex
{
	public class FedexTokenResponse
	{
		public string access_token { get; set; }
		public string token_type { get; set; }
		public string scope { get; set; }
		public int expires_in { get; set; }
	}
}
