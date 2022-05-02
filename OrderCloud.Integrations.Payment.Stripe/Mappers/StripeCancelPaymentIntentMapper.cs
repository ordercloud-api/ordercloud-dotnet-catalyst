using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    public class StripeCancelPaymentIntentMapper
    {
        /// <summary>
        /// https://stripe.com/docs/api/payment_intents/cancel
        /// PaymentIntentCancelOptions only has one property for cancellation_reason, and doesn't map to anything from FollowUpCCTransaction
        /// </summary>
        public static PaymentIntentCancelOptions MapPaymentIntentCancelOptions(FollowUpCCTransaction transaction) =>
            new PaymentIntentCancelOptions()
                { };
    }
}
