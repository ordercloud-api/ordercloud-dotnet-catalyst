## ISimpleCache

Caching can be a great way to improve the performance of data retrieval. For example, under the hood `[OrderCloudUserAuth]` caches a verified user's context for 5 minutes, removing the performance cost of duplicate verifications. 

However, we don't want to dictate what cache technology your app uses. For flexibilty, Catalyst provides an interface [ISimpleCache](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/master/library/OrderCloud.Catalyst/DataMovement/ISimpleCache.cs). You can register your own implementation in [Startup](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/blob/master/demo/OrderCloud.DemoWebApi/Startup.cs) and the Catalyst library will use it.   

```c#
public virtual void ConfigureServices(IServiceCollection services) {
	services.AddSingleton<ISimpleCache, MyCacheService>() 
}
```

We've also provided implementations for a couple cache technologies that we like. 
- [LazyCache](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/blob/master/demo/OrderCloud.DemoWebApi/Services/LazyCacheService.cs), which has the advange of requiring no set up.
- [Redis](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/blob/master/demo/OrderCloud.DemoWebApi/Services/RedisCacheService.cs), which is more complex but stays consistent if you scale your server to multiple instances. 

If you don't see a method on the interface you were hoping for, please open an issue.
