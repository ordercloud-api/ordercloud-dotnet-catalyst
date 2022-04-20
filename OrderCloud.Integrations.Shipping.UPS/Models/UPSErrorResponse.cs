using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.UPS
{
	public class UPSErrorBody
	{
		public UPSErrorResponse response { get; set; } = new UPSErrorResponse();
	}

	public class UPSErrorResponse
	{
		public List<UPSError> errors { get; set; } = new List<UPSError>();
	}

	public class UPSError
	{
		public string code { get; set; }
		public string message { get; set; }
	}
}
