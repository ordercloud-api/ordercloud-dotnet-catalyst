using AutoFixture;
using Flurl.Http.Testing;
using NUnit.Framework;
using OrderCloud.Integrations.Tax.Avalara;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Tests.IntegrationTests
{ 
	[TestFixture]
	public class AvalaraTests
	{
		private static Fixture _fixture = new Fixture();
		private HttpTest _httpTest;
		private static AvalaraConfig _config = new AvalaraConfig()
		{
			AccountID = _fixture.Create<string>(),
			LicenseKey = _fixture.Create<string>(),
			CompanyCode = _fixture.Create<string>(),
			BaseUrl = "https://api.fake.com"
		};
		private AvalaraService _command = new AvalaraService(_config);
		private OrderSummaryForTax _order = _fixture.Create<OrderSummaryForTax>();

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
			var config = new AvalaraConfig();
			// Act 
			var ex = Assert.ThrowsAsync<IntegrationMissingConfigsException>(async () =>
				await _command.CalculateEstimateAsync(_order, config)
			);
			// Assert
			var data = (IntegrationMissingConfigs)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Avalara");
			Assert.AreEqual(data.MissingFieldNames, new List<string> { "BaseUrl", "AccountID", "LicenseKey", "CompanyCode" });
		}

		[Test]
		public void ShouldThrowErrorIfAuthorizationFails()
		{
			// Arrange
			_httpTest.RespondWith("{\"error\": {\"code\": \"AuthenticationException\",\"message\": \"Authentication failed.\",\"details\": [{\"code\": \"AuthenticationException\",\"message\": \"Authentication failed.\",\"description\": \"Missing authentication or unable to authenticate the user or the account.\",\"faultCode\": \"Client\",\"helpLink\": \"http://developer.avalara.com/avatax/errors/AuthenticationException\"}]}}", 401); // real avalara response

			var ex = Assert.ThrowsAsync<IntegrationAuthFailedException>(async () =>
				// Act 
				await _command.CalculateEstimateAsync(_order)
			);

			var data = (IntegrationAuthFailedError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Avalara");
			Assert.AreEqual(data.ResponseStatus, 401);
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/transactions/create");
		}

		[Test]
		public void ShouldThrowErrorIfNoResponse()
		{
			// Arrange
			_httpTest.SimulateTimeout();

			var ex = Assert.ThrowsAsync<IntegrationNoResponseException>(async () =>
				// Act 
				await _command.CalculateEstimateAsync(_order)
			);

			var data = (IntegrationNoResponseError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Avalara");
		}

		[Test]
		public void ShouldThrowErrorForBadRequest()
		{
			// Arrange
			_httpTest
				// real avalara response
				.RespondWith("{\"error\":{\"code\":\"InvalidAddress\",\"message\":\"The address value was incomplete.\",\"target\":\"IncorrectData\",\"details\":[{\"code\":\"InvalidAddress\",\"number\":309,\"message\":\"The address value was incomplete.\",\"description\":\"The address value ShipTo was incomplete.You must provide either a valid postal code, line1 + city + region, or line1 + postal code.\",\"faultCode\":\"Client\",\"helpLink\":\"http://developer.avalara.com/avatax/errors/InvalidAddress\",\"severity\":\"Error\"}]}}", 400);

			var ex = Assert.ThrowsAsync<IntegrationErrorResponseException>(async () =>
				// Act 
				await _command.CalculateEstimateAsync(_order)
			);

			var data = (IntegrationErrorResponseError) ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "Avalara");
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/transactions/create");
			Assert.AreEqual(data.ResponseStatus, 400);
		}
	}
}
