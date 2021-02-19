using OrderCloud.SDK;
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
            var i = 1;
            do
            {
                pageTasks.Add(listFunc(i++));
                var running = pageTasks.Where(t => !t.IsCompleted && !t.IsFaulted).ToList();
                if (running.Count == 0 && pageTasks?.FirstOrDefault()?.Result?.Meta?.TotalPages != null)
                {
                    totalPages = pageTasks.FirstOrDefault().Result.Meta.TotalPages;
                }
                else if (totalPages == 0 || running.Count >= 16) // throttle parallel tasks at 16
                    totalPages = (await await Task.WhenAny(running)).Meta.TotalPages;  //Set total number of pages based on returned Meta.
            } while (i <= totalPages);
            var data = (
                from finalResult in await Task.WhenAll(pageTasks) //When all pageTasks are complete, save items in data variable.
                from item in finalResult?.Items
                select item).ToList();
            return data;
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
