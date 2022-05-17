using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.CardConnect.Models
{
    /// <summary>
    /// https://developer.cardpointe.com/cardconnect-api#refund-request
    /// </summary>
    public class CardConnectFundReversalRequest
	{
        /// <summary>
        /// CardPointe retrieval reference number from authorization response
        /// </summary>
        public string retref { get; set; }
        /// <summary>
        /// Merchant ID, required for all requests. Must match merchid of transaction to be captured
        /// </summary>
        public string merchid { get; set; }
        /// <summary>
        /// Positive amount with decimal or amount without decimal in currency minor units (for example, USD Pennies or MXN Centavos) for partial refunds.
        /// If no value is provided, the full amount of the transaction is refunded.
        /// </summary>
        public decimal? amount { get; set; } = 0M;
    }

    public class CardConnectFundReversalResponse : CardConnectResponseData
    {
        /// <summary>
        /// Copied from refund request
        /// </summary>
        public string merchid { get; set; }
        /// <summary>
        /// Copied from refund request, contains the refund amount
        /// </summary>
		public string amount { get; set; }
        /// <summary>
        /// New retref of refund 
        /// </summary>
        public string retref { get; set; }
        /// <summary>
        /// The orderid, copied from the original authorization request or the refund request.
        /// The orderId is returned in the response when the authorization or refund request includes an orderid and "receipt":"y"
        /// If no orderid was passed and "receipt":"json", "n", or is omitted, the orderId field is omitted from the response.
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// Identifies if the void was successful. Can one of the following values:
        /// - REVERS: Successful
        /// - Null: Unsuccessful.  Refer to the respcode and resptext
        /// </summary>
        public string authcode { get; set; }
    }
}