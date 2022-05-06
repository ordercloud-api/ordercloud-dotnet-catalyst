using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class IntegrationAuthFailedException : CatalystBaseException
	{
		public IntegrationAuthFailedException(OCIntegrationConfig config, string requestUrl, int responseStatus) : base(
			"IntegrationAuthorizationFailed",
			$"A request to 3rd party service {config.ServiceName} resulted in an UnAuthorized error. Check your config credentials.",
			new IntegrationAuthFailedError() 
			{
				ServiceName = config.ServiceName,
				RequestUrl = requestUrl,
				ResponseStatus = responseStatus
			},
			HttpStatusCode.BadRequest) {}
	}

	public class IntegrationAuthFailedError
	{
		public string ServiceName { get; set; }
		public string RequestUrl { get; set; }
		public int ResponseStatus { get; set; }
	}
}
