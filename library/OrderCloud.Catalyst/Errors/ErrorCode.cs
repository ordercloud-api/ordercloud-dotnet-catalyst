using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class ErrorCode
	{
		public ErrorCode(string code, string defaultMessage, int httpStatus = 400)
		{
			Code = code;
			DefaultMessage = defaultMessage;
			HttpStatus = httpStatus;
		}

		public string Code { get; set; }
		public int HttpStatus { get; set; }
		public string DefaultMessage { get; set; }
	}

	public class ErrorCode<TData> : ErrorCode
	{
		public ErrorCode(string code, int httpStatus, string defaultMessage) : base(code, defaultMessage, httpStatus) { }
	}
}
