using OrderCloud.SDK;
using System;

namespace OrderCloud.Catalyst
{
	public class CatalystBaseException : Exception
	{
		public ApiError ApiError { get; }
		public int HttpStatus { get; set; }

		public CatalystBaseException(string errorCode, int httpStatus, string message, object data = null)
		{
			HttpStatus = httpStatus;
			ApiError = new ApiError
			{
				ErrorCode = errorCode,
				Message = message,
				Data = data
			};
		}
	}

	public class ApiError<T> : ApiError 
	{
		public new T Data { get; set; }
	}
}
