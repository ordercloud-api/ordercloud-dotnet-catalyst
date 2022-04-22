using System;
using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Tax.Vertex
{
	public class VertexConfig : OCIntegrationConfig
	{
		public override string ServiceName { get; } = "Vertex";
		[RequiredIntegrationField]
		public string CompanyName { get; set; }
		[RequiredIntegrationField]
		public string ClientID { get; set; }
		[RequiredIntegrationField]
		public string ClientSecret { get; set; }
		[RequiredIntegrationField]
		public string Username { get; set; }
		[RequiredIntegrationField]
		public string Password { get; set; }
	}
}
