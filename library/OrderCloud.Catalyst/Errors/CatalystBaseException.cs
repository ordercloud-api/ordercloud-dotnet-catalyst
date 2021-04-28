using OrderCloud.Catalyst.Extensions;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;

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
			if (errors.IsNullOrEmpty())
				throw new Exception("errors collection must contain at least one item.");
			Errors = errors;
		}

		protected CatalystBaseException(string errorCode, string message, object data, int httpStatus = 400)
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
	}
}
