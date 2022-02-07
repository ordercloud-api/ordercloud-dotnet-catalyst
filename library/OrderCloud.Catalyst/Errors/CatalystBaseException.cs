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
		public HttpStatusCode HttpStatus { get; set; }

		public IList<ApiError> Errors { get; }

		public CatalystBaseException(ApiError apiError, HttpStatusCode httpStatus = HttpStatusCode.BadRequest)
			: this(apiError.ErrorCode, apiError.Message, apiError.Data, httpStatus) { }


		public CatalystBaseException(IList<ApiError> errors, HttpStatusCode httpStatus = HttpStatusCode.BadRequest)
		{
			HttpStatus = httpStatus;
			Require.That(!errors.IsNullOrEmpty(), new Exception("errors collection must contain at least one item."));
			Errors = errors;
		}

		public CatalystBaseException(string errorCode, string message, object data = null, HttpStatusCode httpStatus = HttpStatusCode.BadRequest)
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
			: this(errorCode.Code, errorCode.DefaultMessage, data, errorCode.HttpStatus) { }



		// Keeping these depreacated constructors that take an Int for backwards compatibility.
		//public CatalystBaseException(ApiError apiError, int httpStatus) : this(apiError, (HttpStatusCode)httpStatus) { }

		//public CatalystBaseException(IList<ApiError> errors, int httpStatus): this(errors, (HttpStatusCode)httpStatus) { }

		//public CatalystBaseException(string errorCode, string message, object data, int httpStatus)
		//	: this(errorCode, message, data, (HttpStatusCode)httpStatus) { }


	}
}
