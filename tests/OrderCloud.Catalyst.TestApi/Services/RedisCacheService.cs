using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OrderCloud.Catalyst;
using StackExchange.Redis;

namespace OrderCloud.Catalyst.TestApi
{
	// Redis (https://redis.io/) is a distributed in-memory database used for many things, including caching.  
	// You can host a Redis DB in Azure https://docs.microsoft.com/en-us/azure/azure-cache-for-redis/cache-dotnet-core-quickstart. 
	// Install a free local version for
	// Linux-ish - https://redis.io/download
	// Windows - https://redislabs.com/ebook/appendix-a/a-3-installing-on-windows/a-3-2-installing-redis-on-window/
	// To connect you'll need to add two settings to this project, a connection string (e.g. "localhost:6379") and a databaseID (an integer 0-15);
	public class RedisService : ISimpleCache
	{
		// Building the IDatabase property has performance overhead, so only one instance of this class should exist in your project. 
		private readonly IDatabase _db;
		private readonly TestSettings _settings;

		public RedisService(TestSettings settings)
		{
			_settings = settings;
			var redisConfig = ConfigurationOptions.Parse(_settings.RedisSettings.ConnectionString);
			// Customize your databse connection here. For default settings see https://stackexchange.github.io/StackExchange.Redis/Configuration.html

			_db = ConnectionMultiplexer.Connect(redisConfig).GetDatabase(_settings.RedisSettings.DatabaseID);
		}

		public async Task<T> GetOrAddAsync<T>(string key, TimeSpan expireAfter, Func<Task<T>> calculateFunc)
		{
			PrefixKey(ref key);
			string cachedValue;
			try
			{
				cachedValue = (await _db.StringGetAsync(key)).ToString();
			}
			catch
			{
				return await calculateFunc(); // if the redis cache is down, fallback to calculating the value fully.
			}
			if (cachedValue == null)
			{
				var calculatedValue = await calculateFunc();
				var str = JsonConvert.SerializeObject(calculatedValue);
				await _db.StringSetAsync(key, str, expireAfter, When.Always, CommandFlags.FireAndForget);
				return calculatedValue;
			}
			return JsonConvert.DeserializeObject<T>(cachedValue);
		}

		public async Task RemoveAsync(string key)
		{
			PrefixKey(ref key);
			await _db.KeyDeleteAsync(key, CommandFlags.FireAndForget);
		}

		// This prevents incompatible cache data from causing problems if you change how you use Redis in a release.
		private void PrefixKey(ref string key)
		{
			key = $"{_settings.EnvironmentSettings.BuildNumber}-{key}";  
		}
	}
}
