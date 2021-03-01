using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using OrderCloud.SDK;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests
{
	class ListAllAsyncTests
	{
		private IOrderCloudClient mockOrderCloudClient;

		private Fixture _fixture;

		[SetUp]
		public void Setup()
		{
			//Setup mocks and substitutions for Http, Ordercloud, and Fixture
			_fixture = new Fixture();
			mockOrderCloudClient = Substitute.For<IOrderCloudClient>();
		}

		[Test, AutoNSubstituteData]
		public async Task ListAllWorking(Task<ListPage<Supplier>> listSupplierResponse)
		{
			//Setup Supplier Response using AutoFixture
			listSupplierResponse.Result.Meta.TotalPages = 1;
			mockOrderCloudClient.Suppliers.ListAsync<Supplier>().ReturnsForAnyArgs(listSupplierResponse);

			var allSuppliers = await mockOrderCloudClient.Suppliers.ListAllAsync<Supplier>();

			allSuppliers.Count.Should().Be(listSupplierResponse.Result.Items.Count);
		}

		[Test, AutoNSubstituteData]
		public async Task ListAll_MultiplePages_Working(ListPage<Shipment> listShipmentResponse)
		{
			//Setup Shipment Response using AutoFixture
			ListPage<Shipment> mockListMultiPageShipment = _fixture
					.Build<ListPage<Shipment>>()
					.With(x => x.Meta, new ListPageMeta() { TotalPages = 5 })
					.With(x => x.Items, _fixture.CreateMany<Shipment>(100).ToList())
					.Create();

			listShipmentResponse.Meta.TotalPages = 5;
			((List<Shipment>)listShipmentResponse.Items).AddRange(mockListMultiPageShipment.Items);

			mockOrderCloudClient.Shipments.ListAsync<Shipment>().ReturnsForAnyArgs(listShipmentResponse, listShipmentResponse, listShipmentResponse, listShipmentResponse);

			var allSuppliers = await mockOrderCloudClient.Shipments.ListAllAsync<Shipment>();
			listShipmentResponse.Meta.Page.Should().BeGreaterThan(1); //expect multiple pages with multiple shipment responses.
			allSuppliers.Count.Should().BeGreaterThan(100);
		}

		[Test, AutoNSubstituteData]
		public async Task ListWithFacets_Working(Task<ListPageWithFacets<Product>> response)
		{
			response.Result.Meta.TotalPages = 1;

			mockOrderCloudClient.Products.ListAsync<Product>().ReturnsForAnyArgs(response);

			List<Product> allProducts = await mockOrderCloudClient.Products.ListAllAsync<Product>();

			allProducts.Count.Should().Equals(response.Result.Items.Count);
		}
	}
}
