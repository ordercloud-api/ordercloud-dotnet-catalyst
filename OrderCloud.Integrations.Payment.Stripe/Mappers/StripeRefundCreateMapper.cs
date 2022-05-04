using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    public class StripeRefundCreateMapper
    {
        /// <summary>
        /// https://stripe.com/docs/api/refunds/create
        /// </summary>
        public RefundCreateOptions MapRefundCreateOptions(FollowUpCCTransaction transaction) =>
            new RefundCreateOptions()
            {
                Amount = Convert.ToInt64(transaction.Amount),
                PaymentIntent = transaction.TransactionID
            };
    }
}
