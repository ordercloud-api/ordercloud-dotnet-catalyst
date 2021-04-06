using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
    /// <summary>
    /// Sent to Ordercloud in response to a pre-webhook. 
    /// </summary>
    public class PreWebhookResponse
    {
        /// <summary>
        /// Returns a webhook response indicating that OrderCloud should proceed with normal processing of the request.
        /// </summary>
        public static PreWebhookResponse Proceed() => new PreWebhookResponse { proceed = true };

        /// <summary>
        /// Returns a webhook response indicating that OrderCloud should NOT proceed with normal processing of the request.
        /// </summary>
        /// <returns></returns>
        public static PreWebhookResponse Block() => new PreWebhookResponse { proceed = false, body = "Access Denied" };

        /// <summary>
        /// Returns a webhook response indicating that OrderCloud should NOT proceed with normal processing of the request.
        /// </summary>
        /// <param name="responseBody">
        /// A response to pass along to the client when blocking a request
        /// </param>
        /// <returns></returns>
        public static PreWebhookResponse Block(object responseBody) => new PreWebhookResponse { proceed = false, body = responseBody };

        /// <summary>
        /// A value indicating whether OrderCloud should proceed with normal processing of the request.
        /// </summary>
        public bool proceed { get; set; }

        /// <summary>
        /// The body that the client will recieve if a request is blocked
        /// </summary>
        public object body { get; set; }
    }
}
