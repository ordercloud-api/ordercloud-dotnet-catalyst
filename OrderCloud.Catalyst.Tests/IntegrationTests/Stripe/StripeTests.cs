using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task should_call_create_payment_intent_and_confirm()
        {
            var paymentIntents = new [] { "card" }.ToArray();
            var createRequest = new StripePaymentIntentRequest()
            {
                amount = 500,
                currency = "usd",
                payment_method_types = paymentIntents
            };
            var response = await _client.CreatePaymentIntentAsync(createRequest, _config);

            Assert.IsNotNull(response.id);

            var confirmRequest = new StripePaymentIntentRequest()
            {
                payment_method = response.payment_method ?? "pm_card_visa"
            };

            response = await _client.ConfirmPaymentIntentAsync(response.id, confirmRequest, _config);

            Assert.AreEqual(createRequest.amount, response.amount_received);
        }
    }
}
