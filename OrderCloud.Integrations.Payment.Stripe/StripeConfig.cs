using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Payment.Stripe
{
    public class StripeConfig : OCIntegrationConfig
    {
        public override string ServiceName { get; } = "Stripe";
        [RequiredIntegrationField]
        public string SecretKey { get; set; }
        // Not required because it could be saved as a F.E. config instead. Retrieve with ICreditCardProcessor.GetIframeCredentialAsync()
        public string PublishableKey { get; set; } 
    }
}
