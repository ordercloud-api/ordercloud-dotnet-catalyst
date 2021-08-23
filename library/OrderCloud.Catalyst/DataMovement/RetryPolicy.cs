using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{   
	/// <summary>
	/// Stores info about how many retries to attempt and how long to pause before each. 
	/// </summary>	
	public class RetryPolicy
	{
		private readonly List<int> _retryBackoffScheduleInMS;

		/// <summary>
		/// Stores info about how many retries to attempt and how long to pause before each.
		/// <param name="retryBackoffScheduleInMS">A list with an entry for each retry attempt. Value is the pause time in milliseconds.</param>
		/// </summary>	
		public RetryPolicy(List<int> retryBackoffScheduleInMS)
		{
			_retryBackoffScheduleInMS = retryBackoffScheduleInMS;
		}

		/// <summary>
		/// Runs delagate function and will attempt retries for HTTP error responses 408, 429, 500+.
		/// </summary>
		public async Task<TResult> RunWithRetries<TResult>(Func<Task<TResult>> action)
		{
			int tryCount = 0;
			while (true)
			{
				try
				{
					return await action();
				}
				catch (FlurlHttpException ex)
				{
					var retryableError =
						ex.StatusCode >= 500 ||  // anything 500 or over
						ex.StatusCode == 408 ||  // server down 
						ex.StatusCode == 429;    // too many requests
					if (!retryableError || tryCount >= _retryBackoffScheduleInMS.Count)
					{
						throw;
					}
					await Task.Delay(_retryBackoffScheduleInMS[tryCount]);
					tryCount++;
				}
			}
		}
	}
}
