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
		[JsonIgnore]
		public int StatusCode { get; set; }

		public IList<ApiError> Errors { get; }

		public CatalystBaseException(ApiError apiError, int status = 400) : base(apiError.Message)
		{
			StatusCode = status;
			Errors = new[] {
				new ApiError
				{
					ErrorCode = apiError.ErrorCode,
					Message = apiError.Message,
					Data = apiError.Data
				}
			};
		}

		public CatalystBaseException(IList<ApiError> errors, int status = 400)
		{
			StatusCode = status;
			if (errors.IsNullOrEmpty())
				throw new Exception("errors collection must contain at least one item.");
			Errors = errors;
		}

		protected CatalystBaseException(string errorCode, string message, object data, int status = 400)
		{
			StatusCode = status;
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
