using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class IntegrationNoResponseException : CatalystBaseException
	{
		public IntegrationNoResponseException(OCIntegrationConfig config, string requestUrl) : base(
				"IntegrationNoResponse",
				$"Request to 3rd party service \"{config.ServiceName}\" returned no response.",
				new IntegrationNoResponseError()
				{
					ServiceName = config.ServiceName,
					RequestUrl = requestUrl,
				}
				, 400)
			{ }
	}

	public class IntegrationNoResponseError
	{
		public string ServiceName { get; set; }
		public string RequestUrl { get; set; }
	}
}
