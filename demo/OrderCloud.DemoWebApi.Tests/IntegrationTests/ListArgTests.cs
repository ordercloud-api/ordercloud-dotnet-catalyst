using Flurl.Http;
using NUnit.Framework;
using OrderCloud.Catalyst;
using OrderCloud.SDK;
using SampleApp.WebApi.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.DemoWebApi.Tests
{
	[TestFixture]
	public class ListArgTests
	{
		private async Task<HttpResponseMessage> QueryListArgsRoute(string query)
		{
			return await TestFramework.Client.GetResponseAsync($"demo/listargs?{query}");
		}

		[TestCase("", 1)] // default page is 1
		[TestCase("pageSize=2&anything=random", 1)]
		[TestCase("page=1", 1)]
		[TestCase("page=10", 10)]
		[TestCase("page=47383", 47383)]
		[TestCase("page=15&pageSize=2&anything=random", 15)]
		public async Task page_should_deserialize_if_valid(string query, int expectedPage)
		{
			var response = await QueryListArgsRoute(query);
			response.ShouldHaveStatusCode(200);
			var args = await response.DeserializeAsync<ListArgs<ExampleModel>>();
			Assert.AreEqual(expectedPage, args.Page);
		}

		[TestCase("page=0")]
		[TestCase("page=1.1")]
		[TestCase("page=-1")]
		[TestCase("page=-3825")]
		[TestCase("page=-1&pageSize=2&anything=random")]
		[TestCase("page=-3825&pageSize=2&anything=random")]
		[TestCase("page=0x")]
		[TestCase("page=text")]
		[TestCase("page=12!&pageSize=2")]
		[TestCase("page=text&anything=random")]
		public async Task page_should_throw_error_if_invalid(string query)
		{
			var response = await QueryListArgsRoute(query);
			response.ShouldBeApiError("InvalidRequest", 400, "page must be an integer greater than or equal to 1.");
		}

		[TestCase("", 20)] // default pageSize is 20
		[TestCase("pageSize=1", 1)]
		[TestCase("pageSize=2", 2)]
		[TestCase("pageSize=43", 43)]
		[TestCase("pageSize=100", 100)]
		public async Task page_size_should_deserialize_if_valid(string query, int expectedPageSize)
		{
			var response = await QueryListArgsRoute(query);
			response.ShouldHaveStatusCode(200);
			var args = await response.DeserializeAsync<ListArgs<ExampleModel>>();
			Assert.AreEqual(expectedPageSize, args.PageSize);
		}

		[TestCase("pageSize=0")]
		[TestCase("pageSize=1.1")]
		[TestCase("pageSize=101")]
		[TestCase("pageSize=-1")]
		[TestCase("pageSize=-3825")]
		[TestCase("pageSize=0&page=2&anything=random")]
		[TestCase("pageSize=101&page=2&anything=random")]
		[TestCase("pageSize=0x")]
		[TestCase("pageSize=text")]
		[TestCase("pageSize=12!&page=2")]
		[TestCase("pageSize=text&anything=random")]
		public async Task page_size_should_throw_error_if_invalid(string query)
		{
			var response = await QueryListArgsRoute(query);
			response.ShouldBeApiError("InvalidRequest", 400, "pageSize must be an integer between 1 and 100.");
		}

		[TestCase("search=something", "something")]
		public async Task search_should_deserialize(string query, string expectedSearch)
		{
			var response = await QueryListArgsRoute(query);
			response.ShouldHaveStatusCode(200);
			var args = await response.DeserializeAsync<ListArgs<ExampleModel>>();
			Assert.AreEqual(expectedSearch, args.Search);
		}

		[TestCase("searchOn=something", "something")]
		public async Task search_on_should_deserialize(string query, string expectedSearchOn)
		{
			var response = await QueryListArgsRoute(query);
			response.ShouldHaveStatusCode(200);
			var args = await response.DeserializeAsync<ListArgs<ExampleModel>>();
			Assert.AreEqual(expectedSearchOn, args.SearchOn);
		}

		[TestCase("sortBy=RequiredField", new[] { "RequiredField" })]
		[TestCase("sortBy=!requiredfield", new[] { "!RequiredField" })]
		[TestCase("sortBy=!!!!RequiredField", new[] { "!RequiredField" })]
		[TestCase("sortBy=RequiredField,!boundedstring", new[] { "RequiredField", "!BoundedString" })]
		[TestCase("sortBy=!BoundedDecimal,boundedstring", new[] { "!BoundedDecimal", "BoundedString" })]
		[TestCase("sortBy=xp.anything.property", new[] { "xp.anything.property" })]
		public async Task sort_should_deserialize_if_valid(string query, string[] expectedSortBy)
		{
			var response = await QueryListArgsRoute(query);
			response.ShouldHaveStatusCode(200);
			var args = await response.DeserializeAsync<ListArgs<ExampleModel>>();
			for (int i = 0; i < args.SortBy.Count; i++)
			{
				Assert.AreEqual(args.SortBy[i], expectedSortBy[i]);
			}
		}

		[TestCase("sortBy=NotAField", "ExampleModel.NotAField")]
		[TestCase("sortBy=fakez,NotAField", "ExampleModel.fakez")]
		public async Task sort_should_throw_error_if_invalid(string query, string errorMessage)
		{
			var response = await QueryListArgsRoute(query);
			response.ShouldBeApiError("InvalidProperty", 400, errorMessage);
		}

		static object[] FilterCases =
		{
			new object[] { "color=red", new List<ListFilter> () { new ListFilter()
			{
				PropertyName = "color",
				FilterExpression = "red",
				FilterValues = new List<ListFilterValue>() { new ListFilterValue()
				{
					Operator = ListFilterOperator.Equal,
					Term = "red",
					WildcardPositions = new int[] {}
				}}
			}}},
			new object[] { "name=!Oli*", new List<ListFilter> () { new ListFilter()
			{
				PropertyName = "name",
				FilterExpression = "!Oli*",
				FilterValues = new List<ListFilterValue>() { new ListFilterValue()
				{
					Operator = ListFilterOperator.NotEqual,
					Term = "Oli",
					WildcardPositions = new int[] { 3 }
				}}
			}}},
			new object[] { "size=>20", new List<ListFilter> () { new ListFilter()
			{
				PropertyName = "size",
				FilterExpression = ">20",
				FilterValues = new List<ListFilterValue>() { new ListFilterValue()
				{
					Operator = ListFilterOperator.GreaterThan,
					Term = "20",
					WildcardPositions = new int[] { }
				}}
			}}}
		};

		[TestCaseSource(nameof(FilterCases))]
		public async Task filters_should_deserialize(string query, List<ListFilter> expectedFilters)
		{
			var response = await QueryListArgsRoute(query);
			response.ShouldHaveStatusCode(200);
			var args = await response.DeserializeAsync<ListArgs<ExampleModel>>();
			Assert.AreEqual(query, args.ToFilterString());
			for (int i = 0; i < args.Filters.Count; i++)
			{
				var expectedFilter = expectedFilters[i];
				var actualFilter = args.Filters[i];
				Assert.AreEqual(expectedFilter.PropertyName, actualFilter.PropertyName);
				Assert.AreEqual(expectedFilter.FilterExpression, actualFilter.FilterExpression);
				for (int j = 0; j < expectedFilter.FilterValues.Count; j++)
				{
					var expectedFilterValue = expectedFilter.FilterValues[j];
					var actualFilterValue = actualFilter.FilterValues[j];
					Assert.AreEqual(expectedFilterValue.Operator, actualFilterValue.Operator);
					Assert.AreEqual(expectedFilterValue.Term, actualFilterValue.Term);
					for (int k = 0; k < expectedFilterValue.WildcardPositions.Count; k++)
					{
						Assert.AreEqual(expectedFilterValue.WildcardPositions[k], actualFilterValue.WildcardPositions[k]);
					}
				}
			}
		}
	}
}
