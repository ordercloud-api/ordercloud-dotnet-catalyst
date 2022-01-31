using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class UnauthorizedResponseException : CatalystBaseException
	{
		public UnauthorizedResponseException(OCIntegrationConfig config, string requestUrl) : base(
			"IntegrationAuthorizationFailed",
			$"Authentication to 3rd party service \"{config.ServiceName}\" failed. Check your config credentials.",
			new {
				config.ServiceName,
				RequestUrl = requestUrl,
			}, 
			400) {}
	}
}
