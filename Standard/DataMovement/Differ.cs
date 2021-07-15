using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// Useful for comparing 2 large sets of raw source data (strings), one representing "current" data and one representing "previous" data, and quickly identifying
	/// the differences. Example: You want to update an OrderCloud catalog based on 100,000 rows of CSV source data that is updated nightly. Use Differ to quickly
	/// reduce the set to only the rows that changed since yesterday.
	/// </summary>
	public class Differ
	{
		// ConnectionMultiplexer should be reused heavily, regardless of how Differ is used. So do one static instance per connection string
		// https://stackexchange.github.io/StackExchange.Redis/Basics
		private static ConcurrentDictionary<string, ConnectionMultiplexer> _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();

		private IDatabase GetDb() => _connections.GetOrAdd(RedisConnectionString, ConnectionMultiplexer.Connect(RedisConnectionString)).GetDatabase();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="redisConnectionString">Required.</param>
		/// <param name="dataKeyPrefix">Required. Prefix used for Redis keys to isolate this job/application from other keys in the database.
		/// It is recommended to use alphanumeric characters and hyhens only.
		/// </param>
		public Differ(string redisConnectionString, string dataKeyPrefix) {
			RedisConnectionString = redisConnectionString ?? throw new ArgumentNullException(nameof(redisConnectionString));
			DataKeyPrefix = dataKeyPrefix ?? throw new ArgumentNullException(nameof(dataKeyPrefix));
		}

		/// <summary>
		/// Required.
		/// </summary>
		public string RedisConnectionString { get; }

		/// <summary>
		/// Required. Used as a Redis key prefix to isolate everything in this Differ instance from other keys in the database.
		/// Recommended to use alphanumeric characters and hyhens only.
		/// </summary>
		public string DataKeyPrefix { get; }

		/// <summary>
		/// Load new data as "current" set and (optionally) move existing "current" set to "previous".
		/// </summary>
		/// <param name="stateId">Identifies the current state of the entire data set. A file timestamp is ideal.
		/// MUST be different on EACH data load that represents a new state of the data than the previous load.
		/// Use the same value only if you want to redo a failed load, for example.</param>
		/// <param name="data">New "current" data to load</param>
		/// <param name="resetPrevious">If true, existing "current" data becomes "previous" data upon successful load of new data.
		/// By default (when null), "previous" is overwritten only if stateId is different than the last run, which is normally appropriate.
		/// </param>
		public async Task LoadCurrentAsync(string stateId, IEnumerable<string> data, bool? resetPrevious = null) {
			var db = GetDb();

			try {
				await db.StringSetAsync(TempIdKey, stateId);
				await Throttler.RunAsync(data, 0, 1000, row => db.SetAddAsync(TempDataKey, row)).ConfigureAwait(false);

				// data load into temp keys is done. the rest is quick key swaps, do them all transactionally.

				resetPrevious = resetPrevious ?? (stateId != await db.StringGetAsync(CurrIdKey).ConfigureAwait(false));
				resetPrevious = resetPrevious.Value
				                && await db.KeyExistsAsync(CurrIdKey)
				                && await db.KeyExistsAsync(CurrDataKey);

				await GetDb().TransactAsync(q => {
					if (resetPrevious == true) {
						// move current to previous
						q.Enqueue(tran => tran.KeyRenameAsync(CurrIdKey, PrevIdKey));
						q.Enqueue(tran => tran.KeyRenameAsync(CurrDataKey, PrevDataKey));
					}
					// move temp to current
					q.Enqueue(tran => tran.KeyRenameAsync(TempIdKey, CurrIdKey));
					q.Enqueue(tran => tran.KeyRenameAsync(TempDataKey, CurrDataKey));
				});
			}
			finally {
				// if any temp keys are hanging around, clean them up
				await db.KeyDeleteAsync(TempIdKey).ConfigureAwait(false);
				await db.KeyDeleteAsync(TempDataKey).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Overwrite "previous" set with "current" set. Seldom needed; LoadCurrentDataAsync with resetPreviousData=true handles this for typical scheduled jobs.
		/// </summary>
		/// <returns></returns>
		public Task MoveCurrentToPreviousAsync() => GetDb().TransactAsync(q => q
			.Enqueue(tran => tran.KeyRenameAsync(CurrIdKey, PrevIdKey))
			.Enqueue(tran => tran.KeyRenameAsync(CurrDataKey, PrevDataKey)));

		/// <summary>
		/// Compares sets and returns all rows in "current" and not "previous".
		/// </summary>
		public Task<IList<string>> GetCurrentNotInPreviousAsync() => GetSetDffAsync(CurrDataKey, PrevDataKey);

		/// <summary>
		/// Compares sets and returns all rows in "previous" and not "current".
		/// </summary>
		public Task<IList<string>> GetPreviousNotInCurrentAsync() => GetSetDffAsync(PrevDataKey, CurrDataKey);

		private string CurrDataKey => $"{DataKeyPrefix}:current:data";
		private string PrevDataKey => $"{DataKeyPrefix}:previous:data";
		private string TempDataKey => $"{DataKeyPrefix}:temp:data";
		private string CurrIdKey => $"{DataKeyPrefix}:current:id";
		private string PrevIdKey => $"{DataKeyPrefix}:previous:id";
		private string TempIdKey => $"{DataKeyPrefix}:temp:id";

		/// <summary>
		/// Wipe out all "current" and "previous" data from the cache
		/// </summary>
		public Task ClearAllAsync() {
			var db = GetDb();
			return Task.WhenAll(
				db.KeyDeleteAsync(CurrDataKey),
				db.KeyDeleteAsync(PrevDataKey),
				db.KeyDeleteAsync(TempDataKey),
				db.KeyDeleteAsync(CurrIdKey),
				db.KeyDeleteAsync(PrevIdKey),
				db.KeyDeleteAsync(TempIdKey));
		}

		private async Task<IList<string>> GetSetDffAsync(string key1, string key2) {
			var values = await GetDb().SetCombineAsync(SetOperation.Difference, new[] { (RedisKey)key1, (RedisKey)key2 }).ConfigureAwait(false);
			return values.Select(x => x.ToString()).ToList();
		}
	}

	/// <summary>
	/// Useful for comparing 2 large sets of raw source data (strings), one representing "current" data and one representing "previous" data, and quickly identifying
	/// the differences. You provide ParseRow and GetId functions, which help determine whether a given diff is a Create, Update, or Delete.
	///  Example: You want to update an OrderCloud catalog based on 100,000 rows of CSV source data that is updated nightly. Use Differ to quickly
	/// reduce the set to only the rows that changed since yesterday.
	/// </summary>
	/// <typeparam name="T">A type that can be parsed from the raw string data. Should contain some kind of unique ID field.</typeparam>
	public class Differ<T> : Differ
	{
		/// <param name="redisConnectionString"></param>
		/// <param name="dataKeyPrefix"></param>
		public Differ(string redisConnectionString, string dataKeyPrefix) : base(redisConnectionString, dataKeyPrefix) { }

		/// <summary>
		/// A mapping function to create an object of type T from a string in the data set. Keep it simple and use only data
		/// that can be parsed directly from the string. Decorate object with data from other sources in later steps of the process, if necessary.
		/// </summary>
		public Func<string, T> ParseRow { get; set; }

		/// <summary>
		/// A function to get a unique identifier from an object of type T. Used to determine whether a row from the set diff is new, deleted, or updated.
		/// </summary>
		public Func<T, string> GetId { get; set; }

		/// <summary>
		/// Get all differences between the 2 sets. Each result contains Previous and Current value, parsed to type T, and indicates whether the change
		/// is a Create, Update, or Delete.
		/// </summary>
		public async Task<IEnumerable<DiffResult<T>>> GetDiffsAsync() {
			if (ParseRow == null) throw new ArgumentNullException(nameof(ParseRow), "ParseRow fuction must be set on Differ instance.");
			if (GetId == null) throw new ArgumentNullException(nameof(GetId), "GetId function must be set on Differ instance.");

			var getPrevDiffs = GetPreviousNotInCurrentAsync();
			var getCurrDiffs = GetCurrentNotInPreviousAsync();

			var results = new Dictionary<string, DiffResult<T>>();
			foreach (var row in await getPrevDiffs.ConfigureAwait(false)) {
				var prev = ParseRow(row);
				var key = GetId(prev);
				results.Add(key, new DiffResult<T> { Previous = prev });
			}
			foreach (var row in await getCurrDiffs.ConfigureAwait(false)) {
				var curr = ParseRow(row);
				var key = GetId(curr);
				if (results.TryGetValue(key, out var existing))
					existing.Current = curr;
				else
					results.Add(key, new DiffResult<T> { Current = curr });
			}

			return results.Values;
		}
	}

	public class DiffResult<T>
	{
		public T Previous { get; set; }
		public T Current { get; set; }

		public ChangeType ChangeType =>
			(Previous == null) ? ChangeType.Create :
			(Current == null) ? ChangeType.Delete :
			ChangeType.Update;
	}

	public enum ChangeType { Create, Update, Delete }

	// https://stackoverflow.com/a/48814292/62600
	public static class RedisExtensions
	{
		public class RedisCommandQueue
		{
			private readonly ITransaction _tran;
			private readonly IList<Task> _tasks = new List<Task>();

			internal RedisCommandQueue(ITransaction tran) => _tran = tran;
			internal Task CompleteAsync() => Task.WhenAll(_tasks);

			public RedisCommandQueue Enqueue(Func<ITransaction, Task> cmd) {
				_tasks.Add(cmd(_tran));
				return this;
			}
		}

		public static async Task TransactAsync(this IDatabase db, Action<RedisCommandQueue> addCommands) {
			var tran = db.CreateTransaction();
			var q = new RedisCommandQueue(tran);
			addCommands(q);
			if (await tran.ExecuteAsync())
				await q.CompleteAsync();
		}
	}
}