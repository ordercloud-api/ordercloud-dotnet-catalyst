using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using OrderCloud.Catalyst.Extensions;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
    public static class ErrorCodes
    {
        public static IDictionary<string, ErrorCode> All { get; } = new Dictionary<string, ErrorCode>
        {
            { "Authorization.InvalidToken", new ErrorCode<UnAuthorizedException>("Authorization.InvalidToken", 401, "Access token is invalid or expired.") }
        };
        public static class Authorization
        {
            public static readonly ErrorCode<UnAuthorizedException> InvalidToken = All["Authorization.InvalidToken"] as ErrorCode<UnAuthorizedException>;
        }
    }
}
