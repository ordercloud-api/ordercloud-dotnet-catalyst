# ordercloud-dotnet-catalyst

[![OrderCloud.Catalyst](https://img.shields.io/nuget/v/OrderCloud.AzureApp.svg?maxAge=3600)](https://www.nuget.org/packages/OrderCloud.AzureApp/)

Extensions and helpers for building ASP.NET Core 3.1 API apps and WebJobs, typically hosted in Azure App Services, that integrate with the OrderCloud.io e-commerce platform.

## OrderCloud User Authentication

When a user authenticates and acquires an access token from OrderCloud.io, typically in a front-end web or mobile app, that token can be used in your custom endpoints to verify the user's identity and roles. Here are the steps involved:

### 1. Register OrderCloud user authentication in your [`Startup`](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup) class. You must include one or more OrderCloud.io client IDs identifying your app.

```c#
public virtual void ConfigureServices(IServiceCollection services) {
    services.AddAuthentication()
        .AddOrderCloudUser(opts => opts.AddValidClientIDs("my-client-id"));

    ...
}
```

#### 2. Mark any of your controllers or action  methods with `[OrderCloudUserAuth]`.

Optionally, You may provide one or more required roles in this attribute, any one of which the user must be assigned in order for authorization to succeed.

```c#
[HttpGet]
[OrderCloudUserAuth(ApiRole.Shopper, ApiRole.OrderReader, ApiRole.OrderAdmin)]
public Thing Get(string id) {
    ...
}

[HttpPut]
[OrderCloudUserAuth(ApiRole.OrderAdmin)]
public void Edit([FromBody] Thing thing) {
    ...
}
```

#### 3. In your front-end app, anywhere you call one of your custom endpoints, pass the OrderCloud.io access token in a request header.

```
Authorization: Bearer my-ordercloud-token
```

## OrderCloud Webhook Authentication

One of the most common ways to integrate with OrderCloud.io is via webhooks, where your custom endpoints are called directly by OrderCloud, rather than a user app, when some event occurs within the platform. When you configure a webhook, you provide a secret key that is used by OrderCloud to create a hash of the request body and send it in the `X-oc-hash` header. Your custom endpoint can then check this hash to ensure the authenticity of the call. Here are the steps involved:

#### 1. Register OrderCloud webhook authentication in your [`Startup`](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup) class.

You must include your secret key here.

```c#
public virtual void ConfigureServices(IServiceCollection services) {
    services.AddAuthentication()
        .AddOrderCloudWebhooks(opts => opts.HashKey = "my-secret-key");

    ...
}
```

#### 2. Mark any of your controllers or action  methods with `[OrderCloudWebhookAuth]`.

```c#
[Route("webhook")]
[OrderCloudWebhookAuth]
public object HandleAddressSave([FromBody] WebhookPayloads.Addresses.Save<MyConfigData> payload) {
    ...
}
```

Webhook payload types (such as `WebhookPayloads.Addresses.Save` above) are defined in the [OrderCloud.io .NET SDK](https://github.com/ordercloud-api/ordercloud-dotnet-sdk).

## Dependency injection helpers

If you're using [dependency injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection), `OrderCloud.Catalyst` provides a few extension methods you might find useful.

`IWebHostBuilder.UseAppSettings<T>` allows you to inject a custom app settings object, populated from any [configuration source](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration), into any service, or even your `Startup` class. This should be called in your `Program` class where you configure the `WebHost`:


```c#
WebHost.CreateDefaultBuilder(args)
    .UseAppSettings<AppSettings>() // call before UseStartup to allow injecting AppSettings into Startup
    .UseStartup<Startup>()
    .Build();
```

Note that this is very similar to the [Options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) in how it maps configuration settings to your `AppSettings` class, except it bypasses the `IOptions<T>` indirection and allows you to inject `AppSettings` directly.

`IServiceCollection.AddServicesByConvention` is a DI helper that allows you to register many services in a given assembly and (optionally) namespace by naming convention. Call this in your [Startup](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup) class:

```c#
protected virtual void RegisterServices(IServiceCollection services) {
    services.AddServicesByConvention(typeof(IMyService).Assembly, typeof(IMyService).Namespace);
    ...
}
```

This call will scan the assembly/namespace, and for every interface `IServiceName` with an implementation `ServiceName`, the following is called implicitly:

```c#
services.AddTransient<IServiceName, ServiceName>();
```
## ISimpleCache 
Caching can be a great way to improve the performance of data retrieval. For example, under the hood `[OrderCloudUserAuth]` caches a verified user's context for 5 minutes, removing the performance cost of duplicate verifications. 

However, we don't want to dictate what cache technology your app uses. For flexibilty, Catalyst provides an interface [ISimpleCache](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/master/library/OrderCloud.Catalyst/DataMovement/ISimpleCache.cs). You can register your own implementation in [Startup](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/blob/master/demo/OrderCloud.DemoWebApi/Startup.cs) and the Catalyst library will use it.   

We've also provided example implementations for a couple cache technologies that we like. 
- [LazyCache](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/blob/master/demo/OrderCloud.DemoWebApi/Services/LazyCacheService.cs), which has the advange of requiring no set up.
- [Redis](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/blob/master/demo/OrderCloud.DemoWebApi/Services/RedisService.cs), which is more complex but stays consistent if you scale your server to multiple instances. 

The ISimpleCache interface is ... simple. If you don't see a method you were hoping for, please open an issue. 


## Testing helpers

If you are writing an integration test that hits an endpoint marked with `[OrderCloudUserAuth]`, you'll need to pass a properly formatted JWT token in the Authorization header, otherwise the call will fail. Fake tokens are a bit tedious to create, so `OrderCloud.Catalyst` provides a helper: 

```c#
var token = FakeOrderCloudToken.Create("my-client-id");
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);
```

## What else?

`OrderCloud.Catalyst` is a continuous work in progress based entirely on developer feedback. If you're building solutions for OrderCloud.io using ASP.NET Core and find a particular task difficult or tedious, we welcome you to [suggest a feature](https://github.com/ordercloud-api/ordercloud-dotnet-sdk-extensions/issues/new) for inclusion in this library. 
