using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class NoResponseException : CatalystBaseException
	{
		public NoResponseException(OCIntegrationConfig config, string requestUrl) : base(
				"IntegrationNoResponse",
				$"Request to 3rd party service \"{config.ServiceName}\" returned no response.",
				new
				{
					config.ServiceName,
					RequestUrl = requestUrl,
				}
				, 400)
			{ }
	}
}
