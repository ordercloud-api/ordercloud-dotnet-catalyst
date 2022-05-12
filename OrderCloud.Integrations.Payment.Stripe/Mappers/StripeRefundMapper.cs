using System;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    /// <summary>
    /// https://stripe.com/docs/api/refunds
    /// </summary>
    public class StripeRefundMapper
    {
        public RefundCreateOptions MapRefundCreateOptions(FollowUpCCTransaction transaction) =>
            new RefundCreateOptions()
            {
                Amount = Convert.ToInt64(transaction.Amount),
                PaymentIntent = transaction.TransactionID
            };

        public CCTransactionResult MapRefundCreateResponse(Refund refund) => 
            new CCTransactionResult()
            {
                Message = refund.Status,
                Succeeded = refund.Status.ToLower() == "succeeded",
                TransactionID = refund.Id,
                Amount = refund.Amount
            };
    }
}
