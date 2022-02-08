using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class AvalaraConfig: OCIntegrationConfig
	{
		public override string ServiceName { get; } = "Avalara";
		[RequiredIntegrationField]
		public string BaseUrl { get; set; }
		[RequiredIntegrationField]
		public int AccountID { get; set; }
		[RequiredIntegrationField]
		public string LicenseKey { get; set; }
		[RequiredIntegrationField]
		public int CompanyID { get; set; }
		[RequiredIntegrationField]
		public string CompanyCode { get; set; }
	}
}
