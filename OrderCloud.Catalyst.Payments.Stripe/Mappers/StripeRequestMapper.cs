using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst.Integrations.Interfaces;
using Stripe;

namespace OrderCloud.Catalyst.Payments.Stripe.Mappers
{
    public class StripeRequestMapper
    {

        public static PaymentIntentCreateOptions MapPaymentIntentCreateOptions(CreateCardTransaction transaction, string captureMethod)
        {
            return new PaymentIntentCreateOptions()
            {
                Amount = Convert.ToInt64(transaction.Amount),
                Currency = transaction.Currency,
                CaptureMethod = captureMethod
            };
        }
    }
}
