﻿using AutoFixture;
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
			mockOrderCloudClient.Suppliers.ListAsync().ReturnsForAnyArgs(listSupplierResponse);

			var allSuppliers = await mockOrderCloudClient.Suppliers.ListAllAsync();

			allSuppliers.Count.Should().Be(listSupplierResponse.Result.Items.Count);
		}

		[Test, AutoNSubstituteData]
		public async Task ListAll_MultiplePages_Working()
		{
			var mockResponse = BuildListPage<Shipment>(5);
			mockOrderCloudClient.Shipments.ListAsync().ReturnsForAnyArgs(mockResponse);
			var allSuppliers = await mockOrderCloudClient.Shipments.ListAllAsync();
			allSuppliers.Count.Should().Be(500);
		}


		[Test, AutoNSubstituteData]
		public async Task ListWithFacets_Working(Task<ListPageWithFacets<Product>> response)
		{
			response.Result.Meta.TotalPages = 1;

			mockOrderCloudClient.Products.ListAsync().ReturnsForAnyArgs(response);

			List<Product> allProducts = await mockOrderCloudClient.Products.ListAllAsync();

			allProducts.Count.Should().Equals(response.Result.Items.Count);
		}

		//Setup Shipment Response using AutoFixture
		public ListPage<T> BuildListPage<T>(int totalPages)
		{
			return _fixture
					.Build<ListPage<T>>()
					.With(x => x.Meta, new ListPageMeta() { TotalPages = totalPages })
					.With(x => x.Items, _fixture.CreateMany<T>(100).ToList())
					.Create();
		}
	}
}
