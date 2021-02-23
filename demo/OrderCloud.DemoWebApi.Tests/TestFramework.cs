using Flurl.Http;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NUnit.Framework;
using OrderCloud.Catalyst;
using OrderCloud.SDK;
using OrderCloud.TestWebApi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.DemoWebApi.Tests
{
	public static class TestFramework
	{
		public static IFlurlClient Client
		{
			get
			{
				var server = new TestServer(CatalystWebHostBuilder.CreateWebHostBuilder<TestStartup, AppSettings>(new string[] { }));
				return new FlurlClient(server.CreateClient())
					.AllowAnyHttpStatus();  // This allows us to test error responses.
			}
		}

		public static async Task<HttpResponseMessage> GetResponseAsync(this IFlurlClient client, string path)
			=> await client.Request(path).GetAsync();

		public static async Task<T> DeserializeAsync<T>(this HttpResponseMessage response)
			=> JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

		public static void ShouldHaveStatusCode(this HttpResponseMessage response, int expectedCode)
			=> Assert.AreEqual((HttpStatusCode)expectedCode, response.StatusCode);

		public static async void ShouldBeApiError(this HttpResponseMessage response, string errorCode, int statusCode, string message)
		{
			var error = await response.DeserializeAsync<ApiError>();
			response.ShouldHaveStatusCode(statusCode);
			Assert.AreEqual(errorCode, error.ErrorCode);
			Assert.AreEqual(message, error.Message);
		}
	}
}
