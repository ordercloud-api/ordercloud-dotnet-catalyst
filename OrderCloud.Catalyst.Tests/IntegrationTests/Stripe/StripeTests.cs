using System.Collections.Generic;
using AutoFixture;
using NUnit.Framework;
using OrderCloud.Integrations.Payment.Stripe;
using OrderCloud.Integrations.Payment.Stripe.Mappers;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst.Tests.IntegrationTests
{
    public class StripeTests
    {
        private static Fixture _fixture = new Fixture();

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
                CardDetails = new PCISafeCardDetails()
                {
                    Token = "pm_234234234234234234" // payment method ID
                },
                OrderWorksheet = _fixture.Create<OrderWorksheet>()
            };
            var paymentIntentCreateOpts = new StripePaymentIntentMapper().MapPaymentIntentCreateAndConfirmOptions(transaction);
            Assert.AreEqual(transaction.Amount, 50000);
            Assert.AreEqual(transaction.Currency, paymentIntentCreateOpts.Currency);
            Assert.AreEqual(transaction.CardDetails.Token, paymentIntentCreateOpts.PaymentMethod);
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

		[Test]
        [TestCase(500.1234, 50012)]
        [TestCase(1, 100)]
        [TestCase(0, 0)]
        [TestCase(5.759, 576)]
        public void ForADecimalCurrencyAmountShouldBeInCents(decimal input, long expectedResult)
		{
			var transaction = new AuthorizeCCTransaction()
			{
				Amount = input,
				Currency = "USD",
                OrderWorksheet = _fixture.Create<OrderWorksheet>()
            };

			var paymentIntentMapper = new StripePaymentIntentMapper();
			var paymentIntentCreateOptions = paymentIntentMapper.MapPaymentIntentCreateAndConfirmOptions(transaction);

            Assert.AreEqual(expectedResult, paymentIntentCreateOptions.Amount);
		}

        [Test]
        [TestCase(500.1234, 500)]
        [TestCase(1, 1)]
        [TestCase(0, 0)]
        [TestCase(5.759, 6)]
        public void ForANonDecimalCurrencyAmountShouldBeInDollars(decimal input, long expectedResult)
        {
            var transaction = new AuthorizeCCTransaction()
            {
                Amount = input,
                Currency = "JPY", // yen
                OrderWorksheet = _fixture.Create<OrderWorksheet>()
            };

            var paymentIntentMapper = new StripePaymentIntentMapper();
            var paymentIntentCreateOptions = paymentIntentMapper.MapPaymentIntentCreateAndConfirmOptions(transaction);

            Assert.AreEqual(expectedResult, paymentIntentCreateOptions.Amount);
        }
    }
}
