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
        public async Task should_call_create_customer()
        {
            var createRequest = new StripeCustomerRequest();
            var response = await _client.CreateCustomerAsync(createRequest, _config);

            Assert.IsNotNull(response.id);
        }

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

        [Test]
        public async Task payment_flow_should_succeed()
        {
            // https://stripe.com/docs/development/quickstart#test-api-request
            var customerCreateRequest = new StripeCustomerRequest();
            var customer = await _client.CreateCustomerAsync(customerCreateRequest, _config);

            var paymentIntents = new[] { "card" }.ToArray();
            var createPaymentIntentRequest = new StripePaymentIntentRequest()
            {
                amount = 500,
                currency = "usd",
                payment_method_types = paymentIntents,
                customer = customer.id,
                setup_future_usage = "on_session"
            };
            var response = await _client.CreatePaymentIntentAsync(createPaymentIntentRequest, _config);

            Assert.IsNotNull(response.id);

            var confirmPaymentIntentRequest = new StripePaymentIntentRequest()
            {
                payment_method = response.payment_method ?? "pm_card_visa"
            };

            response = await _client.ConfirmPaymentIntentAsync(response.id, confirmPaymentIntentRequest, _config);

            Assert.AreEqual(createPaymentIntentRequest.amount, response.amount_received);
        }
    }
}
