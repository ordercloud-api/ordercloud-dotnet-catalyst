using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrderCloud.Integrations.Payment.CardConnect.Models;

namespace OrderCloud.Integrations.Payment.CardConnect.Extensions
{
    public static class CardConnectExtensions
    {
        public static bool WasSuccessful(this CardConnectAuthorizationResponse attempt)
        {
            return attempt.respstat == "A" &&
                   (attempt.respcode == "0" || attempt.respcode == "00" || attempt.respcode == "000");
        }
        public static bool WasSuccessful(this CardConnectCaptureResponse attempt)
        {
            return attempt.respstat == "A";
        }

        public static bool WasSuccessful(this CardConnectFundReversalResponse attempt)
        {
            // If the fund reversal is successful, the authcode will contain REVERS. If transaction is not found or an error occurs:
            // Identifies if the void was successful.Can one of the following values: 
            // REVERS - Successful
            // Null - Unsuccessful.Refer to the respcode and resptext.
            return attempt.authcode == "REVERS";
        }
        public static bool WasSuccessful(this CardConnectGetProfileResponse attempt)
        {
            return attempt.profiles.FirstOrDefault().GetType().GetProperty("acctid") != null;
        }

        public static bool WasSuccessful(this CardConnectCreateUpdateProfileResponse attempt)
        {
            return attempt.respstat == "A";
        }

        public static bool WasSuccessful(this CardConnectDeleteProfileResponse attempt)
        {
            return attempt.respstat == "A";
        }
    }
}
