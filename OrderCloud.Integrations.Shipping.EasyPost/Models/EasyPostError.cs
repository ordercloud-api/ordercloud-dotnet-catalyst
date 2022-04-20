using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.EasyPost
{
	/// <summary>
	/// https://www.easypost.com/docs/api#error-object
	/// </summary>
	public class EasyPostError
	{
		public EasyPostErrorDetails error = new EasyPostErrorDetails();
	}

	public class EasyPostErrorDetails
	{
		public string code { get; set; }
		public string message { get; set; }
		public List<EasyPostFieldError> errors = new List<EasyPostFieldError>();
	}

	public class EasyPostFieldError
	{
		public string field { get; set; }
		public string message { get; set; }
	}
}
