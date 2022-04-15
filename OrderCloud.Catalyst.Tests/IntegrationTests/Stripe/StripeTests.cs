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
        public async Task should_call_create_payment_intent()
        {
            var result = new [] {"pm_card_visa"}.ToArray();
            var request = new StripePaymentIntentRequest()
            {
                amount = 500,
                currency = "usd",
                //https://stackoverflow.com/questions/67750333/flutter-invalid-array-in-payment-method-types-of-stripe-checkout?noredirect=1#comment119781638_67750333
                payment_method_types = result
                // payment_method_types[n] = "pm_card_visa"
            };
            var response = await _client.CreatePaymentIntentAsync(request, _config);

            Assert.IsNotNull(response.id);

            request = new StripePaymentIntentRequest()
            {
                payment_method = response.payment_method
            };

            response = await _client.ConfirmPaymentIntentAsync(response.id, request, _config);

            Assert.AreEqual(request.amount, response.amount_received);
        }
    }
}
