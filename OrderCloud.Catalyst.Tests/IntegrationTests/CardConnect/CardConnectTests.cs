using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Flurl.Http.Testing;
using NUnit.Framework;
using OrderCloud.Integrations.Payment.CardConnect;
using OrderCloud.Integrations.Payment.CardConnect.Mappers;
using OrderCloud.Integrations.Payment.CardConnect.Models;

namespace OrderCloud.Catalyst.Tests.IntegrationTests
{
    [TestFixture]
    public class CardConnectTests
    {
        private static Fixture _fixture = new Fixture();
        private HttpTest _httpTest;

        private static CardConnectConfig _config = new CardConnectConfig()
        {
            MerchantId = _fixture.Create<string>(),
            APIPassword = _fixture.Create<string>(),
            APIUsername = _fixture.Create<string>(),
            BaseUrl = "https://api.fake.com"
        };

        private CardConnectService _command = new CardConnectService(_config);
        private AuthorizeCCTransaction _authorization = _fixture.Create<AuthorizeCCTransaction>();
        private FollowUpCCTransaction _capture = _fixture.Create<FollowUpCCTransaction>();

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
        public void ShouldThrowErrorIfMissingRequiredConfigs()
        {
            // Arrange
            var config = new CardConnectConfig() {APIUsername = _fixture.Create<string>()};
            // Act
            var ex = Assert.ThrowsAsync<IntegrationMissingConfigsException>(async () =>
                await _command.AuthorizeOnlyAsync(_authorization, config));
            // Assert
            var data = (IntegrationMissingConfigs)ex.Errors[0].Data;
            Assert.AreEqual(data.ServiceName, "CardConnect");
            Assert.AreEqual(data.MissingFieldNames, new List<string> {"BaseUrl", "APIPassword", "MerchantId"});
        }

        [Test]
        public void ShouldThrowErrorIfAuthorizationFails()
        {
            // Arrange
            _httpTest.RespondWith("", 401);
            // Act
            var ex = Assert.ThrowsAsync<IntegrationAuthFailedException>(async () =>
                await _command.AuthorizeOnlyAsync(_authorization));
            var data = (IntegrationAuthFailedError)ex.Errors[0].Data;
            var code = ex.Errors[0].ErrorCode;
            // Assert
            Assert.AreEqual(data.ServiceName, "CardConnect");
            Assert.AreEqual(code, "IntegrationAuthorizationFailed");
            Assert.AreEqual(data.ResponseStatus, 401);
            Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/auth");
        }

