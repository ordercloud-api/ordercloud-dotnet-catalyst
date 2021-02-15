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

        public NotFoundException(string thingName, string interopID) : base("NotFound", 404, $"{thingName} with ID {interopID} not found") { }
    }

    public class InsufficientRolesError
    {
        public IList<string> SufficientRoles { get; set; }
        public IList<string> AssignedRoles { get; set; }
    }
}
