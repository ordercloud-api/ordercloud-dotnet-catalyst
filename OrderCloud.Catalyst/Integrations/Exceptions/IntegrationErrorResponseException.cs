﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class IntegrationErrorResponseException : CatalystBaseException 
	{
		public IntegrationErrorResponseException(OCIntegrationConfig config, string requestUrl, int responseStatus, object responseBody) : base(
			"IntegrationErrorResponse",
			$"A request to 3rd party service {config.ServiceName} resulted in an error. See ResponseBody for details.",
			new IntegrationErrorResponseError()
			{
				ServiceName = config.ServiceName,
				RequestUrl = requestUrl,
				ResponseStatus = responseStatus,
				ResponseBody = responseBody,
			}
			, HttpStatusCode.BadRequest)
		{ }
	}

	public class IntegrationErrorResponseError
	{
		public string ServiceName { get; set; }
		public string RequestUrl { get; set; }
		public int ResponseStatus { get; set; }
		public object ResponseBody { get; set; }
	}
}
