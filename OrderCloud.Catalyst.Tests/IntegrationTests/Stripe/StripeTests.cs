using System.Collections.Generic;
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
            var paymentIntentCreateOpts = new StripePaymentIntentMapper().MapPaymentIntentCreateAndConfirmOptions(transaction);
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
            var paymentIntentCaptureOptions = new StripePaymentIntentMapper().MapPaymentIntentCaptureOptions(transaction);
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

            var refundCreateOptions = new StripeRefundMapper().MapRefundCreateOptions(transaction);
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

            var cardCreateOptions = new StripeCardMapper().MapStripeCardCreateOptions(pciSafeCardDetails);
            Assert.AreEqual(pciSafeCardDetails.Token, cardCreateOptions.Source.Value);
        }
    }
}
