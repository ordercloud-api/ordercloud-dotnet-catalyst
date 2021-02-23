using OrderCloud.SDK;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OrderCloud.Catalyst.DataMovement
{
    /// <summary>
    /// Helper class for retrieving all items for a specific resource.
    /// </summary>
    public class ListAllAsync
    {
        /// <summary>
		/// Retrieve all pages and items for a specific resource.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="listFunc">Delegate representing the function call being attempted. The resulting list will be the type specified in this parameter</param>
		/// <returns>Task<List<T>>. Determined by the listFunc passed in</returns>
        public static async Task<List<T>> List<T>(Func<int, Task<ListPage<T>>> listFunc)
        {
            var pageTasks = new List<Task<ListPage<T>>>();
            var totalPages = 0;
            int totalCount = 0;
            int totalCountCutoff = 10000;
            var i = 1;
            do
            {
                pageTasks.Add(listFunc(i++));
                var running = pageTasks.Where(t => !t.IsCompleted && !t.IsFaulted).ToList();
                if (running.Count == 0 && pageTasks?.FirstOrDefault()?.Result?.Meta?.TotalPages != null)
                {
                    totalPages = pageTasks.FirstOrDefault().Result.Meta.TotalPages;
                    totalCount = pageTasks.FirstOrDefault().Result.Meta.TotalCount;
                }
                if (totalCount > totalCountCutoff)
                {
                    ListPage<T> result = ListAllByIDAsync(running);

                }
                else if (totalPages == 0 || running.Count >= 16) // throttle parallel tasks at 16
                {
                    var result = await await Task.WhenAny(running);
                    totalPages = result.Meta.TotalPages; //Set total number of pages based on returned Meta.
                    totalCount = result.Meta.TotalCount;

                }

            } while (i <= totalPages);
            var data = (
                from finalResult in await Task.WhenAll(pageTasks) //When all pageTasks are complete, save items in data variable.
                from item in finalResult?.Items
                select item).ToList();
            return data;
        }

        private static async ListPage<T> ListAllByIDAsync<T>(List<Task<ListPage<T>>> running)
        {
            var lastID = "";
            var retryPolicy = Policy.Handle<OrderCloudException>(ex => ex.HttpStatus == System.Net.HttpStatusCode.RequestTimeout || ex.HttpStatus == System.Net.HttpStatusCode.InternalServerError).WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(5));

            var list = await retryPolicy.ExecuteAsync(async () => await await Task.WhenAny(running));
            var allItems = list.Items.ToList();
            if (allItems.Count() > 0)
                lastID = (string)(list.Items.Last()?.GetType().GetProperty("ID").GetValue(list.Items.Last(), null));

            do
            {
                try
                {
                    if (allItems.Count() < list.Meta.TotalCount)
                    {
                        filtersArr.Add($"&ID=>{lastID}");
                        var nextList = await retryPolicy.ExecuteAsync(async () => await await Task.WhenAny(running));

                        if (nextList.Items.Count == 0)
                        {
                           // if total count of items has changed and there are no more items to list, exit block
                            incomplete = false;
                        }

                        allItems.AddRange(nextList.Items);
                        // increment by lastID instead of page to improve performance
                        lastID = nextList.Items.Last().ID;
                    }
                    else
                    {
                        incomplete = false;
                    }

                }
                catch (OrderCloudException ex)
                {

                }
            } while (incomplete);
        }

        /// <summary>
        /// Retrieve all pages and items with facets for a specific resource.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listFunc">Delegate representing the function call being attempted. The resulting list will be the type specified in this parameter</param>
        /// <returns>Task<List<T>>. Determined by the listFunc passed in</returns>
        public static async Task<List<T>> ListWithFacets<T>(Func<int, Task<ListPageWithFacets<T>>> listFunc)
        {
            var pageTasks = new List<Task<ListPageWithFacets<T>>>();
            var totalPages = 0;
            var i = 1;
            do
            {
                pageTasks.Add(listFunc(i++));
                var running = pageTasks.Where(t => !t.IsCompleted && !t.IsFaulted).ToList();
                if (!running.Any()) { continue; }
                if (totalPages == 0 || running.Count >= 16) // throttle parallel tasks at 16
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
