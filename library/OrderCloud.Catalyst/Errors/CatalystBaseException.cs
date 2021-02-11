using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Errors
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
}
