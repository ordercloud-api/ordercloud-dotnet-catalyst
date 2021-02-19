using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
    public class InsufficientRolesException : CatalystBaseException
    {
        public InsufficientRolesException(InsufficientRolesError data) : base("InsufficientRoles", 403, "Insufficient Roles", data) { }
    }

    public class RequiredFieldException : CatalystBaseException
    {
        public RequiredFieldException(string fieldName) : base("RequiredField", 400, $"Field {fieldName} is required") { }
    }

    public class NotFoundException : CatalystBaseException
    {
        public NotFoundException() : base("NotFound", 404, $"Not found.") { }

        public NotFoundException(string thingName, string  thingID) : base("NotFound", 404, "Not Found.", new { ObjectType = thingName, ObjectID = thingID }) { }
    }

    public class InvalidPropertyException : CatalystBaseException
    {
        public InvalidPropertyException(Type type, string name) : base("Invalid Property", 400, $"{type.Name}.{name}", null) { }
    }

    public class UserErrorException : CatalystBaseException
    {
        public UserErrorException(string message) : base("InvalidRequest", 400, message, null) { }
    }

    public class InsufficientRolesError
    {
        public IList<string> SufficientRoles { get; set; }
        public IList<string> AssignedRoles { get; set; }
    }
}
