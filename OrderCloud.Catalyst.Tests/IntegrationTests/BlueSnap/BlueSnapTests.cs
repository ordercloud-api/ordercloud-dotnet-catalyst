using AutoFixture;
using Flurl.Http.Testing;
using NUnit.Framework;
using OrderCloud.Integrations.Payment.BlueSnap;
using OrderCloud.Integrations.Shipping.EasyPost;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests.IntegrationTests
{
	[TestFixture]
	public class BlueSnapTests
	{
		private static Fixture _fixture = new Fixture();
		private HttpTest _httpTest;
		private static BlueSnapConfig _config = new BlueSnapConfig()
		{
			APIUsername = _fixture.Create<string>(),
			APIPassword = _fixture.Create<string>(),
			BaseUrl = "https://api.fake.com",
		};
		private BlueSnapService _command = new BlueSnapService(_config);
		private AuthorizeCCTransaction _authorizeModel = _fixture.Create<AuthorizeCCTransaction>();

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
			var config = new BlueSnapConfig()
			{
				APIUsername = "something"
			};
			// Act 
			var ex = Assert.ThrowsAsync<IntegrationMissingConfigsException>(async () =>
				await _command.AuthorizeOnlyAsync(_authorizeModel, config)
			);
			// Assert
			var data = (IntegrationMissingConfigs)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "BlueSnap");
			Assert.AreEqual(data.MissingFieldNames, new List<string> { "BaseUrl", "APIPassword" });
		}

		[Test]
		public void ShouldThrowErrorIfAuthorizationFails()
		{
			// Arrange
			_httpTest.RespondWith("<!doctype html><html lang=\"en\"><head><title>HTTP Status 401 – Unauthorized</title><style type=\"text / css\">body {font-family:Tahoma,Arial,sans-serif;} h1, h2, h3, b {color:white;background-color:#525D76;} h1 {font-size:22px;} h2 {font-size:16px;} h3 {font-size:14px;} p {font-size:12px;} a {color:black;} .line {height:1px;background-color:#525D76;border:none;}</style></head><body><h1>HTTP Status 401 – Unauthorized</h1><hr class=\"line\" /><p><b>Type</b> Status Report</p><p><b>Message</b> Unauthorized</p><p><b>Description</b> The request has not been applied because it lacks valid authentication credentials for the target resource.</p><hr class=\"line\" /><h3>Apache Tomcat Version X</h3></body></html>", 401); // real BlueSnap response

			var ex = Assert.ThrowsAsync<IntegrationAuthFailedException>(async () =>
				// Act 
				await _command.AuthorizeOnlyAsync(_authorizeModel)
			);

			var data = (IntegrationAuthFailedError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "BlueSnap");
			Assert.AreEqual(data.ResponseStatus, 401);
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/services/2/transactions");
		}

		[Test]
		public void ShouldThrowErrorIfNoResponse()
		{
			// Arrange
			_httpTest.SimulateTimeout();

			var ex = Assert.ThrowsAsync<IntegrationNoResponseException>(async () =>
				// Act 
				await _command.AuthorizeOnlyAsync(_authorizeModel)
			);

			var data = (IntegrationNoResponseError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "BlueSnap");
		}

		[Test]
		public void ShouldThrowErrorForBadRequest()
		{
			// Arrange
			_httpTest
				// real vertex response
				.RespondWith("{\"message\":[{\"errorName\":\"NO_PAYMENT_DETAILS_LINKED_TO_TOKEN\",\"code\":\"14042\",\"description\":\"Token is not associated with a payment method, please verify your client integration or contact BlueSnap support.\"}]}", 400);

			var ex = Assert.ThrowsAsync<IntegrationErrorResponseException>(async () =>
				// Act 
				await _command.AuthorizeOnlyAsync(_authorizeModel)
			);

			var data = (IntegrationErrorResponseError)ex.Errors[0].Data;
			Assert.AreEqual(data.ServiceName, "BlueSnap");
			Assert.AreEqual(data.ResponseStatus, 400);
			Assert.AreEqual(data.RequestUrl, $"{_config.BaseUrl}/services/2/transactions");
			var responseBody = (BlueSnapError) data.ResponseBody;
			Assert.AreEqual(responseBody.message[0].code, "14042");
			Assert.AreEqual(responseBody.message[0].errorName, "NO_PAYMENT_DETAILS_LINKED_TO_TOKEN");
			Assert.AreEqual(responseBody.message[0].description, "Token is not associated with a payment method, please verify your client integration or contact BlueSnap support.");
		}
	}
}
