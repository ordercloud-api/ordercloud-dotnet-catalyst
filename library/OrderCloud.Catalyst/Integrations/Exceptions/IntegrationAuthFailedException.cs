using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class IntegrationAuthFailedException : CatalystBaseException
	{
		public IntegrationAuthFailedException(OCIntegrationConfig config, string requestUrl) : base(
			"IntegrationAuthorizationFailed",
			$"Authentication to 3rd party service \"{config.ServiceName}\" failed. Check your config credentials.",
			new IntegrationAuthFailedError() 
			{
				ServiceName = config.ServiceName,
				RequestUrl = requestUrl,
			}, 
			400) {}
	}

	public class IntegrationAuthFailedError
	{
		public string ServiceName { get; set; }
		public string RequestUrl { get; set; }
	}
}
