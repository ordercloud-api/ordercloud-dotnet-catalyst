using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class ErrorCode
	{
		public ErrorCode(string code, string defaultMessage, HttpStatusCode httpStatus = HttpStatusCode.BadRequest)
		{
			Code = code;
			DefaultMessage = defaultMessage;
			HttpStatus = httpStatus;
		}

		public string Code { get; set; }
		public HttpStatusCode HttpStatus { get; set; }
		public string DefaultMessage { get; set; }
	}

	public class ErrorCode<TData> : ErrorCode
	{
		public ErrorCode(string code, HttpStatusCode httpStatus, string defaultMessage) : base(code, defaultMessage, httpStatus) { }
	}
}
