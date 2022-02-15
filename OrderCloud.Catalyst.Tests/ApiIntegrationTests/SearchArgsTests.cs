using AutoFixture.NUnit3;
using Flurl.Http;
using NUnit.Framework;
using OrderCloud.Catalyst.TestApi;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests
{
	[TestFixture]
	public class SearchArgsTests
	{
		private async Task<IFlurlResponse> QuerySearchArgsRoute(string query)
		{
			return await TestFramework.Client.Request($"demo/searchargs?{query}").GetAsync();
		}

		[TestCase("", SearchType.AnyTerm)]
		[TestCase("searchType=AnyTerm", SearchType.AnyTerm)]
		[TestCase("searchType=AllTermsAnyField", SearchType.AllTermsAnyField)]
		[TestCase("searchType=AllTermsSameField", SearchType.AllTermsSameField)]
		[TestCase("searchType=ExactPhrase", SearchType.ExactPhrase)]
		[TestCase("searchType=ExactPhrasePrefix", SearchType.ExactPhrasePrefix)]
		public async Task search_type_should_deserialize_if_valid(string query, SearchType expectedType)
		{
			var response = await QuerySearchArgsRoute(query);
			Assert.AreEqual(200, response.StatusCode);
			var args = await response.GetJsonAsync<SearchArgs<ExampleModel>>();
			Assert.AreEqual(expectedType, args.SearchType);
		}

		[Test]
		[AutoData]
		public async Task search_type_should_throw_error_if_not_valid(string randomString)
		{
			var response = await QuerySearchArgsRoute($"searchType={randomString}");
			await response.ShouldHaveFirstApiError("InvalidRequest", 400, "searchType must be one of: AnyTerm, AllTermsAnyField, AllTermsSameField, ExactPhrase, ExactPhrasePrefix");
		}
	}
}
