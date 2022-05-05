using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public void MapPaymentIntentCreateOptions()
        {
            var transaction = new AuthorizeCCTransaction()
            {
                Amount = 500,
                Currency = "USD",
                TransactionID = "pm_234234234234234234" // payment method ID
            };
            var paymentIntentCreateOpts = new StripePaymentIntentCreateMapper().MapPaymentIntentCreateAndConfirmOptions(transaction);
            Assert.AreEqual(transaction.Amount, paymentIntentCreateOpts.Amount);
            Assert.AreEqual(transaction.Currency, paymentIntentCreateOpts.Currency);
            Assert.AreEqual(transaction.TransactionID, paymentIntentCreateOpts.PaymentMethod);
            Assert.AreEqual(transaction.ProcessorCustomerID, paymentIntentCreateOpts.Customer);
        }

        [Test]
        public void MapPaymentIntentCaptureOptions()
        {
            var transaction = new FollowUpCCTransaction()
            {
                Amount = 500,
                TransactionID = "pi_345345346456456" // payment intent ID
            };
            var paymentIntentCaptureOptions = new StripePaymentIntentCaptureMapper().MapPaymentIntentCaptureOptions(transaction);
            Assert.AreEqual(transaction.Amount, paymentIntentCaptureOptions.AmountToCapture);
        }

        [Test]
        public void MapRefundCreateOptions()
        {
            var transaction = new FollowUpCCTransaction()
            {
                Amount = 500,
                TransactionID = "pi_345345346456456" // payment intent ID
            };

            var refundCreateOptions = new StripeRefundCreateMapper().MapRefundCreateOptions(transaction);
            Assert.AreEqual(transaction.Amount, refundCreateOptions.Amount);
            Assert.AreEqual(transaction.TransactionID, refundCreateOptions.PaymentIntent);
        }

        [Test]
        public void MapCardCreateOptions()
        {
            var pciSafeCardDetails = new PCISafeCardDetails()
            {
                Token = "tok_mastercard"
            };

            var cardCreateOptions = new StripeCardCreateMapper().MapCardCreateOptions(pciSafeCardDetails);
            Assert.AreEqual(pciSafeCardDetails.Token, cardCreateOptions.Source.Value);
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
                TransactionID = paymentMethod.Id
            };
            var paymentIntent = await _service.AuthorizeOnlyAsync(transaction, _config);
            Assert.AreEqual("requires_capture", paymentIntent.Message);

            var captureTransaction = new FollowUpCCTransaction()
            {
                Amount = 500,
                TransactionID = paymentIntent.TransactionID
            };
            var capturePaymentIntent = await _service.CapturePriorAuthorizationAsync(captureTransaction, _config);
            Assert.AreEqual("succeeded", capturePaymentIntent.Message);
        }


        [Test]
        public async Task CreateWhenCustomerDoesNotExistCC()
        {

            var paymentSysCustomer = new PaymentSystemCustomer()
            {
                FirstName = "Alexa",
                LastName = "Snyder",
                Email = "alexa.snyder@sitecore.com",
                CustomerAlreadyExists = false
            };

            var pciSafeCardDetails = new PCISafeCardDetails()
            {
                Token = "tok_mastercard"
            };
            
            var createdCard = await _service.CreateSavedCardAsync(paymentSysCustomer, pciSafeCardDetails);
            Assert.IsNotNull(createdCard.Token);
        }

        [Test]
        public async Task GetAndListCCs()
        {
            var paymentSysCustomer = new PaymentSystemCustomer()
            {
                FirstName = "Alexa",
                LastName = "Snyder",
                Email = "alexa.snyder@sitecore.com",
                CustomerAlreadyExists = true
            };

            var stripeCustomerOptions = StripeCustomerCreateMapper.MapCustomerOptions(paymentSysCustomer);

            var stripeCustomer = await StripeClient.CreateCustomerAsync(stripeCustomerOptions, _config);
            paymentSysCustomer.ID = stripeCustomer.Id;
            var pciSafeCardDetails = new PCISafeCardDetails()
            {
                Token = "tok_mastercard"
            };
            
            var createdCard = await _service.CreateSavedCardAsync(paymentSysCustomer, pciSafeCardDetails);

            var listCards = await _service.ListSavedCardsAsync(paymentSysCustomer.ID, _config);
            Assert.Contains(createdCard.SavedCardID, listCards.Select(c => c.SavedCardID).ToList());

            var getCard = await _service.GetSavedCardAsync(paymentSysCustomer.ID, createdCard.SavedCardID, _config);
            Assert.IsNotNull(getCard.Token);
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
