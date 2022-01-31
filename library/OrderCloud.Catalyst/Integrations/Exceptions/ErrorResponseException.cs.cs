using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class ErrorResponseException : CatalystBaseException 
	{
		public ErrorResponseException(OCIntegrationConfig config, string requestUrl, object responseBody) : base(
			"IntegrationErrorResponse",
			$"Request to 3rd party service \"{config.ServiceName}\" resulted in an error. See body for details.",
			new
			{
				config.ServiceName,
				RequestUrl = requestUrl,
				ResponseBody = responseBody
			}
			, 400)
		{ }
	}
}
