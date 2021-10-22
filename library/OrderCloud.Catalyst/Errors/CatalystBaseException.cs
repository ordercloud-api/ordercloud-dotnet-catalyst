using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace OrderCloud.Catalyst
{
	public class CatalystBaseException : Exception
	{
		public override string Message => Errors?.FirstOrDefault()?.Message ?? "";
		public int HttpStatus { get; set; }

		public IList<ApiError> Errors { get; }

		public CatalystBaseException(ApiError apiError, int httpStatus = 400) : base(apiError.Message)
		{
			HttpStatus = httpStatus;
			Errors = new[] {
				new ApiError
				{
					ErrorCode = apiError.ErrorCode,
					Message = apiError.Message,
					Data = apiError.Data
				}
			};
		}

		public CatalystBaseException(IList<ApiError> errors, int httpStatus = 400)
		{
			HttpStatus = httpStatus;
			Require.That(!errors.IsNullOrEmpty(), new Exception("errors collection must contain at least one item."));
			Errors = errors;
		}

		public  CatalystBaseException(string errorCode, string message, object data = null, int httpStatus = 400)
		{
			HttpStatus = httpStatus;
			Errors = new[] {
				new ApiError {
					ErrorCode = errorCode,
					Message = message,
					Data = data
				}
			};
		}

		public CatalystBaseException(ErrorCode errorCode, object data = null)
		{
			HttpStatus = errorCode.HttpStatus;
			Errors = new[] {
				new ApiError {
					ErrorCode = errorCode.Code,
					Message = errorCode.DefaultMessage,
					Data = data
				}
			};
		}
	}
}
