using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using Flurl.Http.Testing;
using NUnit.Framework;
using OrderCloud.Integrations.Payment.BlueSnap;
using OrderCloud.Integrations.Payment.CardConnect;
using OrderCloud.Integrations.Payment.CardConnect.Models;

namespace OrderCloud.Catalyst.Tests.IntegrationTests.CardConnect
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
            // Arrange (real CardConnect 
            _httpTest.RespondWith(
                "{\"Errors\":[{\"ErrorCode\":\"IntegrationErrorResponse\",\"Message\":\"A request to 3rd party service CardConnect resulted in an error.See ResponseBody for details.\",\"Data\":{\"ServiceName\":\"CardConnect\",\"RequestUrl\":\"https://fts-uat.cardconnect.com/cardconnect/rest/auth\",\"ResponseStatus\":200,\"ResponseBody\":{\"token\":\"9601616143390000\",\"retref\":\"118134039900\",\"amount\":0.00,\"expiry\":null,\"merchid\":\"840000000052\",\"avsresp\":null,\"cvvresp\":null,\"signature\":null,\"bintype\":\"\",\"commcard\":\"N\",\"emv\":null,\"binInfo\":null,\"receipt\":null,\"authcode\":null,\"respcode\":\"15\",\"respproc\":\"PPS\",\"respstat\":\"C\",\"resptext\":\"Non-numeric expiry\"}}}]}");
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
    }
}
