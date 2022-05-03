using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.CardConnect.Models
{
    public class CardConnectResponseData
    {
        ///<summary>
        /// Alpha-numeric response code that represents the description of the response
        /// </summary>
        public string respcode { get; set; }
        /// <summary>
        /// Abbreviation that represents the platform and the processor for the transaction
        /// </summary>
        public string respproc { get; set; }
        /// <summary>
        /// - A: Approved
        /// - B: Retry
        /// - C: Declined
        /// </summary>
        public string respstat { get; set; }
        /// <summary>
        /// Text description of the 
        /// </summary>
        public string resptext { get; set; }
    }
}
