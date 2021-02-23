using System.Threading.Tasks;
using Flurl.Http;
using OrderCloud.Catalyst.Tests.TestingHelpers;
using NUnit.Framework;
using FluentAssertions;
using System.Net;
using OrderCloud.SDK;
using AutoFixture;
using OrderCloud.DemoWebApi;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Security.Cryptography;

namespace OrderCloud.Catalyst.Tests
{
    public class OCWebhookAuthTests
    {
        private TestService _service;

        [SetUp]
        public void Setup()
        {
            _service = new TestService();
        }

        [Test]
        public async Task can_disambiguate_webhook()
        {
            Fixture fixture = new Fixture();
            var payload = fixture.Create<WebhookPayloads.Addresses.Save>();
            payload.Route = "v1/buyers/{buyerID}/addresses/{addressID}";
            payload.Verb = "PUT";

            //dynamic resp = await _service.SendWebhookReq(payload).ReceiveJson(); //SendWebhookReq(payload).ReceiveJson();

            var _settings = new AppSettings();
            var json = JsonConvert.SerializeObject(payload);
            var keyBytes = Encoding.UTF8.GetBytes(_settings.WebhookHashKey);
            var dataBytes = Encoding.UTF8.GetBytes(json);
            var hash = new HMACSHA256(keyBytes).ComputeHash(dataBytes);
            var base64 = Convert.ToBase64String(hash);

            dynamic resp = _service.CreateServer()
                .CreateFlurlClient()
                .AllowAnyHttpStatus()
                .Request("demo/webhook")
                .WithHeader("X-oc-hash", base64)
                .PostJsonAsync(payload);

            Assert.AreEqual(resp.Action, "HandleAddressSave");
            Assert.AreEqual(resp.City, "Minneapolis");
            Assert.AreEqual(resp.Foo, "blah");
        }

        //[Test]
        //public async Task hash_does_not_match()
        //{
        //    var payload = new
        //    {
        //        Route = "v1/buyers/{buyerID}/addresses/{addressID}",
        //        Verb = "PUT",
        //        Request = new { Body = new { City = "Minneapolis" } },
        //        ConfigData = new { Foo = "blah" }
        //    };

        //    var resp = await _service.SendWebhookReq(payload, "dfadasfd");  //SendWebhookReq(payload, "dfadasfd");
        //    resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        //}
    }
}
