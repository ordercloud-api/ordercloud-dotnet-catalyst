using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class IntegrationErrorResponseException : CatalystBaseException 
	{
		public IntegrationErrorResponseException(OCIntegrationConfig config, string requestUrl, object responseBody) : base(
			"IntegrationErrorResponse",
			$"Request to 3rd party service \"{config.ServiceName}\" resulted in an error. See body for details.",
			new IntegrationErrorResponseError()
			{
				ServiceName = config.ServiceName,
				RequestUrl = requestUrl,
				ResponseBody = responseBody
			}
			, 400)
		{ }
	}

	public class IntegrationErrorResponseError
	{
		public string ServiceName { get; set; }
		public string RequestUrl { get; set; }
		public object ResponseBody { get; set; }
	}
}
