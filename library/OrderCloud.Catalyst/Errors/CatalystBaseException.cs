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
		public int StatusCode =>
            Errors.IsNullOrEmpty() ? 500 :
            (Errors.Any(e => e.StatusCode == HttpStatusCode.InternalServerError)) ? 500 :
			(Errors.Any(e => e.StatusCode == HttpStatusCode.BadRequest)) ? 400 :
			(int)Errors.First().StatusCode;

		public IList<ApiError> Errors { get; }

		public CatalystBaseException(ErrorCode errorCode, object data = null)
		{
			Errors = new[] {
				new ApiError
				{
					ErrorCode = errorCode.Code,
					StatusCode = (HttpStatusCode)errorCode.HttpStatus,
					Message = errorCode.DefaultMessage,
					Data = data
				}
			};
		}

		public CatalystBaseException(ApiError apiError) : base(apiError.Message)
		{
			Errors = new[] {
				new ApiError
				{
					ErrorCode = apiError.ErrorCode,
					Message = apiError.Message,
					Data = apiError.Data
				}
			};
		}

		public CatalystBaseException(IList<ApiError> errors)
		{
			if (errors.IsNullOrEmpty())
				throw new Exception("errors collection must contain at least one item.");
			Errors = errors;
		}

		protected CatalystBaseException(string errorCode, int status, string message, object data)
		{
			Errors = new[] {
				new ApiError {
					ErrorCode = errorCode,
					StatusCode = (HttpStatusCode)status,
					Message = message,
					Data = data
				}
			};
		}
	}

	public class ApiErrorList : List<ApiError>
	{
		public void Add<TData>(ErrorCode<TData> errorCode, TData data)
		{
			Add(new ApiError
			{
				ErrorCode = errorCode.Code,
				StatusCode = (HttpStatusCode)errorCode.HttpStatus,
				Message = errorCode.DefaultMessage,
				Data = data
			});
		}
		public void AddIf<TData>(bool condition, ErrorCode<TData> errorCode, TData data)
		{
			if (condition) Add(errorCode, data);
		}
		public void AddIf(bool condition, ErrorCode errorCode)
		{
			if (condition)
				Add(new ApiError
				{
					ErrorCode = errorCode.Code,
					StatusCode = (HttpStatusCode)errorCode.HttpStatus,
					Message = errorCode.DefaultMessage
				});
		}
        public void ThrowIfAny()
        {
            if (this.Any()) throw new CatalystBaseException(this);
        }
    }

	public class ApiError
	{
		[JsonIgnore]
		public HttpStatusCode StatusCode { get; set; }
		public string ErrorCode { get; set; }
		public string Message { get; set; }
		public object Data { get; set; }
	}
}
