using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    public class StripeCustomerCreateMapper
    {
        public static Customer MapCustomerOptions(PaymentSystemCustomer customer)
        {
            return new Customer();
        }
    }
}
