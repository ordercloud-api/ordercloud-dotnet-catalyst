using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NUnit.Framework;
using OrderCloud.Catalyst.DataMovement;
using OrderCloud.SDK;
using AutoFixture;
using FluentAssertions;
using System.Linq;

namespace OrderCloud.Catalyst.Tests
{
    class ListAllAsyncTests
    {
        private const string token = "super-secure-token";
        private HttpRequest mockHttpRequest;
        private IOrderCloudClient mockOrderCloudClient;

        private Fixture _fixture;


        [SetUp]
        public void Setup()
        {
            //Setup mocks and substitutions for Http, Ordercloud, and Fixture
            _fixture = new Fixture();
            mockHttpRequest = Substitute.For<HttpRequest>();
            mockOrderCloudClient = Substitute.For<IOrderCloudClient>();
        }

        [Test]
        public async Task ListAllWorking()
        {
            Task<ListPage<Supplier>> listSupplierResponse;

            //Setup Supplier Response using AutoFixture
            listSupplierResponse = _fixture.Create<Task<ListPage<Supplier>>>();
            listSupplierResponse.Result.Meta.TotalPages = 1;
            mockOrderCloudClient.Suppliers.ListAsync<Supplier>(page: 1, pageSize: 100).Returns(listSupplierResponse);

            var allSuppliers = await ListAllAsync.List(page => mockOrderCloudClient.Suppliers.ListAsync<Supplier>(page: page, pageSize: 100));

            allSuppliers.Count.Should().Be(listSupplierResponse.Result.Items.Count);
        }


        [Test]
        public async Task ListAll_MultiplePages_Working()
        {
            Task<ListPage<Shipment>> listShipmentResponse;

            //Setup Shipment Response using AutoFixture
            ListPage<Shipment> mockListMultiPageShipment = _fixture
                    .Build<ListPage<Shipment>>()
                    .With(x => x.Meta, new ListPageMeta() { TotalPages = 5 })
                    .With(x => x.Items, _fixture.CreateMany<Shipment>(100).ToList())
                    .Create();
            listShipmentResponse = _fixture.Create<Task<ListPage<Shipment>>>();
            listShipmentResponse.Result.Meta.TotalPages = 5;
            ((List<Shipment>)listShipmentResponse.Result.Items).AddRange(mockListMultiPageShipment.Items);

            mockOrderCloudClient.Shipments.ListAsync<Shipment>(pageSize: 20).Returns(listShipmentResponse, listShipmentResponse, listShipmentResponse, listShipmentResponse);


            var allSuppliers = await ListAllAsync.List(page => mockOrderCloudClient.Shipments.ListAsync<Shipment>(pageSize: 20));
            listShipmentResponse.Result.Meta.Page.Should().BeGreaterThan(1); //expect multiple pages with multiple shipment responses.
            allSuppliers.Count.Should().BeGreaterThan(100);
        }

        [Test]
        public async Task ListWithFacets_Working()
        {
            Task<ListPageWithFacets<Product>> response = _fixture.Create<Task<ListPageWithFacets<Product>>>();
            response.Result.Meta.TotalPages = 1;

            mockOrderCloudClient.Products.ListAsync<Product>(pageSize: 20).Returns(response);

            List<Product> allProducts = await ListAllAsync.ListWithFacets(page => mockOrderCloudClient.Products.ListAsync<Product>(pageSize: 20));

            allProducts.Count.Should().Equals(response.Result.Items.Count);
        }
    }
}
