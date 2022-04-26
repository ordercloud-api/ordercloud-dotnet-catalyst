using System;
using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Tax.TaxJar
{
	public class TaxJarConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "TaxJar";
		[RequiredIntegrationField]
		public string BaseUrl { get; set; }
		[RequiredIntegrationField]
		public string APIToken { get; set; }
	}
}
