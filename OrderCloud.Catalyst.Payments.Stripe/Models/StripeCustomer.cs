using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Payments.Stripe.Models
{
    public class StripeCustomerRequest
    {
        public string email { get; set; }
    }

    public class StripeCustomerResponse : StripeCustomerRequest
    {
        public string id { get; set; }
    }
}
