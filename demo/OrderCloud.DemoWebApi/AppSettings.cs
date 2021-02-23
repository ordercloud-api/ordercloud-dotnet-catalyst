using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloud.DemoWebApi
{
	public class AppSettings
	{
		public string MyDBConnectionString { get; set; }

		public string WebhookHashKey { get; } = "myhashkey"; //	Should match the HashKey configured on your webhook in the Ordercloud portal.
    }
}
