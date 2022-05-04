using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    public class StripePaymentIntentCreateMapper
    {
        public PaymentIntentCreateOptions MapPaymentIntentCreateAndConfirmOptions(AuthorizeCCTransaction transaction)
        {
            return new PaymentIntentCreateOptions()
            {
                Amount = Convert.ToInt64(transaction.Amount),
                Confirm = true,
                CaptureMethod = "manual", // required value for separate auth and capture
                Currency = transaction.Currency,
                Customer = transaction.ProcessorCustomerID,
                PaymentMethod = transaction.PaymentMethodID,
            };
        }
    }
}
