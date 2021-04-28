using Flurl.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using OrderCloud.Catalyst;
using OrderCloud.Catalyst.TestApi;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests
{
	public static class TestFramework
	{
		public static IFlurlClient Client
		{
			get
			{
				var server = new TestServer(CatalystWebHostBuilder.CreateWebHostBuilder<TestStartup, TestSettings>(new string[] { }));
				// AllowSynchronousIO = false became the default in asp.net core 3.0 to combat application hangs
				// however we're using synchronous APIs when validating webhook hash
				// specifically ComputeHash will trigger an error here
				// TODO: figure out how to compute the hash in an async manner so we can remove this
				server.AllowSynchronousIO = true; // for webhook tests
				return new FlurlClient(server.CreateClient())
					.AllowAnyHttpStatus();  // This allows us to test error responses.
			}
		}

		public static async void ShouldBeApiError(this IFlurlResponse response, string errorCode, int statusCode, string message)
		{
			var error = await response.GetJsonAsync<ApiErrorList>();
			Assert.AreEqual(statusCode, response.StatusCode);
            Assert.AreEqual(errorCode, error[0].ErrorCode);
            Assert.AreEqual(message, error[0].Message);
        }
	}
}
