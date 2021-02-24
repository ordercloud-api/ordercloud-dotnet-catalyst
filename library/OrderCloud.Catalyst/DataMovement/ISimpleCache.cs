using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// Improve performance by caching data that are expensive to compute and unlikely to change over a given period of time. 
	/// </summary>
	public interface ISimpleCache
	{
		/// <summary>
		/// Get the value directly from the cache if it exists. If not, call addItemFactory() and cache the result for future use.
		/// </summary>
		/// <param name="key">Unique key pointing to a value in the cache</param>
		/// <param name="expireAfter">Time before the cache is cleared. Also called "Time to Live"</param>
		/// <param name="addItemFactory">A function to calculate the value fully.</param>
		/// <returns></returns>
		Task<T> GetOrAddAsync<T>(string key, TimeSpan expireAfter, Func<Task<T>> addItemFactory);
		/// <summary>
		/// Remove the value from the cache. 
		/// </summary>
		/// <param name="key">Unique key pointing to a value in the cache</param>
		Task RemoveAsync(string key);
	}
}
