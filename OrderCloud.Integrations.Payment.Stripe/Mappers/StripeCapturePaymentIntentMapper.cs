using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    public class StripeCapturePaymentIntentMapper
    {
        /// <summary>
        /// https://stripe.com/docs/api/payment_intents/capture
        /// </summary>
        public static PaymentIntentCaptureOptions MapPaymentIntentCaptureOptions(FollowUpCCTransaction transaction)
        {
            var options = new PaymentIntentCaptureOptions();
            if (transaction.Amount > 0)
                // defaults to full amount_capturable if not provided
                options.AmountToCapture = Convert.ToInt64(transaction.Amount);
            return options;
        }
    }
}
