using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OrderCloud.Catalyst.Payments.Stripe;
using OrderCloud.Catalyst.Payments.Stripe.Models;
using Stripe;
using StripeClient = OrderCloud.Catalyst.Payments.Stripe.StripeClient;

namespace OrderCloud.Catalyst.Tests.IntegrationTests.Stripe
{
    public class StripeTests
    {
        private static StripeConfig _config = new StripeConfig()
        {
            SecretKey = ""
        };
        private readonly StripeClient _client = new StripeClient(_config);

        [Test]
        public async Task should_call_create_customer()
        {
            var createRequest = new CustomerCreateOptions()
            {
                Email = "alexa.snyder@sitecore.net"
            };
            var response = await _client.CreateCustomerAsync(createRequest, _config);

            Assert.IsNotNull(response.Id);
        }

        [Test]
        public async Task should_call_create_payment_intent_and_confirm()
        {
            var createRequest = new PaymentIntentCreateOptions()
            {
                Amount = 500,
                Currency = "usd",
                PaymentMethodTypes= new List<string>()
                {
                    "card"
                }
            };
            PaymentIntent response = await _client.CreatePaymentIntentAsync(createRequest, _config);

            Assert.IsNotNull(response.Id);

            var confirmRequest = new PaymentIntentConfirmOptions()
            { PaymentMethod = response.PaymentMethodId };

            response = await _client.ConfirmPaymentIntentAsync(response.Id, confirmRequest, _config);

            Assert.AreEqual(createRequest.Amount, response.AmountReceived);
        }

        [Test]
        public async Task payment_flow_should_succeed()
        {
            // https://stripe.com/docs/development/quickstart#test-api-request
            var createRequest = new CustomerCreateOptions()
            {
                Email = "alexa.snyder@sitecore.net"
            };
            var customer = await _client.CreateCustomerAsync(createRequest, _config);
            
            var createPaymentIntentRequest = new PaymentIntentCreateOptions()
            {
                Amount = 500,
                Currency = "usd",
                //PaymentMethodTypes = new List<string>() {"card"},
                Customer = customer.Id,
                SetupFutureUsage = "on_session",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions()
                {
                    Enabled = true
                },
                CaptureMethod = "manual"
            };
            var paymentIntent = await _client.CreatePaymentIntentAsync(createPaymentIntentRequest, _config);

            Assert.IsNotNull(paymentIntent.Id);

            var paymentMethodOptions = new PaymentMethodCreateOptions()
            {
                Type = "card",
                Card = new PaymentMethodCardOptions
                {
                    Number = "4242424242424242",
                    ExpMonth = 4,
                    ExpYear = 2023,
                    Cvc = "314",
                }
            };
            var paymentMethod = await _client.CreatePaymentMethodAsync(paymentMethodOptions, _config);

            var confirmPaymentIntentRequest = new PaymentIntentConfirmOptions()
            {
                PaymentMethod = paymentMethod.Id ?? "pm_card_visa",
                ReturnUrl = "https://someplace.com"
            };

            paymentIntent = await _client.ConfirmPaymentIntentAsync(paymentIntent.Id, confirmPaymentIntentRequest, _config);

            // paymentIntent.CaptureMethod == "manual"
            Assert.AreEqual("requires_capture", paymentIntent.Status);

            //var capturePaymentIntentOptions = new PaymentIntentCaptureOptions()
            //{
            //    AmountToCapture = 200 // this defaults to full value if excluded
            //};

            paymentIntent = await _client.CapturePaymentIntentAsync(paymentIntent.Id, new PaymentIntentCaptureOptions());
            Assert.AreEqual("succeeded", paymentIntent.Status);
        }
    }
}
