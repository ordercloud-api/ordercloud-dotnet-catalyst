using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public class UnAuthorizedException : CatalystBaseException
	{
		public UnAuthorizedException() : base("InvalidToken", "Access token is invalid or expired.", null, HttpStatusCode.Unauthorized) { }
	}
	public class WebhookUnauthorizedException : CatalystBaseException
	{
		public WebhookUnauthorizedException() : base("Unauthorized", "X-oc-hash header does not match. Endpoint can only be hit from a valid OrderCloud webhook.", null, HttpStatusCode.Unauthorized) { }
	}

	public class InsufficientRolesException : CatalystBaseException
	{
		public InsufficientRolesException(InsufficientRolesError data) : base("InsufficientRoles", "User does not have role(s) required to perform this action.", data, HttpStatusCode.Forbidden) { }
	}

	public class InvalidUserTypeException : CatalystBaseException
	{
		public InvalidUserTypeException(InvalidUserTypeError data) : base("InvalidUserType", $"Users of type {data.ThisUserType} do not have permission to perform this action.", data, HttpStatusCode.Forbidden) { }
	}

	public class RequiredFieldException : CatalystBaseException
	{
		public RequiredFieldException(string fieldName) : base("RequiredField", $"Field {fieldName} is required", null, HttpStatusCode.BadRequest) { }
	}

	public class NotFoundException : CatalystBaseException
	{
		public NotFoundException() : base("NotFound", $"Not found.", null, HttpStatusCode.NotFound) { }

		public NotFoundException(string thingName, string thingID) : base("NotFound", "Not Found.", new { ObjectType = thingName, ObjectID = thingID }, HttpStatusCode.NotFound) { }
	}

	public class InvalidPropertyException : CatalystBaseException
	{
		public InvalidPropertyException(Type type, string name) : base("InvalidProperty", $"{type.Name}.{name}", null, HttpStatusCode.BadRequest) { }
	}

	public class UserErrorException : CatalystBaseException
	{
		public UserErrorException(string message) : base("InvalidRequest", message, null, HttpStatusCode.BadRequest) { }
	}

	public class UserContextException : CatalystBaseException
	{
		public UserContextException(string message) : base("UserContextError", message, null, HttpStatusCode.BadRequest) { }
	}

	public class WrongEnvironmentException : CatalystBaseException
	{
		public WrongEnvironmentException(WrongEnvironmentError data) : base("InvalidToken", $"Environment mismatch. Token gives access to {data.TokenIssuerEnvironment} while this API expects {data.ExpectedEnvironment}", data, HttpStatusCode.Unauthorized) { }
	}

	public class WrongEnvironmentError
	{
		public string TokenIssuerEnvironment { get; set; }
		public string ExpectedEnvironment { get; set; }

	}

	public class InsufficientRolesError
	{
		public IList<string> SufficientRoles { get; set; }
		public IList<string> AssignedRoles { get; set; }
	}

	public class InvalidUserTypeError
	{
		public string ThisUserType { get; set; }
		public List<string> UserTypesThatCanAccess { get; set; }
	}
}