        [Test]
        public void ShouldThrowErrorIfNoResponse()
        {
            // Arrange
            _httpTest.SimulateTimeout();
            // Act
            var ex = Assert.ThrowsAsync<IntegrationNoResponseException>(async () =>
                await _command.AuthorizeOnlyAsync(_authorization));
            var data = (IntegrationNoResponseError)ex.Errors[0].Data;
            var code = ex.Errors[0].ErrorCode;
            // Assert
            Assert.AreEqual(data.ServiceName, "CardConnect");
            Assert.AreEqual(code, "IntegrationNoResponse");
            Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/auth");

        }
        [Test]
        public void ShouldThrowErrorForBadRequest()
        {
            // Arrange (CardConnect doesn't return errors, OrderCloud.Integrations considers response bodies with respstat = B | C to be error responses)
            _httpTest.RespondWith(
                "{\"amount\":\"0.00\",\"resptext\":\"Non-numeric expiry\",\"cardproc\":\"RPCT\",\"acctid\":\"1\",\"commcard\":\"N\",\"respcode\":\"15\",\"entrymode\":\"ECommerce\",\"defaultacct\":\"Y\",\"merchid\":\"840000000052\",\"token\":\"9601616143390000\",\"respproc\":\"PPS\",\"bintype\":\"\",\"binInfo\":{\"country\":\"USA\",\"product\":\"D\",\"bin\":\"\",\"purchase\":false,\"prepaid\":false,\"issuer\":\"\",\"cardusestring\":\"True credit, No PIN/Signature capability\",\"gsa\":false,\"corporate\":false,\"fsa\":false,\"subtype\":\"Consumer Credit - Rewards\",\"binlo\":\"601100X\",\"binhi\":\"601101X\"},\"retref\":\"119719045889\",\"respstat\":\"C\",\"account\":\"60XXXXXXXXXX0000\"}");
            // Act
            var ex = Assert.ThrowsAsync<IntegrationErrorResponseException>(async () =>
                await _command.AuthorizeOnlyAsync(_authorization));
            var data = (IntegrationErrorResponseError)ex.Errors[0].Data;
            var code = ex.Errors[0].ErrorCode;
            // Assert
            Assert.AreEqual(data.ServiceName, "CardConnect");
            Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/auth");
            Assert.AreEqual(code, "IntegrationErrorResponse");
            // CardConnect always returns `200`, and returns errors disguised as a 200 response with error messages
            Assert.AreEqual(data.ResponseStatus, 200);
        }
        [Test]
        public async Task AuthorizeResponseShouldBeMappedCorrectly()
        {
            // Arrange
            var response = _fixture.Create<CardConnectAuthorizationResponse>();
            response.respstat = "A";
            response.respcode = "0";
            _httpTest.RespondWithJson(response);
            // Act
            var actual = await _command.AuthorizeOnlyAsync(_authorization);
            // Assert
            Assert.AreEqual(actual.Amount, response.amount);
            Assert.AreEqual(actual.TransactionID, response.retref);
            Assert.AreEqual(actual.AuthorizationCode, response.authcode);
            Assert.AreEqual(actual.AVSResponseCode, response.avsresp);
            Assert.AreEqual(actual.ResponseCode, response.respstat);
            Assert.AreEqual(actual.Message, response.resptext);
            Assert.AreEqual(actual.Succeeded, response.respstat == "A");
        }
        [Test]
        public async Task AuthorizeResponseWithCardTokenShouldBeMappedCorrectly()
        {
            // Arrange
            var request = _fixture.Create<AuthorizeCCTransaction>();
            request.CardDetails.SavedCardID = null;
            // Act
            var actual = CardConnectAuthorizationRequestMapper.ToCardConnectAuthorizationRequest(request, _config);
            // Assert
            Assert.AreEqual(request.Amount.ToString(), actual.amount);
            Assert.AreEqual(request.Currency, actual.currency);
            Assert.AreEqual(request.CardDetails.Token, actual.account);
            Assert.AreEqual(request.OrderID, actual.orderid);
            Assert.AreEqual(request.AddressVerification.Street1, actual.address);
            Assert.AreEqual(request.AddressVerification.City, actual.city);
            Assert.AreEqual(request.AddressVerification.Zip, actual.postal);
            Assert.AreEqual(request.AddressVerification.State, actual.region);
            Assert.AreEqual(request.AddressVerification.Country, actual.country);
        }
        [Test]
        public async Task AuthorizeRequestWithSavedCardShouldBeMappedCorrectly()
        {
            // Arrange
            var request = _authorization;
            request.CardDetails.SavedCardID = $"{request.ProcessorCustomerID}/{request.CardDetails.SavedCardID}";
            request.CardDetails.Token = null;

            // Act
            var actual = CardConnectAuthorizationRequestMapper.ToCardConnectAuthorizationRequest(request, _config);

            // Assert
            Assert.AreEqual(request.Amount.ToString(), actual.amount);
            Assert.AreEqual(request.Currency, actual.currency);
            Assert.AreEqual(null, actual.account);
            Assert.AreEqual($"{request.ProcessorCustomerID}/{request.CardDetails.SavedCardID}", actual.profile);
        }
        [Test]
        public async Task CaptureResponseShouldBeMappedCorrectly()
        {
            // Arrange
            var response = _fixture.Create<CardConnectCaptureResponse>();
            response.respstat = "A";
            response.amount = "10"; // Has to be an actual number string, not just random char string
            _httpTest.RespondWithJson(response);

            // Act
            var actual = await _command.CapturePriorAuthorizationAsync(_capture);
            // Assert
            Assert.AreEqual(actual.AuthorizationCode, response.authcode);
            Assert.AreEqual(actual.Message, response.setlstat);
            Assert.AreEqual(actual.Succeeded, response.ToIntegrationsCCTransactionResponse().Succeeded);
            Assert.AreEqual(actual.TransactionID, response.retref);
            Assert.AreEqual(actual.Amount, response.ToIntegrationsCCTransactionResponse().Amount);
        }
        [Test]
        public async Task SuccessAuthorizeResponse()
        {
            // Arrange
            var response = _fixture.Create<CardConnectAuthorizationResponse>();
            response.respstat = "A";
            response.respcode = "0";

            _httpTest.RespondWithJson(response);
            // Act
            var actual = await _command.AuthorizeOnlyAsync(_authorization);

            // Assert
            Assert.AreEqual(true, actual.Succeeded);
        }
    }
}
