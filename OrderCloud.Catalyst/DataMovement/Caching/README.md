## ISimpleCache

Caching can be a great way to improve the performance of data retrieval. For example, under the hood `[OrderCloudUserAuth]` caches a verified user's context for 5 minutes, removing the performance cost of duplicate verifications. 

However, we don't want to dictate what cache technology your app uses. For flexibilty, Catalyst provides an interface [ISimpleCache](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/master/library/OrderCloud.Catalyst/DataMovement/ISimpleCache.cs). You can register your own implementation in [Startup](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/blob/master/demo/OrderCloud.DemoWebApi/Startup.cs) and the Catalyst library will use it.   

```c#
public virtual void ConfigureServices(IServiceCollection services) {
	services.AddSingleton<ISimpleCache, MyCacheService>() 
}
```

We've also provided an implementation for an easy to use C# in-memory cache technology that we like, [LazyCache](./LazyCacheService.cs). If you wanted to use something like Redis, create a RedisService.cs that implments `ISimpleCache`, inject it, and `[OrderCloudUserAuth]` will use that caching technology instead. 

If you don't see a method on the interface you were hoping for, please open an issue.
