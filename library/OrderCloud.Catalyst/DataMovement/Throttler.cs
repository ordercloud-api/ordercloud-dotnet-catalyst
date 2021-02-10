using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// Helper class for throttling concurrent async tasks.
	/// </summary>
    public static class Throttler
    {
		/// <summary>
		/// Perform concurrent asynchronous work on a set of data, but limit concurrency to some maximum number of running tasks.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <param name="doWorkAsync"></param>
		/// <param name="maxConcurrent">Maximum number of tasks that can be running concurrently at any given time. Default is 50.</param>
		/// <returns></returns>
	    public static async Task ForEachAsync<T>(IEnumerable<T> data, Func<T, Task> doWorkAsync, int maxConcurrent = 50) {
			var tasks = new List<Task>();
			foreach (var x in data) {
				if (tasks.Count >= maxConcurrent) { // do a quick check first
					// count running tasks. if at or over limit, wait for one (or more) to complete before letting the next one in
					while (tasks.Count(t => !t.IsCompleted && !t.IsFaulted) > maxConcurrent)
						await Task.WhenAny(tasks).ConfigureAwait(false);
				}
				tasks.Add(doWorkAsync(x));
			}
			await Task.WhenAll(tasks).ConfigureAwait(false);
		}
    }
}
