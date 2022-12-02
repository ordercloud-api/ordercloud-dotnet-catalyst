using AutoFixture;
using Flurl.Http.Testing;
using NUnit.Framework;
using OrderCloud.Integrations.Messaging.MailChimp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests.IntegrationTests
{
    [TestFixture]
    public class MailChimpTests
    {
        private static Fixture _fixture = new Fixture();
        private HttpTest _httpTest;
        private static MailChimpConfig _config = _fixture.Create<MailChimpConfig>();
        private MailChimpService _command = new MailChimpService(_config);
        private EmailMessage _message = _fixture.Create<EmailMessage>();

        [SetUp]
        public void CreateHttpTest()
        {
            _httpTest = new HttpTest();
        }

        [TearDown]
        public void DisposeHttpTest()
        {
            _httpTest.Dispose();
        }

        [Test]
		[TestCase("unsigned")]
        [TestCase("invalid-sender")]
        public void ShouldThrowErrorForRejectedStatus(string reason)
        {
            // Arrange
            _httpTest
                // real mailchimp response
                .RespondWith("[{\"email\":\"someemail@fake.com\",\"status\":\"rejected\",\"reject_reason\": \"" + reason + "\", \"queued_reason\":null,\"_id\":\"00e66368bcd44947b685d18e58be8d2d\"}]", 200);

            var ex = Assert.ThrowsAsync<IntegrationErrorResponseException>(async () =>
                // Act 
                await _command.SendSingleEmailAsync(_message)
            );

            var data = (IntegrationErrorResponseError)ex.Errors[0].Data;
            Assert.AreEqual(data.ServiceName, "MailChimp");
            Assert.AreEqual(data.ResponseStatus, 200);
            Assert.AreEqual(data.RequestUrl, "https://mandrillapp.com/api/1.0/messages/send");
            Assert.AreEqual(((List<MailChimpSendMessageResult>)data.ResponseBody)[0].reject_reason, reason);
        }

        [Test]
        [TestCase("You must specify a key value", true)]
        [TestCase("Invalid API key", true)]
		[TestCase("No such template \"this-template-doesnt-exist\"", false)]
        public void ShouldThrowUnAuthorizedErrorForCertainMessages(string message, bool shouldBeUnauthorizedException)
        {
            // Arrange
            _httpTest
                // real mailchimp response
                .RespondWith("{\"status\":\"error\",\"code\":-1,\"message\":\"" + message + "\",\"name\":\"Invalid_Key\"}", 500);

            if (shouldBeUnauthorizedException)
			{
                var ex = Assert.ThrowsAsync<IntegrationAuthFailedException>(async () =>
                // Act 
                    await _command.SendSingleEmailAsync(_message)
                );
            } else
			{
                var ex = Assert.ThrowsAsync<IntegrationErrorResponseException>(async () =>
                    // Act 
                    await _command.SendSingleEmailAsync(_message)
                );
            }
        }
    }
}
