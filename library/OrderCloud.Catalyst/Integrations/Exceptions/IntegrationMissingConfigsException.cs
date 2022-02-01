using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class IntegrationMissingConfigsException : CatalystBaseException 
	{
		public IntegrationMissingConfigsException(OCIntegrationConfig config, List<string> missingFields) : base(
			"IntegrationMissingConfigs",
			$"Configuration field(s) for 3rd party service \"{config.ServiceName}\" are null or empty. Check fields on class {config.GetType().Name}.",
			new IntegrationMissingConfigs()
			{
				ServiceName = config.ServiceName,
				MissingFieldNames = missingFields
			},
			400) { }
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
