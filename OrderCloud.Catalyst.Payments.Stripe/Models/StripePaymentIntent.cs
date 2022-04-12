using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Payments.Stripe.Models
{
    // https://stripe.com/docs/api/payment_intents/create?lang=dotnet
    public class StripePaymentIntentRequest
    {
        /// <summary>
        /// REQUIRED A positive integer representing amount intended to be collected.
        /// Minimum value is 50 ($0.50)
        /// </summary>
        public int amount { get; set; }
        /// <summary>
        /// REQUIRED Three-letter ISO currency code in lowercase.
        /// Supported currencies: https://stripe.com/docs/currencies
        /// </summary>
        public string currency { get; set; }
        public List<string> payment_method_types { get; set; }
    }

    public class StripePaymentIntentResponse
    {
        public string id { get; set; }
        public int amount { get; set; }
        public int amount_capturable { get; set; }
        public int amount_received { get; set; }
        public List<string> payment_method_types { get; set; }

    }
}
