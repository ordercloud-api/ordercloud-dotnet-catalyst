using Flurl.Http;
using Microsoft.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.DataMovement
{
	internal static class SensibleRetry
	{
		private static readonly List<int> RETRY_BACKOFF_SCHEDULE_MS = new List<int> { 0, 1000, 2000, 4000 };

		public async static Task<TResult> Wrap<TResult>(Func<Task<TResult>> action)
		{
			int tryCount = 0;
			Exception mostRecent = null;
			while (tryCount < RETRY_BACKOFF_SCHEDULE_MS.Count)
			{
				try
				{
					return await action();
				} catch (FlurlHttpException ex)
				{
					mostRecent = ex;
					var retryableError =
						ex.StatusCode >= 500 ||  // 
						ex.StatusCode == 408 ||  // server down 
						ex.StatusCode == 429;    // too many requests
					if (!retryableError)
					{
						throw;
					}
					await Task.Delay(RETRY_BACKOFF_SCHEDULE_MS[tryCount]);
					tryCount++;
				}
			}
			throw mostRecent;
		}
	}
}
