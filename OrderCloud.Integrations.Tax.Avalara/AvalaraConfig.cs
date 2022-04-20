using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Tax.Avalara
{
	public class AvalaraConfig: OCIntegrationConfig
	{
		public override string ServiceName { get; } = "Avalara";
		[RequiredIntegrationField]
		public string BaseUrl { get; set; }
		[RequiredIntegrationField]
		public string AccountID { get; set; }
		[RequiredIntegrationField]
		public string LicenseKey { get; set; }
		[RequiredIntegrationField]
		public string CompanyCode { get; set; }
	}
}
