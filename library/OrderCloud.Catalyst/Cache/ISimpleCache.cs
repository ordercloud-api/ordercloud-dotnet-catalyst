using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public interface ISimpleCache
	{
		Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory, TimeSpan expireAfter);
		void Remove(string key);
	}
}
