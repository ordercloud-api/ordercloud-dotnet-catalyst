using System;

namespace OrderCloud.Catalyst.Tax.TaxJar
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
