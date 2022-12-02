using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
    /// <summary>
    /// Used for message sender types ForgottenPassword and NewUserInvitation
    /// </summary>
    public class SetPasswordMessageSenderPayload : MessageSenderPayload
    {
        public SetPasswordMessageSenderEventBody EventBody { get; set; }
    }

    /// <summary>
    /// Used for message sender types ForgottenPassword and NewUserInvitation
    /// </summary>
    public class SetPasswordMessageSenderPayload<TMessageSenderXp, TUser> : MessageSenderPayload<TMessageSenderXp, TUser>
        where TUser: User
    {
        public SetPasswordMessageSenderEventBody EventBody { get; set; }
	}

    /// <summary>
    /// Used for message sender types ForgottenPassword and NewUserInvitation
    /// </summary>
    public class SetPasswordMessageSenderEventBody
    {
        /// <summary>
        /// The username of the user submitting the request
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The URL passed in the original request
        /// </summary>
        public string PasswordRenewalUrl { get; set; }
        /// <summary>
        /// The verification code to be passed when resetting by verification code
        /// </summary>
        public string PasswordRenewalVerificationCode { get; set; }
        /// <summary>
        /// The token of the user with only the role `PasswordReset` encoded. Used when resetting password by token
        /// </summary>
        public string PasswordRenewalAccessToken { get; set; }
    }
}
