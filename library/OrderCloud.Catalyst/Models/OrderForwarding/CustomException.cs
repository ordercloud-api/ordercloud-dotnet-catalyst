using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace OrderCloud.Catalyst.Models.OrderForwarding
{
    public class CustomException : Exception
    {
        public string ErrorCode { get; set; }
        public HttpStatusCode? HttpStatus { get; set; }

        public CustomException(CustomError error, HttpStatusCode httpStatus) : base(error.message)
        {
            ErrorCode = error.code;
            HttpStatus = httpStatus;
        }
    }

    public class CustomError
    {
        public string code { get; set; }
        public string message { get; set; }
    }
}
