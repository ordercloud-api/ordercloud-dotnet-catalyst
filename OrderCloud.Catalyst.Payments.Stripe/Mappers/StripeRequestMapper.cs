using System;
using System.Collections.Generic;
using System.Text;
using Stripe;

namespace OrderCloud.Catalyst.Payments.Stripe.Mappers
{
    public class StripeRequestMapper
    {

        // Not sure if we even need this? 
        public static PaymentIntentConfirmOptions MapPaymentIntentConfirmOptions(AuthorizeCCTransaction transaction) => 
            new PaymentIntentConfirmOptions()
                {};

        public static PaymentIntentCaptureOptions MapPaymentIntentCaptureOptions(FollowUpCCTransaction transaction) =>
            new PaymentIntentCaptureOptions()
            {
                AmountToCapture = Convert.ToInt64(transaction.Amount),
            };

        public static RefundCreateOptions MapRefundCreateOptions(FollowUpCCTransaction transaction) =>
            new RefundCreateOptions()
            {
                Amount = Convert.ToInt64(transaction.Amount),
                PaymentIntent = transaction.TransactionID
            };

        public static PaymentIntentCancelOptions MapPaymentIntentCancelOptions(FollowUpCCTransaction transaction) =>
            new PaymentIntentCancelOptions()
                { };
    }
}
