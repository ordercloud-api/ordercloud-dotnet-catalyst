using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public static async Task RunAsync<T>(IEnumerable<T> items, int minPause, int maxConcurrent, Func<T, Task> op)
        {
            using (var sem = new SemaphoreSlim(maxConcurrent))
            {
                async Task RunOneAsync(T item)
                {
                    try { await op(item); }
                    finally { sem.Release(); }
                }

                var tasks = new List<Task>();
                foreach (var item in items)
                {
                    if (tasks.Any()) // avoid pausing before the first one
                        await Task.WhenAll(sem.WaitAsync(), Task.Delay(minPause)); // wait until we're under the concurrency limit AND at least minPause has passed
                    tasks.Add(RunOneAsync(item));
                }
                await Task.WhenAll(tasks);
            }
        }

        /// <summary>
        /// Perform concurrent asynchronous work on a set of data, but limit concurrency to some maximum number of running tasks.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="doWorkAsync"></param>
        /// <param name="maxConcurrent">Maximum number of tasks that can be running concurrently at any given time. Default is 50.</param>
        /// <returns></returns>
        public static async Task<IList<TOutput>> RunAsync<TInput, TOutput>(IEnumerable<TInput> items, int minPause, int maxConcurrent, Func<TInput, Task<TOutput>> op)
        {
            using (var sem = new SemaphoreSlim(maxConcurrent))
            {
                async Task<TOutput> RunOneAsync(TInput item)
                {
                    try { return await op(item); }
                    finally { sem.Release(); }
                }

                var tasks = new List<Task<TOutput>>();
                foreach (var item in items)
                {
                    if (tasks.Any()) // avoid pausing before the first one
                        await Task.WhenAll(sem.WaitAsync(), Task.Delay(minPause)); // wait until we're under the concurrency limit AND at least minPause has passed
                    tasks.Add(RunOneAsync(item));
                }

                var result = await Task.WhenAll(tasks);
                return result.ToList();
            }
        }
    }
}
