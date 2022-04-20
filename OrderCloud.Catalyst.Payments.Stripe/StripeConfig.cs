using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Payments.Stripe
{
    public class StripeConfig : OCIntegrationConfig
    {
        public override string ServiceName { get; } = "Stripe";
        [RequiredIntegrationField]
        public string SecretKey { get; set; }
        // not sure if this is needed
        // On the client-side. Can be publicly-accessible in your web or mobile app’s client-side code (such as checkout.js) to tokenize payment information such as with Stripe Elements. By default, Stripe Checkout tokenizes payment information.
        //[RequiredIntegrationField]
        //public string PublishableKey { get; set; }
    }
}
