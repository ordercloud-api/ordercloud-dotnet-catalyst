using OrderCloud.SDK;
using System;

namespace OrderCloud.Catalyst
{
	public class CatalystBaseException : Exception
	{
		public ApiError ApiError { get; }
		public int HttpStatus { get; set; }

		public CatalystBaseException(string errorCode, string message, object data = null)
		{
			HttpStatus = 400;
			ApiError = new ApiError
			{
				ErrorCode = errorCode,
				Message = message,
				Data = data
			};
		}

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

		public CatalystBaseException(ApiError apiError)
		{
			HttpStatus = 400;
			ApiError = new ApiError
			{
				ErrorCode = apiError.ErrorCode,
				Message = apiError.Message,
				Data = apiError.Data
			};
		}
	}

	public class ApiError<T> : ApiError 
	{
		public new T Data { get; set; }
	}
}
