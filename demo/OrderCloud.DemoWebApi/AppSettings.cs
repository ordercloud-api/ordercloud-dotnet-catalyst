using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloud.DemoWebApi
{
	public class AppSettings
	{
		public OrderCloudSettings OrderCloudSettings { get; set; } = new OrderCloudSettings();
		public EnvironmentSettings EnvironmentSettings { get; set; } = new EnvironmentSettings();
		public RedisSettings RedisSettings { get; set; } = new RedisSettings();
	}

	public class RedisSettings
	{
		public string ConnectionString { get; set; }
		public int DatabaseID { get; set; }
	}

	public class EnvironmentSettings
	{
		public string BuildNumber { get; set; }
	}

	public class OrderCloudSettings
	{
		public string ApiUrl { get; set; }
		public string MiddlewareClientID { get; set; }
		public string MiddlewareClientSecret { get; set; }
		public string WebhookHashKey { get; } = "myhashkey"; //	Should match the HashKey configured on your webhook in the Ordercloud portal.
	}
}
