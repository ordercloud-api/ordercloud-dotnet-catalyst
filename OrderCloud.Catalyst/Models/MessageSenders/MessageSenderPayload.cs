using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
    public abstract class MessageSenderPayload
	{
        /// <summary>
        /// Null if user is not a Buyer
        /// </summary>
        public string BuyerID { get; set; }
        /// <summary>
        /// Internal log id for OrderCloud
        /// </summary>
        public string OcLogIdHeader { get; set; }
        /// <summary>
        /// Sandbox, Staging, or Production
        /// </summary>
        public string Environment { get; set; }
        /// <summary>
        /// Token of the recipient user, if ElevatedRoles is defined then the token will include those additional roles as well
        /// </summary>
        public string UserToken { get; set; }
        /// <summary>
        /// The message type that this event was triggered for.
        /// </summary>
        public MessageType MessageType { get; set; }
        /// <summary>
        /// The list of additional emails that will be CC'd for this message
        /// </summary>
        public string[] CCList { get; set; }
        /// <summary>
        /// The full user object of the recipient user
        /// </summary>
        public User Recipient { get; set; }

        /// <summary>
        /// The specific event body for that message type
        /// </summary>
        public dynamic ConfigData { get; set; }
    }

    public abstract class MessageSenderPayload<TMessageSenderXp, TUser> : MessageSenderPayload
        where TUser: User
	{
        
        /// <summary>
        /// The full user object of the recipient user
        /// </summary>
        public new TUser Recipient { get; set; }

        /// <summary>
        /// The specific event body for that message type
        /// </summary>
        public new TMessageSenderXp ConfigData { get; set; }
    }


}
