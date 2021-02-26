using System.Threading.Tasks;
using Flurl.Http;
using OrderCloud.TestWebApi;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using System;
using OrderCloud.DemoWebApi;
using System.Net.Http;

namespace OrderCloud.Catalyst.Tests.TestingHelpers
{
    public class TestService
    {
		public TestServer CreateServer()
		{
			return new TestServer(CatalystWebHostBuilder.CreateWebHostBuilder<TestStartup, AppSettings>(new string[] { }));
		}

		public IFlurlRequest CreateRequestWithToken(string endpoint, string mockToken)
		{
			return CreateServer()
				.CreateFlurlClient()
				.WithFakeOrderCloudToken(mockToken)
				.Request(endpoint);
		}

		public Task<HttpResponseMessage> SendWebhookReq(object payload, string hashKey = null)
		{

			var _settings = new AppSettings();
			var json = JsonConvert.SerializeObject(payload);
			var keyBytes = Encoding.UTF8.GetBytes(hashKey ?? _settings.OrderCloudSettings.WebhookHashKey);
			var dataBytes = Encoding.UTF8.GetBytes(json);
			var hash = new HMACSHA256(keyBytes).ComputeHash(dataBytes);
			var base64 = Convert.ToBase64String(hash);

			return CreateServer()
				.CreateFlurlClient()
				.AllowAnyHttpStatus()
				.Request("webhook/saveaddress")
				.WithHeader("X-oc-hash", base64)
				.PostJsonAsync(payload);
		}
	}
}
