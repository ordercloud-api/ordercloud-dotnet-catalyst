using System.Threading.Tasks;
using Flurl.Http;
using OrderCloud.Catalyst.Tests.TestingHelpers;
using NUnit.Framework;
using FluentAssertions;
using System.Net;

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
            var payload = new
            {
                Route = "v1/buyers/{buyerID}/addresses/{addressID}",
                Verb = "PUT",
                Request = new { Body = new { City = "Minneapolis" } },
                ConfigData = new { Foo = "blah" }
            };

            dynamic resp = await _service.SendWebhookReq(payload).ReceiveJson(); //SendWebhookReq(payload).ReceiveJson();

            Assert.AreEqual(resp.Action, "HandleAddressSave");
            Assert.AreEqual(resp.City, "Minneapolis");
            Assert.AreEqual(resp.Foo, "blah");
        }

        [Test]
        public async Task hash_does_not_match()
        {
            var payload = new
            {
                Route = "v1/buyers/{buyerID}/addresses/{addressID}",
                Verb = "PUT",
                Request = new { Body = new { City = "Minneapolis" } },
                ConfigData = new { Foo = "blah" }
            };

            var resp = await _service.SendWebhookReq(payload, "dfadasfd");  //SendWebhookReq(payload, "dfadasfd");
            resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
