using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;
using OrderCloud.Integrations.Payment.Stripe;
using OrderCloud.Integrations.Payment.Stripe.Mappers;
using Stripe;
using StripeClient = OrderCloud.Integrations.Payment.Stripe.StripeClient;

namespace OrderCloud.Catalyst.Tests.IntegrationTests.Stripe
{
    public class StripeTests
    {
        //https://stackoverflow.com/questions/67705263/c-sharp-mock-stripe-services-returns-null
        private static StripeConfig _config = new StripeConfig()
        {
            SecretKey = ""
        };
        private readonly StripeService _service = new StripeService(_config);
        private readonly StripeClient _client = new StripeClient(_config);

        [Test]
        public void ShouldThrowErrorIfDefaultConfigMissingFields()
        {
            var config = new StripeConfig();
            var ex = Assert.Throws<IntegrationMissingConfigsException>(() =>
                new StripeService(config)
            );
            var data = (IntegrationMissingConfigs)ex.Errors[0].Data;
            Assert.AreEqual(data.ServiceName, "Stripe");
            Assert.AreEqual(new List<string> { "SecretKey" }, data.MissingFieldNames);
        }

        [Test]
        public void MapCreatePaymentIntentOptions()
        {
            var transaction = new AuthorizeCCTransaction()
            {
                Amount = 500,
                Currency = "USD",
                PaymentMethodID = "pm_234234234234234234"
            };
            var paymentIntentCreateOpts = new StripeCreatePaymentIntentMapper().MapPaymentIntentCreateAndConfirmOptions(transaction);
            Assert.AreEqual(transaction.Amount, paymentIntentCreateOpts.Amount);
            Assert.AreEqual(transaction.Currency, paymentIntentCreateOpts.Currency);
            Assert.AreEqual(transaction.PaymentMethodID, paymentIntentCreateOpts.PaymentMethod);
            Assert.AreEqual(transaction.ProcessorCustomerID, paymentIntentCreateOpts.Customer);
        }

        [Test]
        public void MapCapturePaymentIntentOptions()
        {

        }

        // local testing only
        [Test]
        public async Task SuccessfulPaymentFlowWithCaptureLater()
        {
            // this is handled in the user's middleware
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
            // this is handled in the user's middleware

            var transaction = new AuthorizeCCTransaction()
            {
                Amount = 500,
                Currency = "USD",
                PaymentMethodID = paymentMethod.Id
            };
            var paymentIntent = await _service.CreateAndConfirmPaymentIntentAsync(transaction, _config);
            Assert.AreEqual("requires_capture", paymentIntent.Message);

            var captureTransaction = new FollowUpCCTransaction()
            {
                Amount = 500,
                TransactionID = paymentIntent.TransactionID
            };
            var capturePaymentIntent = await _service.CapturePaymentIntentAsync(captureTransaction, _config);
            Assert.AreEqual("succeeded", capturePaymentIntent.Message);
        }

        //[Test]
        //public async Task payment_flow_should_succeed()
        //{
        //    // https://stripe.com/docs/development/quickstart#test-api-request
        //    var createRequest = new CustomerCreateOptions()
        //    {
        //        Email = "alexa.snyder@sitecore.net"
        //    };
        //    var customer = await _client.CreateCustomerAsync(createRequest, _config);

        //    var createPaymentIntentRequest = new PaymentIntentCreateOptions()
        //    {
        //        Amount = 500,
        //        Currency = "usd",
        //        //PaymentMethodTypes = new List<string>() {"card"},
        //        Customer = customer.Id,
        //        SetupFutureUsage = "on_session",
        //        AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions()
        //        {
        //            Enabled = true
        //        },
        //        CaptureMethod = "manual"
        //    };
        //    var paymentIntent = await _client.CreatePaymentIntentAsync(createPaymentIntentRequest, _config);

        //    Assert.IsNotNull(paymentIntent.Id);

        //    var paymentMethodOptions = new PaymentMethodCreateOptions()
        //    {
        //        Type = "card",
        //        Card = new PaymentMethodCardOptions
        //        {
        //            Number = "4242424242424242",
        //            ExpMonth = 4,
        //            ExpYear = 2023,
        //            Cvc = "314",
        //        }
        //    };
        //    var paymentMethod = await _client.CreatePaymentMethodAsync(paymentMethodOptions, _config);

        //    var confirmPaymentIntentRequest = new PaymentIntentConfirmOptions()
        //    {
        //        PaymentMethod = paymentMethod.Id ?? "pm_card_visa",
        //        ReturnUrl = "https://someplace.com"
        //    };

        //    paymentIntent = await _client.ConfirmPaymentIntentAsync(paymentIntent.Id, confirmPaymentIntentRequest, _config);

        //    // paymentIntent.CaptureMethod == "manual"
        //    Assert.AreEqual("requires_capture", paymentIntent.Status);

        //    //var capturePaymentIntentOptions = new PaymentIntentCaptureOptions()
        //    //{
        //    //    AmountToCapture = 200 // this defaults to full value if excluded
        //    //};

        //    paymentIntent = await _client.CapturePaymentIntentAsync(paymentIntent.Id, new PaymentIntentCaptureOptions());
        //    Assert.AreEqual("succeeded", paymentIntent.Status);
        //}
    }
}
