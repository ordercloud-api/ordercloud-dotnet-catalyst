# ordercloud-dotnet-catalyst

[![OrderCloud.Catalyst](https://img.shields.io/nuget/v/OrderCloud.AzureApp.svg?maxAge=3600)](https://www.nuget.org/packages/OrderCloud.AzureApp/)

Extensions and helpers for building ASP.NET Core 3.1 API apps and WebJobs, typically hosted in Azure App Services, that integrate with the OrderCloud.io e-commerce platform.

### User Authentication


Use Ordercloud's Authentication scheme in your own API's. 

```c#
[HttpGet("hello")]   
[OrderCloudUserAuth]
public string Hello([FromBody] Thing thing) {
    return $"Hello {UserContext.FirstName} {UserContext.LastName}";  
}
```

[More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/Auth/UserAuth)

### Webhook Authentication 


Securely recieve push notifications of events from the Ordercloud Platform. 

```c#
[HttpPost("webhook")]
[OrderCloudWebhookAuth]
public object HandleAddressSave([FromBody] WebhookPayloads.Addresses.Save<MyConfigData> payload) {
    ...
}
```

[More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/Auth/WebhookAuth)

### Listing All Pages


If OrderCloud's limit of 100 records per page is a pain point. 

```c#
var client = new OrderCloudClient(...);
var orders = client.Orders.ListAllAsync();
```

[More Details](./library/OrderCloud.Catalyst/DataMovement/ListAllAsync)

### Proxying Platform List Calls


Receive list requests to your API with user defined filters, search, paging, and sorting.

```c#
[HttpGet("orders"), OrderCloudUserAuth(ApiRole.Shopper)]
public async Task<ListPage<Order>> ListOrders(IListArgs args)
{
    // Read or modify args here

    var orders = await _oc.Orders.ListAsync(OrderDirection.Incoming,
        page: args.Page,
        pageSize: args.PageSize,
        sortBy: string.Join(',', args.SortBy),
        search: args.Search,
        searchOn: args.SearchOn,
        filters: args.ToFilterString());
    return orders;
}
```

[More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/Models/ListOptions)

### Caching 

Use Redis or LazyCache. Or, define your own implementation of ISimpleCache. 

```c#
private ISimpleCache _cache;

[HttpGet("thing")]
public Thing GetThing(string thingID) {
    var key = $"thing-{thingID}";
    var timeToLive = TimeSpan.FromMinutes(10);
    var thing = await _cache.GetOrAddAsync(key, timeToLive, () database.GetThing(thingID));
    return thing;
}

[HttpPut("thing")]
public Thing EditThing(string thingID) {
    var key = $"thing-{thingID}";
    await _cache.RemoveAsync(thingID);
    return await database.EditThing(thingID);
}
```

[More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/DataMovement/Caching)

### Throttler 


A perfomance helper for multiple async function calls.

```c# 
var cars = new List<Car>();

var maxConcurency = 20;
var minPause = 100 // ms
var carOwners = await Throttler.RunAsync(cars, minPause, maxConcurency, car => apiClient.GetCarOwner(car.ID);
```

[More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/DataMovement/Throttler)

### Error Handling  


Handle API errors, including unexpected ones, with a standard JSON response structure. Define your own errors. 

```c#
public class SupplierOnlyException : CatalystBaseException
{
    public SupplierOnlyException() : base("SupplierOnly", 403, "Only Supplier users may perform this action.") { }
}

....

if (UserContext.UserType != "Supplier") {
    throw new SupplierOnlyException();
}
```

[More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/Errors)


### API StartUp


Remove some of the boilerplate code of starting up a new API project 

```c#
public class Program
{
    public static void Main(string[] args)
    {
        CatalystWebHostBuilder.CreateWebHostBuilder<Startup, AppSettings>(args).Build().Run();
    }
}

public class Startup
{
    private readonly AppSettings _settings;

    public Startup(AppSettings settings) {
        _settings = settings;
    }

    public virtual void ConfigureServices(IServiceCollection services) {
        services
            .ConfigureServices()
            .AddSingleton<ISimpleCache, LazyCacheService>()
            .AddOrderCloudWebhookAuth(opts => opts.HashKey = _settings.OrderCloudSettings.WebhookHashKey)
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        CatalystApplicationBuilder.CreateApplicationBuilder(app, env);
    }
}
```

[More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/Startup)

### Testing helpers


If you are writing an integration test that hits an endpoint marked with `[OrderCloudUserAuth]`, you'll need to pass a properly formatted JWT token in the Authorization header, otherwise the call will fail. Fake tokens are a bit tedious to create, so `OrderCloud.Catalyst` provides a helper: 

```c#
var token = FakeOrderCloudToken.Create("my-client-id");
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);
```

### What else?


`OrderCloud.Catalyst` is a continuous work in progress based entirely on developer feedback. If you're building solutions for OrderCloud.io using ASP.NET Core and find a particular task difficult or tedious, we welcome you to [suggest a feature](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/issues/new) for inclusion in this library. 
