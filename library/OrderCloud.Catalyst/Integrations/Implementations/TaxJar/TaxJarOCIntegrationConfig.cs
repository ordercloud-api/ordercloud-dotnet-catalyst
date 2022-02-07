using System;

namespace OrderCloud.Catalyst
{
	public class TaxJarOCIntegrationConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "TaxJar";
		[RequiredIntegrationField]
		public string BaseUrl { get; set; }
		[RequiredIntegrationField]
		public string APIToken { get; set; }
	}
}
