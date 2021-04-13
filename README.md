# ordercloud-dotnet-catalyst

[![OrderCloud.Catalyst](https://img.shields.io/nuget/v/OrderCloud.AzureApp.svg?maxAge=3600)](https://www.nuget.org/packages/OrderCloud.AzureApp/)

Extensions and helpers for building ASP.NET Core 3.1 API apps and WebJobs that integrate with the OrderCloud e-commerce platform. Below is a list of features and how to use them. See [examples](https://github.com/ordercloud-api/dotnet-catalyst-examples) for step-by-step guides on solving commerce problems. 

`OrderCloud.Catalyst` is a continuous work in progress based entirely on developer feedback. If you're building solutions for OrderCloud using ASP.NET Core and find a particular task difficult or tedious, we welcome you to [suggest a feature](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/issues/new) for inclusion in this library. 

### User Authentication

Use Ordercloud's authentication scheme in your own APIs. [More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/Auth/UserAuth)

```c#
[HttpGet("hello"), OrderCloudUserAuth(ApiRole.Shopper)]
public string SayHello() {
    return $"Hello {_userContext.FirstName} {_userContext.LastName}";  
}
```

### Webhook Authentication 

Securely receive push notifications of events from the Ordercloud platform. [More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/Auth/WebhookAuth)

```c#
[HttpPost("webhook"), OrderCloudWebhookAuth]
public object HandleAddressSave([FromBody] WebhookPayloads.Addresses.Save<MyConfigData> payload) {
    ...
}
```

### Listing All Pages

If OrderCloud's limit of 100 records per page is a pain point. [More Details](./library/OrderCloud.Catalyst/DataMovement/ListAllAsync)

```c#
var orders = new OrderCloudClient(...).Suppliers.ListAllAsync();
```

### Proxying Platform List Calls

Receive list requests to your API with user defined filters, search, paging, and sorting. [More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/Models/ListOptions)

```c#
[HttpGet("orders"), OrderCloudUserAuth(ApiRole.Shopper)]
public async Task<ListPage<Order>> ListOrders(IListArgs args)
{
    args.Filters.Add(new ListFilter("FromCompanyID", _userContext.Buyer.ID))
    args.Filters.Add(new ListFilter("LineItemCount", ">5"))

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

### Caching 

Use Redis or LazyCache. Or, define your own implementation of ISimpleCache. [More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/DataMovement/Caching) 

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

### Throttler 

A perfomance helper for multiple async function calls. [More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/DataMovement/Throttler)

```c# 
var cars = new List<Car>();

var maxConcurency = 20;
var minPause = 100 // ms
var carOwners = await Throttler.RunAsync(cars, minPause, maxConcurency, car => apiClient.GetCarOwner(car.ID);
```

### Error Handling  

Handle API errors, including unexpected ones, with a standard JSON response structure. Define your own errors. [More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/Errors)

```c#
public class SupplierOnlyException : CatalystBaseException
{
    public SupplierOnlyException() : base("SupplierOnly", 403, "Only Supplier users may perform this action.") { }
}

....

if (_userContext.UserType != "Supplier") {
    throw new SupplierOnlyException();
}
```

### API StartUp

Remove some of the boilerplate code of starting up a new API project. [More Details](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/library/OrderCloud.Catalyst/Startup)

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
        CatalystApplicationBuilder.DefaultCatalystAppBuilder(app, env);
    }
}
```

### Testing helpers

When writing integration tests that hit an endpoint marked with `[OrderCloudUserAuth]`, you'll need to pass a properly formatted JWT token in the Authorization header, otherwise the call will fail. Fake tokens are a bit tedious to create, so `OrderCloud.Catalyst` provides a helper: 

```c#
var token = FakeOrderCloudToken.Create("my-client-id");
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);
```