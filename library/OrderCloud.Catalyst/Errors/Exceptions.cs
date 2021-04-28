using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using OrderCloud.Catalyst.Extensions;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
    public class UnAuthorizedException : CatalystBaseException
    {
        public UnAuthorizedException() : base("Authorization.InvalidToken", 401, "Access token is invalid or expired.", null) { }
    }
    public class WebhookUnauthorizedException : CatalystBaseException
    {
        public WebhookUnauthorizedException() : base("Authorization.Unauthorized", 401, "X-oc-hash header does not match. Endpoint can only be hit from a valid OrderCloud webhook.", null) { }
    }

    public class InsufficientRolesException : CatalystBaseException
    {
        public InsufficientRolesException(InsufficientRolesError data) : base("InsufficientRoles", 403, "User does not have role(s) required to perform this action.", data) { }
    }

    public class RequiredFieldException : CatalystBaseException
    {
        public RequiredFieldException(string fieldName) : base("RequiredField", 400, $"Field {fieldName} is required", null) { }
    }

    public class NotFoundException : CatalystBaseException
    {
        public NotFoundException() : base("NotFound", 404, $"Not found.", null) { }

        public NotFoundException(string thingName, string thingID) : base("NotFound", 404, "Not Found.", new { ObjectType = thingName, ObjectID = thingID }) { }
    }

    public class InvalidPropertyException : CatalystBaseException
    {
        public InvalidPropertyException(Type type, string name) : base("InvalidProperty", 400, $"{type.Name}.{name}", null) { }
    }

    public class UserErrorException : CatalystBaseException
    {
        public UserErrorException(string message) : base("InvalidRequest", 400, message, null) { }
    }

    public class UserContextException : CatalystBaseException
    {
        public UserContextException(string message) : base("UserContextError", 400, message, null) { }
    }

    public class InsufficientRolesError
    {
        public IList<string> SufficientRoles { get; set; }
        public IList<string> AssignedRoles { get; set; }
    }
}
