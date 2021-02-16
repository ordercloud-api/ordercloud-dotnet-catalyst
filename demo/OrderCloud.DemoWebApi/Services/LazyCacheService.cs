using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LazyCache;

namespace OrderCloud.DemoWebApi.Services
{
	public class LazyCacheService : ISimpleCache
	{
		private readonly IAppCache _cache = new CachingService();

		public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, TimeSpan expireAfter) 
			=> await _cache.GetOrAddAsync(key, addItemFactory, expireAfter);

		public void Remove(string key)
			=> _cache.Remove(key);
	}
}
