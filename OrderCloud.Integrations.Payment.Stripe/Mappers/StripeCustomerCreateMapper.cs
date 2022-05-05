using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    public class StripeCustomerCreateMapper
    {
        public static CustomerCreateOptions MapCustomerOptions(PaymentSystemCustomer customer)
        {
            return new CustomerCreateOptions()
            {
                Name = $"{customer.FirstName} {customer.LastName}",
                Email = customer.Email
            };
        }
    }
}
