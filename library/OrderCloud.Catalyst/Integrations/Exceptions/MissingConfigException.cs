using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class MissingConfigException : CatalystBaseException 
	{
		public MissingConfigException(OCIntegrationConfig config, List<string> missingFields) : base(
			"IntegrationMissingConfig",
			$"Configuration field(s) for 3rd party service \"{config.ServiceName}\" are null or empty. Check fields on class {config.GetType().Name}.",
			new
			{
				config.ServiceName,
				MissingFieldNames = missingFields
			},
			400) { }
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class RequiredIntegrationFieldAttribute : Attribute
	{

	}
}
