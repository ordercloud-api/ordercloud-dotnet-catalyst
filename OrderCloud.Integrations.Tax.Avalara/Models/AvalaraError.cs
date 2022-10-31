using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Tax.Avalara
{
	public class AvalaraError 
	{
		public AvalaraErrorDetail error { get; set; } = new AvalaraErrorDetail();
	}

	public class AvalaraErrorDetail
	{
		public string code { get; set; }
		public string message { get; set; }
		public string target { get; set; }
		public List<AvalaraErrorDetails> details { get; set; } = new List<AvalaraErrorDetails>();
	}

	/// <summary>
	/// https://developer.avalara.com/api-reference/avatax/rest/v2/models/ErrorDetail/
	/// </summary>
	public class AvalaraErrorDetails
	{
		public string code { get; set; }
		public string number { get; set; }
		public string message { get; set; }
		public string description { get; set; }
		public string faultCode { get; set; }
		public string faultSubCode { get; set; }
		public string helpLink { get; set; }
		public string refersTo { get; set; }
		public string severity { get; set; }
	}
}
