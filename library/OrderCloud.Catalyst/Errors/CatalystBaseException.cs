using Flurl.Http;
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
		public HttpStatusCode? HttpStatus { get; }
		public ApiError[] Errors { get; }

		internal CatalystBaseException(FlurlCall call, ApiError[] errors) : base(BuildMessage(call, errors), call.Exception)
		{
			HttpStatus = call.HttpResponseMessage.StatusCode;
			Errors = errors;
		}

		private static string BuildMessage(FlurlCall call, ApiError[] errors)
		{
			var code = errors?.FirstOrDefault()?.ErrorCode;
			var msg = errors?.FirstOrDefault()?.Message;
			if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(msg))
				return $"{code}: {msg}";

			return new[] { code, msg, call?.Exception?.Message, "An unknown error occurred." }
				.FirstOrDefault(x => !string.IsNullOrEmpty(x));
		}
	}

	internal class ApiErrorResponse
	{
		public ApiError[] Errors { get; set; }
	}

	internal class AuthErrorResponse
	{
		public string error { get; set; }
		public string error_description { get; set; }
	}
}
