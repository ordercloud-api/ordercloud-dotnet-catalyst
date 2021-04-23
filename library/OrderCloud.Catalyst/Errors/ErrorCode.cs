using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public class ErrorCode
	{
		public ErrorCode(string code, int httpStatus, string defaultMessage)
		{
			Code = code;
			HttpStatus = httpStatus;
			DefaultMessage = defaultMessage;
		}

		public string Code { get; set; }
		public int HttpStatus { get; set; }
		public string DefaultMessage { get; set; }
	}

	public class ErrorCode<TData> : ErrorCode
	{
		public ErrorCode(string code, int httpStatus, string defaultMessage) : base(code, httpStatus, defaultMessage) { }
	}
}
