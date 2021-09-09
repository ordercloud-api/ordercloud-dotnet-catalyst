using Flurl;
using Flurl.Util;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace OrderCloud.Catalyst
{
	/// <summary>
	/// Helps you get all records in one function call despite OrderCloud pagination.
	/// </summary>	
	internal static class ListAllHelper
	{
		public const int PAGE_ONE = 1;
		public const int MAX_PAGE_SIZE = 100;
		// Always sort by ID to enable id-filter paging technique 
		public const string SORT_BY = "ID";
		// Only relevant to the concurrent paging technique
		public const int MAX_NUM_PARALLEL_REQUESTS = 16;
		// For record sizes above 300 (30 pages), use last ID filter paging. Below 30 pages, use the page parameter concurrently. 
		public const int PAGE_THRESHOLD_FOR_PAGING_TECHNIQUE = 30;
		// Will be ignored by the SDK and have no effect on the request because Value is null;
		public static (string Key, object Value) IGNORED_FILTER => ("ID", null);
		public static RetryPolicy RetryPolicy = new RetryPolicy(new List<int> { 0, 1000, 2000, 4000 });

		// Technically, OrderCloud can field requests successfully with up to 2086 chars in the url. Some padding seemed safer though.
		public const int MAX_CHARS_IN_URL = 2000;
		// The "|" OR character is url encoded as "%7C", which is 3 characters. 
		public const int CHARS_IN_AN_OR = 3;

		/// <summary>
		/// Get all records of specific type from OrderCloud by requesting all list pages and combining the results.
		/// </summary>
		/// <typeparam name="T">The type of record to list</typeparam>
		/// <param name="listFunc">The list function to call repeatedly. Should have inputs of page and filter amd return a Task<ListPage<T>>>. Recommend sorting by ID and settings pageSize to 100.</param>
		/// <returns></returns>
		public static async Task<List<T>> ListAllAsync<T>(Func<int, (string Key, object Value), Task<ListPage<T>>> listFunc)
		{
			var page1 = await listFunc(1, IGNORED_FILTER);
			var id = typeof(T).GetProperty("ID");
			if (id == null || page1.Meta.TotalPages < PAGE_THRESHOLD_FOR_PAGING_TECHNIQUE)
			{
				// If you're listing by page, you don't need to add additional filters.
				return await ListAllByPage(page1, page => listFunc(page, IGNORED_FILTER));
			}
			// If you're listing by filtering, you always request page 1.
			return await ListAllByFilterAsync(page1, id, filter => listFunc(1, filter));
		}

		public static async Task ListBatchedAsync<T>(Func<(string Key, object Value), Task<ListPage<T>>> processPage)
		{
			var id = typeof(T).GetProperty("ID");
			if (id == null)
			{
				await ListBatchedByPageAsync(filter => processPage(IGNORED_FILTER));
			}
			else
			{
				await ListBatchedByFilterAsync(id, filter => processPage(filter));
			}
		}


		private static async Task ListBatchedByPageAsync<T>(Func<int, Task<ListPage<T>>> processPage)
		{
			var page = 1;
			var totalPages = 1; // placeholder until we get data
			while (page <= totalPages)
			{
				var result = await RetryPolicy.RunWithRetries(() => processPage(page));
				totalPages = result.Meta.TotalPages;
				page++;
			}
		}

		private static async Task ListBatchedByFilterAsync<T>(PropertyInfo id, Func<(string Key, object Value), Task<ListPage<T>>> processPage)
		{
			var filter = IGNORED_FILTER;
			var lastResponse = new ListPage<T> { Meta = new ListPageMeta() { TotalPages = 2 } }; // placeholder until we get data
			while (lastResponse.Meta.TotalPages > 1)
			{
				lastResponse = await RetryPolicy.RunWithRetries(() => processPage(filter));
				var lastID = id.GetValue(lastResponse.Items.Last()) as string;
				filter = ("ID", $">{lastID}");
			}
		}

		private static async Task<List<T>> ListAllByPage<T>(ListPage<T> page1, Func<int, Task<ListPage<T>>> listFunc)
		{
			var pageTasks = new List<Task<ListPage<T>>>();
			var totalPages = page1.Meta.TotalPages;
			var page = 2;
			while (page <= totalPages)
			{
				var pageTask = RetryPolicy.RunWithRetries(() => listFunc(page++));
				pageTasks.Add(pageTask);
				var running = pageTasks.Where(t => !t.IsCompleted && !t.IsFaulted).ToList();
				if (!running.Any()) { continue; }
				if (totalPages == 0 || running.Count >= MAX_NUM_PARALLEL_REQUESTS) // throttle parallel tasks
					totalPages = (await await Task.WhenAny(running)).Meta.TotalPages;  //Set total number of pages based on returned Meta.
			};
			var data = (
				from finalResult in await Task.WhenAll(pageTasks) //When all pageTasks are complete, save items in data variable.
				from item in finalResult.Items
				select item).ToList();
			data.AddRange(page1.Items);
			return data;
		}

		private static async Task<List<T>> ListAllByFilterAsync<T>(ListPage<T> page1, PropertyInfo id, Func<(string Key, object Value), Task<ListPage<T>>> listFunc)
		{
			var lastResponse = page1;
			var toReturn = page1.Items as List<T>;
			while (lastResponse.Meta.TotalPages > 1)
			{
				var lastID = id.GetValue(lastResponse.Items.Last()) as string;
				var filter = ("ID", $">{lastID}");
				lastResponse = await RetryPolicy.RunWithRetries(() => listFunc(filter));
				toReturn.AddRange(lastResponse.Items);
			}
			return toReturn;
		}

		/// <summary>
		/// Get mulitple records by providing a list of IDs. Duplicates or Not Founds will be omitted from the results without error. 
		/// </summary>
		public static async Task<List<T>> ListByIDAsync<T>(List<string> ids, Func<string, Task<ListPage<T>>> listFunc)
		{
			if (ids.Count == 0) { return new List<T>(); }
			var records = new List<T>();
			// We must break the request into chunks to avoid putting too many chars in a url.
			var idChunks = new List<List<string>> { new List<string> { } };
			var currentChunkIndex = 0;
			ids
				.Distinct() // remove duplicates
				.Aggregate(0, (charCount, id) =>
				{
					var charCountInCurrentChunk = charCount + id.Length + CHARS_IN_AN_OR;
					var idCountInCurrentChunk = idChunks[currentChunkIndex].Count + 1;
					if (charCountInCurrentChunk > MAX_CHARS_IN_URL || idCountInCurrentChunk > MAX_PAGE_SIZE)
					{
						// Start a new chunk
						currentChunkIndex++;
						idChunks.Add(new List<string> { id });
						return id.Length + CHARS_IN_AN_OR; // reset character count to 0
					}
					// Add the id to the old chunk
					idChunks[currentChunkIndex].Add(id);
					return charCountInCurrentChunk;
				});

			// Make a request for each chunk
			foreach (var idChunk in idChunks)
			{
				var filterValue = string.Join("|", idChunk);
				var response = await RetryPolicy.RunWithRetries(() =>
				{
					return listFunc(filterValue);
				});
				records.AddRange(response.Items);
			}
			return records;
		}

		public static async Task ListBatchedWithFacetsAsync<T>(Func<(string Key, object Value), Task<ListPageWithFacets<T>>> processPage)
			=> await ListBatchedAsync(async filter => DropFacets(await processPage(filter)));

		public static async Task<List<T>> ListAllWithFacetsAsync<T>(Func<int, (string Key, object Value), Task<ListPageWithFacets<T>>> listFunc)
			=> await ListAllAsync(async (page, filter) => DropFacets(await listFunc(page, filter)));

		public static async Task<List<T>> ListByIDWithFacetsAsync<T>(List<string> ids, Func<string, Task<ListPageWithFacets<T>>> listFunc)
			=> await ListByIDAsync(ids, async filterValue => DropFacets(await listFunc(filterValue)));

		/// <summary>
		/// Helper for converting ListPageWithFacets to ListPage. Drops facet data.
		/// </summary>
		private static ListPage<T> DropFacets<T>(ListPageWithFacets<T> page) => new ListPage<T>()
		{
			Items = page.Items,
			Meta = page.Meta
		};
	}
}
