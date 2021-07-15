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
		// Only relevant to the concurrent paging technique
		private const int MAX_NUM_PARALLEL_REQUESTS = 16;
		// For record sizes above 30 pages, use last ID filter paging. Below 30 pages, use the page parameter concurrently. 
		private const int PAGE_THRESHOLD_FOR_PAGING_TECHNIQUE = 30;
		// Will be ignored by the SDK and have no effect on the request because Value is null;
		private static (string Key, object Value) IGNORED_FILTER => ("ID", null);

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

		public static async Task ListBatchedAsync<T>(Func<int, Task<ListPage<T>>> processPage)
		{
			var count = 1;
			var page1 = await processPage(count);
			while (count < page1.Meta.TotalPages)
			{
				count++;
				await processPage(count);
			}
		}

		public static async Task ListBatchedWithFacetsAsync<T>(Func<int, Task<ListPageWithFacets<T>>> processPage)
		{
			var count = 1;
			var page1 = await processPage(count);
			while (count < page1.Meta.TotalPages)
			{
				count++;
				await processPage(count);
			}
		}

		/// <summary>
		/// Get all records of specific type from OrderCloud by requesting all list pages and combining the results.
		/// </summary>
		/// <typeparam name="T">The type of record to list</typeparam>
		/// <param name="listFunc">The list function to call repeatedly. Should have inputs of page and filter amd return a Task<ListPage<T>>>. Recommend sorting by ID and settings pageSize to 100.</param>
		/// <returns></returns>
		public static async Task<List<T>> ListAllWithFacetsAsync<T>(Func<int, (string Key, object Value), Task<ListPageWithFacets<T>>> listFunc)
		{
			var page1 = await listFunc(1, IGNORED_FILTER);
			var id = typeof(T).GetProperty("ID");
			if (id == null || page1.Meta.TotalPages < PAGE_THRESHOLD_FOR_PAGING_TECHNIQUE)
			{
				// If you're listing by page, you don't need to add additional filters.
				return await ListAllWithFacetsByPage(page1, page => listFunc(page, IGNORED_FILTER));
			}
			// If you're listing by filtering, you always request page 1.
			return await ListAllWithFacetsByFilterAsync(page1, id, filter => listFunc(1, filter));
		}

		private static async Task<List<T>> ListAllByFilterAsync<T>(ListPage<T> page1, PropertyInfo id, Func<(string Key, object Value), Task<ListPage<T>>> listFunc)
		{
			var data = page1.Items as List<T>;
			var page = 2;
			while (page <= page1.Meta.TotalPages)
			{
				var lastID = id.GetValue(data.Last()) as string;
				var filter = ("ID", $">{lastID}");
				var response = await listFunc(filter);
				data.AddRange(response.Items);
				page++;
			}
			return data;
		}

		private static async Task<List<T>> ListAllByPage<T>(ListPage<T> page1, Func<int, Task<ListPage<T>>> listFunc)
		{
			var pageTasks = new List<Task<ListPage<T>>>();
			var totalPages = page1.Meta.TotalPages;
			var page = 2;
			while (page <= totalPages)
			{
				pageTasks.Add(listFunc(page++));
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

		private static async Task<List<T>> ListAllWithFacetsByFilterAsync<T>(ListPageWithFacets<T> page1, PropertyInfo id, Func<(string Key, object Value), Task<ListPageWithFacets<T>>> listFunc)
		{
			var data = page1.Items as List<T>;
			var filter = IGNORED_FILTER;
			var page = 2;
			while (page <= page1.Meta.TotalPages)
			{
				var response = await listFunc(filter);
				data.AddRange(response.Items);
				var lastID = id.GetValue(response.Items.Last()) as string;
				filter = ("ID", $">{lastID}");
				page++;
			}
			return data;
		}

		private static async Task<List<T>> ListAllWithFacetsByPage<T>(ListPageWithFacets<T> page1, Func<int, Task<ListPageWithFacets<T>>> listFunc)
		{
			var pageTasks = new List<Task<ListPageWithFacets<T>>>();
			var totalPages = page1.Meta.TotalPages;
			var page = 2;
			while (page <= totalPages)
			{
				pageTasks.Add(listFunc(page++));
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


		// See https://github.com/tmenier/Flurl/blob/ce480aa1aa8ce1f2ff4ebce9f1d6eaf30b7d6d8c/src/Flurl.Http/Configuration/DefaultUrlEncodedSerializer.cs
		/// <summary>
		/// Add a new filter to an existing set of filters. All must evaluate to true.
		/// </summary>
		/// <param name="filters">An existing set of filters.</param>
		/// <param name="listFunc">A new filter that must also evaluate to true.</param>
		/// <returns></returns>
		public static string AndFilter(this object filters, (string Key, object Value) filter)
		{
			var filterList = filters?.ToKeyValuePairs()?.ToList() ?? new List<(string Key, object Value)>();
			filterList.Add(filter);
			var qp = new QueryParamCollection();
			foreach (var kv in filterList)
			{
				if (kv.Value != null)
				{
					qp.Add(kv.Key, kv.Value);
				}
			}
			return string.Join("&", qp.Select(x => $"{x.Name}={x.Value}"));
		}

		public static string GetSort<T>()
        {
			var id = typeof(T).GetProperty("ID");
			return id == null ? null : "ID";
        }
	}
}
