using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst.Integrations.Interfaces;
using Stripe;

namespace OrderCloud.Catalyst.Payments.Stripe.Mappers
{
    public class StripeRequestMapper
    {

        public static PaymentIntentCreateOptions MapPaymentIntentCreateOptions(InitiateCCTransaction transaction, string captureMethod) => 
            new PaymentIntentCreateOptions()
            {
                Amount = Convert.ToInt64(transaction.Amount),
                Currency = transaction.Currency,
                CaptureMethod = captureMethod // will always be manual
            };

        public static PaymentIntentConfirmOptions MapPaymentIntentConfirmOptions(AuthorizeCCTransaction transaction) => 
            new PaymentIntentConfirmOptions()
            {
                ClientSecret = transaction.CardToken
            };

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
    }
}
