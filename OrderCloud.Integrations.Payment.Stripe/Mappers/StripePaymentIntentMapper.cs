using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    public class StripePaymentIntentMapper
    {
        /// <summary>
        /// https://stripe.com/docs/api/payment_intents/capture
        /// </summary>
        public PaymentIntentCaptureOptions MapPaymentIntentCaptureOptions(FollowUpCCTransaction transaction)
        {
            var options = new PaymentIntentCaptureOptions();
            if (transaction.Amount > 0)
                // defaults to full amount_capturable if not provided
                options.AmountToCapture = Convert.ToInt64(transaction.Amount);
            return options;
        }

        public CCTransactionResult MapPaymentIntentCaptureResponse(PaymentIntent capturedPaymentIntent) =>
            new CCTransactionResult()
            {
                Message = capturedPaymentIntent.Status,
                Succeeded = capturedPaymentIntent.Status.ToLower() == "succeeded",
                TransactionID = capturedPaymentIntent.Id,
                Amount = capturedPaymentIntent.Amount
            };

        public PaymentIntentCreateOptions MapPaymentIntentCreateAndConfirmOptions(AuthorizeCCTransaction transaction) =>
            new PaymentIntentCreateOptions()
            {
                Amount = Convert.ToInt64(transaction.Amount),
                Confirm = true, // Creates and Confirms PaymentIntent, otherwise Confirm PaymentIntent would be a separate call
                CaptureMethod = "manual", // Required value for separate auth and capture
                Currency = transaction.Currency,
                Customer = transaction.ProcessorCustomerID,
                PaymentMethod = transaction.TransactionID, // Represents PaymentMethodID
            };

        public CCTransactionResult MapPaymentIntentCreateAndConfirmResponse(PaymentIntent createdPaymentIntent) =>
            new CCTransactionResult()
            {
                Message = createdPaymentIntent.Status,
                Succeeded = createdPaymentIntent.Status.ToLower() == "requires_capture",
                TransactionID =
                    createdPaymentIntent
                        .Id, // transaction.TransactionID represents PaymentMethodID, this now represents PaymentIntentID
                Amount = createdPaymentIntent.Amount
            };

        /// <summary>
        /// https://stripe.com/docs/api/payment_intents/cancel
        /// PaymentIntentCancelOptions only has one property for cancellation_reason, and doesn't map to anything from FollowUpCCTransaction
        /// </summary>
        public PaymentIntentCancelOptions MapPaymentIntentCancelOptions(FollowUpCCTransaction transaction) =>
            new PaymentIntentCancelOptions()
                { };

        public CCTransactionResult MapPaymentIntentCancelResponse(PaymentIntent canceledPaymentIntent) =>
            new CCTransactionResult()
            {
                Message = canceledPaymentIntent.Status,
                Succeeded = canceledPaymentIntent.Status.ToLower() == "canceled",
                TransactionID = canceledPaymentIntent.Id,
                Amount = canceledPaymentIntent.Amount
            };
    }
}
