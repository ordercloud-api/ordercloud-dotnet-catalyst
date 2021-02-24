using OrderCloud.SDK;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OrderCloud.Catalyst
{
	/// <summary>
	/// Helper class for retrieving all items for a specific resource.
	/// </summary>
	internal static class ListAllHelper
	{
		public const int MAX_PAGE_SIZE = 100;
		public const int NUM_PARALLEL_REQUESTS = 16;

		public static async Task<List<T>> ListAllByFilterAsync<T>(Func<ListFilter, Task<ListPage<T>>> listFunc)
		{
			var items = new List<T>();
			var totalPages = 0;
			var i = 1;
			var idProperty = typeof(T).GetProperty("ID");
			var filter = new ListFilter() { PropertyName = "ID", FilterExpression = "*" };
			do
			{
				i++;
				var result = await listFunc(filter);
				items.AddRange(result.Items);
				if (totalPages == 0)
					totalPages = result.Meta.TotalPages;
				var lastID = (string) idProperty.GetValue(result.Items.Last());
				filter.FilterExpression = $">{lastID}";
			} while (i <= totalPages);
			return items;
		}


		/// <summary>
		/// Retrieve all pages and items with facets for a specific resource.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="listFunc">Delegate representing the function call being attempted. The resulting list will be the type specified in this parameter</param>
		/// <returns>Task<List<T>>. Determined by the listFunc passed in</returns>
		public static async Task<List<T>> ListAllByPageWithFacets<T>(Func<int, Task<ListPageWithFacets<T>>> listFunc)
		{
			var pageTasks = new List<Task<ListPageWithFacets<T>>>();
			var totalPages = 0;
			var i = 1;
			do
			{
				pageTasks.Add(listFunc(i++));
				var running = pageTasks.Where(t => !t.IsCompleted && !t.IsFaulted).ToList();
				if (!running.Any()) { continue; }
				if (totalPages == 0 || running.Count >= NUM_PARALLEL_REQUESTS) // throttle parallel tasks
					totalPages = (await await Task.WhenAny(running)).Meta.TotalPages;  //Set total number of pages based on returned Meta.
			} while (i <= totalPages);
			var data = (
				from finalResult in await Task.WhenAll(pageTasks) //When all pageTasks are complete, save items in data variable.
				from item in finalResult.Items
				select item).ToList();
			return data;
		}

		/// <summary>
		/// Retrieve all pages and items for a specific resource.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="listFunc">Delegate representing the function call being attempted. The resulting list will be the type specified in this parameter</param>
		/// <returns>Task<List<T>>. Determined by the listFunc passed in</returns>
		public static async Task<List<T>> ListAllByPage<T>(Func<int, Task<ListPage<T>>> listFunc)
		{
			var pageTasks = new List<Task<ListPage<T>>>();
			var totalPages = 0;
			var i = 1;
			do
			{
				pageTasks.Add(listFunc(i++));
				var running = pageTasks.Where(t => !t.IsCompleted && !t.IsFaulted).ToList();
				if (!running.Any()) { continue; }
				if (totalPages == 0 || running.Count >= NUM_PARALLEL_REQUESTS) // throttle parallel tasks
					totalPages = (await await Task.WhenAny(running)).Meta.TotalPages;  //Set total number of pages based on returned Meta.
			} while (i <= totalPages);
			var data = (
				from finalResult in await Task.WhenAll(pageTasks) //When all pageTasks are complete, save items in data variable.
				from item in finalResult.Items
				select item).ToList();
			return data;
		}
	}

}
