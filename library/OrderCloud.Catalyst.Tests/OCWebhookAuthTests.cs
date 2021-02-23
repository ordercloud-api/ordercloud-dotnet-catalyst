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
        public async Task hash_is_authenticated()
        {
            Fixture fixture = new Fixture();
            var payload = fixture.Create<WebhookPayloads.Addresses.Save>();
            payload.ConfigData = new { Foo = "blah" };
            //payload.Route = "v1/buyers/{buyerID}/addresses/{addressID}";
            //payload.Verb = "PUT";

            dynamic resp = await _service.SendWebhookReq(payload).ReceiveJson(); //SendWebhookReq(payload).ReceiveJson();

            Assert.AreEqual(resp.Action, "HandleAddressSave");
            Assert.AreEqual(resp.City, payload.Request.Body.City);
            Assert.AreEqual(resp.Foo, "blah");
        }

        [Test]
        public async Task hash_does_not_match()
        {
            Fixture fixture = new Fixture();
            var payload = fixture.Create<WebhookPayloads.Addresses.Save>();

            var resp = await _service.SendWebhookReq(payload, "dfadasfd");  //SendWebhookReq(payload, "dfadasfd");
            resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
