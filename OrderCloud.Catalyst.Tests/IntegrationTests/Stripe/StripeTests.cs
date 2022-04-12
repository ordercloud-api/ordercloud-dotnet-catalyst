using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OrderCloud.Catalyst.Payments.Stripe;
using OrderCloud.Catalyst.Payments.Stripe.Models;

namespace OrderCloud.Catalyst.Tests.IntegrationTests.Stripe
{
    public class StripeTests
    {
        private static StripeConfig _config = new StripeConfig()
        {
            BaseUrl = "https://api.stripe.com",
            SecretKey = ""
        };
        private readonly StripeClient _client = new StripeClient(_config);

        [Test]
        public async Task should_call_create_payment_intent()
        {
            var request = new StripePaymentIntentRequest()
            {
                amount = 500,
                currency = "usd",
                payment_method_types = new List<string>()
                {
                    "pm_card_visa"
                }
            };
            var response = await _client.CreatePaymentIntentAsync(request, _config);

            await _client.ConfirmPaymentIntentAsync(response.id, _config);
        }
    }
}
