using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using OrderCloud.Catalyst.Extensions;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public class ApiErrorException : Exception
	{
		public override string Message => ApiErrors?.FirstOrDefault()?.Message ?? "";

		public int StatusCode =>
			(ApiErrors.IsNullOrEmpty()) ? 500 :
			(ApiErrors.Any(e => e.StatusCode == HttpStatusCode.InternalServerError)) ? 500 :
			(ApiErrors.Any(e => e.StatusCode == HttpStatusCode.BadRequest)) ? 400 :
			(int)ApiErrors.First().StatusCode;

		public IList<ApiError> ApiErrors { get; }

		public ApiErrorException(ErrorCode errorCode, object data)
		{
			ApiErrors = new[] {
				new ApiError {
					ErrorCode = errorCode.Code,
					StatusCode = (HttpStatusCode)errorCode.HttpStatus,
					Message = errorCode.DefaultMessage,
					Data = data
				}
			};
		}

		public ApiErrorException(IList<ApiError> errors)
		{
			if (errors.IsNullOrEmpty())
				throw new Exception("errors collection must contain at least one item.");
			ApiErrors = errors;
		}

		protected ApiErrorException(string errorCode, int status, string message, object data)
		{
			ApiErrors = new[] {
				new ApiError {
					ErrorCode = errorCode,
					StatusCode = (HttpStatusCode)status,
					Message = message,
					Data = data
				}
			};
		}
	}
	public class UnAuthorizedException : ApiErrorException
	{
        public UnAuthorizedException() : base("Authorization.InvalidToken", 401, "Access token is invalid or expired.", null) { }
    }
	public class WebhookUnauthorizedException : ApiErrorException
	{
		public WebhookUnauthorizedException() : base("Authorization.Unauthorized", 401, "X-oc-hash header does not match. Endpoint can only be hit from a valid OrderCloud webhook.", null) { }
	}

	public class InsufficientRolesException : ApiErrorException
	{
        public InsufficientRolesException(InsufficientRolesError data) : base("InsufficientRoles", 403, "User does not have role(s) required to perform this action.", data) { }
    }

    public class RequiredFieldException : ApiErrorException
	{
        public RequiredFieldException(string fieldName) : base("RequiredField", 400, $"Field {fieldName} is required", null) { }
    }

    public class NotFoundException : ApiErrorException
	{
        public NotFoundException() : base("NotFound", 404, $"Not found.", null) { }

        public NotFoundException(string thingName, string  thingID) : base("NotFound", 404, "Not Found.", new { ObjectType = thingName, ObjectID = thingID }) { }
    }

    public class InvalidPropertyException : ApiErrorException
	{
        public InvalidPropertyException(Type type, string name) : base("InvalidProperty", 400, $"{type.Name}.{name}", null) { }
    }

    public class UserErrorException : ApiErrorException
	{
        public UserErrorException(string message) : base("InvalidRequest", 400, message, null) { }
    }

    public class UserContextException : ApiErrorException
	{
        public UserContextException(string message) : base("UserContextError", 400, message, null) { }
    }

    public class InsufficientRolesError
    {
        public IList<string> SufficientRoles { get; set; }
        public IList<string> AssignedRoles { get; set; }
    }
}
