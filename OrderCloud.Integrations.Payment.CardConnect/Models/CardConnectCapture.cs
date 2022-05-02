using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.CardConnect.Models
{
	/// <summary>
	/// https://developer.cardpointe.com/cardconnect-api#capture
	/// </summary>
	public class CardConnectCaptureRequest
	{
        /// <summary>
        /// CardPointe retrieval reference number from authorization response
        /// </summary>
        public string retref { get; set; }
        /// <summary>
        /// Merchant ID, required for all requests. Must match merchid of transaction to be captured
        /// </summary>
        public string merchid { get; set; }
    }

	/// <summary>
	/// https://developer.cardpointe.com/cardconnect-api#capture-request
	/// </summary>
	public class CardConnectCaptureResponse
    {
        /// <summary>
        /// Copied from the capture request
        /// </summary>
        public string merchid { get; set; }
        /// <summary>
        /// Masked account number
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// The amount included in the capture request.
        /// </summary>
		public string amount { get; set; }
        /// <summary>
        /// Authorization Code from the Issuer
        /// </summary>
        public string authcode { get; set; }
        /// <summary>
        /// The current settlement status. The settlement status changes throughout the transaction lifecycle, from authorization to settlement. The following values can be returned in the capture response:
        /// Note: See Settlement Status Response Values for a complete list of setlstat values.
        /// - Authorized: The authorization was approved, but the transaction has not yet been captured. 
        /// - Declined: The authorization was declined; therefore, the transaction can not be captured.
        /// - Queued for Capture: The authorization was approved and captured but has not yet been sent for settlement.
        /// - Voided: The authorization was voided; therefore, the transaction cannot be captured.
        /// - Zero Amount: The authorization was a $0 auth for account validation, which cannot be captured.
        /// </summary>
        public string setlstat { get; set; }
        /// <summary>
        /// Y if a Corporate or Purchase Card
        /// </summary>
        public string commcard { get; set; }
        /// <summary>
        /// The retref included in the capture request.
        /// </summary>
        public string retref { get; set; }
        /// <summary>
        /// Automatically created and assigned unless otherwise specified
        /// </summary>
        public string batchid { get; set; }
        /// <summary>
        /// Indicates the status of the authorization request. Can be one of the following values:
        /// - A: Approved
        /// - B: Retry
        /// - C: Declined
        /// </summary>
        public string respstat { get; set; }
    }
}