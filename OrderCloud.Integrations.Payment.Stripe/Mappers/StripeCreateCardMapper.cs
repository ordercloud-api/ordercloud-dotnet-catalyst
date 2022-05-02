using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    public class StripeCreateCardMapper
    {
        public static Card MapCardCreateOptions(PaymentSystemCustomer customer, PCISafeCardDetails card)
        {
            return new Card()
            {

            };
        }
    }
}
