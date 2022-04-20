using AutoFixture;
using Flurl.Http.Testing;
using NUnit.Framework;
using OrderCloud.Catalyst.Shipping.EasyPost;
using OrderCloud.Catalyst.Shipping.Fedex;
using System;
using System.Collections.Generic;

namespace OrderCloud.Catalyst.Tests.IntegrationTests
{
	public class FedexTests
	{
		private static Fixture _fixture = new Fixture();
		private HttpTest _httpTest;
		private static FedexConfig _config = new FedexConfig()
		{
			ClientID = _fixture.Create<string>(),
			ClientSecret = _fixture.Create<string>(),
			AccountNumber = _fixture.Create<string>(),
			BaseUrl = "https://api.fake.com",
		};
		private List<ShippingPackage> _packages = _fixture.Create<List<ShippingPackage>>();

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
		public void ShouldThrowErrorIfOverrideConfigMissingFields()
		{
			// Arrange
			var config = new FedexConfig();
			// Act 
			var ex = Assert.ThrowsAsync<IntegrationMissingConfigsException>(async () =>
				await new FedexService(_config).CalculateShippingRatesAsync(_packages, config)
			);
			// Assert
			var data = (IntegrationMissingConfigs)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Fedex");
			Assert.AreEqual(new List<string> { "BaseUrl", "ClientID", "ClientSecret", "AccountNumber" }, data.MissingFieldNames);
		}

		[Test]
		public void ShouldThrowErrorIfDefaultConfigMissingFields()
		{
			// Arrange
			var config = new FedexConfig();
			// Act 
			var ex = Assert.Throws<IntegrationMissingConfigsException>(() =>
				new FedexService(config)
			); 
			// Assert
			var data = (IntegrationMissingConfigs)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Fedex");
			Assert.AreEqual(new List<string> { "BaseUrl", "ClientID", "ClientSecret", "AccountNumber" }, data.MissingFieldNames);
		}

		[Test]
		public void ShouldThrowErrorIfOverrideConfigTypeIsWrong()
		{
			// Arrange
			var config = new EasyPostConfig();
			// Act 
			var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
				await new FedexService(_config).CalculateShippingRatesAsync(_packages, config)
			);
			// Assert
			Assert.AreEqual("Integration configuration must be of type FedexConfig to match this command. Found EasyPostConfig instead. (Parameter 'configOverride')", ex.Message);
		}

		[Test]
		public void ShouldThrowErrorIfGetTokenFails()
		{
			// Arrange
			_httpTest.RespondWith("{\"transactionId\": \"089c30d0-85a7-47c4-84a6-1acfc91d2b7c\",\"errors\":[{\"code\":\"NOT.AUTHORIZED.ERROR\",\"message\":\"The given client credentials were not valid.Please modify your request and try again.\"}]}", 401); // real fedex response

			var ex = Assert.ThrowsAsync<IntegrationAuthFailedException>(async () =>
				// Act 
				await new FedexService(_config).CalculateShippingRatesAsync(_packages)
			);

			var data = (IntegrationAuthFailedError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Fedex");
			Assert.AreEqual(data.ResponseStatus, 401);
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/oauth/token");
		}

		[Test]
		public void ShouldThrowAuthErrorIfGetTokenHas500Response()
		{
			// Arrange
			_httpTest.RespondWithJson(new FedexError() { errors = new List<FedexErrorDetails> { new FedexErrorDetails() { message = "service unavailable" } } }, 503);

			var ex = Assert.ThrowsAsync<IntegrationAuthFailedException>(async () =>
				// Act 
				await new FedexService(_config).CalculateShippingRatesAsync(_packages)
			);

			var data = (IntegrationAuthFailedError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Fedex");
			Assert.AreEqual(data.ResponseStatus, 503);
		}

		[Test]
		public void ShouldThrowErrorIfRateRequestHas500Response()
		{
			// Arrange
			_httpTest.RespondWithJson(new FedexTokenResponse()
			{
				access_token = _fixture.Create<string>(),
				expires_in = 4000
			}, 200);
			_httpTest.RespondWithJson(new FedexError() { errors = new List<FedexErrorDetails> { new FedexErrorDetails() { message = "service unavailable" } } }, 503);

			var ex = Assert.ThrowsAsync<IntegrationErrorResponseException>(async () =>
				// Act 
				await new FedexService(_config).CalculateShippingRatesAsync(_packages)
			);

			var data = (IntegrationErrorResponseError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Fedex");
			Assert.AreEqual(data.ResponseStatus, 503);
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/rate/v1/rates/quotes");
			Assert.AreEqual(((FedexError)data.ResponseBody).errors[0].message, "service unavailable");
		}

		[Test]
		public void ShouldThrowErrorIfNoResponse()
		{
			// Arrange
			_httpTest.SimulateTimeout();

			var ex = Assert.ThrowsAsync<IntegrationNoResponseException>(async () =>
			// Act 
			await new FedexService(_config).CalculateShippingRatesAsync(_packages));

			var data = (IntegrationNoResponseError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Fedex");
		}

		[Test]
		public void ShouldThrowErrorForBadRequest()
		{
			// Arrange
			_httpTest.RespondWithJson(new FedexTokenResponse()
			{
				access_token = _fixture.Create<string>(),
				expires_in = 4000
			}, 200);
			_httpTest
				// real fedex response
				.RespondWith("{\"transactionId\":\"0aa0d6f8-a870-4f8d-b955-c8ee7489e759\",\"errors\":[{\"code\":\"DESTINATION.COUNTRY.INVALID\",\"message\":\"Destination country code is invalid or missing.Please refer to documentation for valid format.\"}]}", 400);

			var ex = Assert.ThrowsAsync<IntegrationErrorResponseException>(async () =>
				// Act 
				await new FedexService(_config).CalculateShippingRatesAsync(_packages)
			);

			var data = (IntegrationErrorResponseError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Fedex");
			Assert.AreEqual(data.ResponseStatus, 400);
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/rate/v1/rates/quotes");
			Assert.AreEqual(((FedexError)data.ResponseBody).errors[0].code, "DESTINATION.COUNTRY.INVALID");
			Assert.AreEqual(((FedexError)data.ResponseBody).errors[0].message, "Destination country code is invalid or missing.Please refer to documentation for valid format.");
		}
	}
}
