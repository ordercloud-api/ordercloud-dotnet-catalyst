using System;
using System.Collections.Generic;
using System.Net;

namespace OrderCloud.Catalyst
{
	public class IntegrationMissingConfigsException : CatalystBaseException 
	{
		public IntegrationMissingConfigsException(OCIntegrationConfig config, List<string> missingFields) : base(
			"IntegrationMissingConfigs",
			$"One or more configuration fields for 3rd party service {config.ServiceName} are null or empty. Requests to the service were not attempted.",
			new IntegrationMissingConfigs()
			{
				ServiceName = config.ServiceName,
				MissingFieldNames = missingFields
			},
			HttpStatusCode.BadRequest) { }
	}

	public class IntegrationMissingConfigs
	{
		public string ServiceName { get; set; }
		public List<string> MissingFieldNames { get; set; } = new List<string>();
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class RequiredIntegrationFieldAttribute : Attribute
	{

	}
}
