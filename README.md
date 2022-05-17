# ordercloud-dotnet-catalyst

[![](https://img.shields.io/nuget/v/ordercloud-dotnet-catalyst.svg?maxAge=3600)](https://www.nuget.org/packages/ordercloud-dotnet-catalyst/)

A foundational library for building OrderCloud middleware, plugins and extensions with .NET. A toolbox of helpers for authentication, performant bulk requests, error handling, jobs, project setup, ect.    

See [dotnet-catalyst-examples](https://github.com/ordercloud-api/dotnet-catalyst-examples) for a starter template of a middleware project that uses this library. Targeted guides are found there.

If you're building solutions for OrderCloud using .NET and find a particular task difficult or tedious, we welcome you to [suggest a feature](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/issues/new) for inclusion in this library. 

:warning: Versions 1.x.x have known security holes. Please only use version 2.0.1 and later. 

## Features

### 3rd Party Integrations 

Contributing Guide For Integrations -> [CONTRIBUTING.md](./OrderCloud.Catalyst/Integrations/CONTRIBUTING.md)

| Name | Project Guide | Library | Contributed By | Interfaces |
| ------------- | ------------- | ------------- | ------------- | ------------- |
| **BlueSnap** | [README](./OrderCloud.Integrations.Payment.BlueSnap) | [![](https://img.shields.io/nuget/v/OrderCloud.Integrations.Payment.BlueSnap.svg?maxAge=3600)](https://www.nuget.org/packages/OrderCloud.Integrations.Payment.BlueSnap) | OrderCloud Team | ICreditCardProcessor, ICreditCardSaver
| **CardConnect** | [README](./OrderCloud.Integrations.Payment.CardConnect) | [OrderCloud.Integrations.Payment.CardConnect](https://www.nuget.org/packages/OrderCloud.Integrations.Payment.CardConnect) | OrderCloud Team | ICreditCardProcessor, ICreditCardSaver
| **Stripe** | [README](./OrderCloud.Integrations.Payment.Stripe) | [OrderCloud.Integrations.Payment.Stripe](https://www.nuget.org/packages/OrderCloud.Integrations.Payment.Stripe) | OrderCloud Team | ICreditCardProcessor, ICreditCardSaver
| **EasyPost** | [README](./OrderCloud.Integrations.Shipping.EasyPost) | [OrderCloud.Integrations.Shipping.EasyPost](https://www.nuget.org/packages/OrderCloud.Integrations.Shipping.EasyPost) | OrderCloud Team | IShippingRatesCalculator
| **Fedex** | [README](./OrderCloud.Integrations.Shipping.Fedex) | [OrderCloud.Integrations.Shipping.Fedex](https://www.nuget.org/packages/OrderCloud.Integrations.Shipping.Fedex) | OrderCloud Team | IShippingRatesCalculator
| **UPS** | [README](./OrderCloud.Integrations.Shipping.UPS) | [OrderCloud.Integrations.Shipping.UPS](https://www.nuget.org/packages/OrderCloud.Integrations.Shipping.UPS) | OrderCloud Team | IShippingRatesCalculator
| **Avalara** | [README](./OrderCloud.Integrations.Tax.Avalara) | [OrderCloud.Integrations.Tax.Avalara](https://www.nuget.org/packages/OrderCloud.Integrations.Tax.Avalara) | OrderCloud Team | ITaxCalculator, ITaxCodeProvider
| **Vertex** | [README](./OrderCloud.Integrations.Tax.Vertex) | [OrderCloud.Integrations.Tax.Vertex](https://www.nuget.org/packages/OrderCloud.Integrations.Tax.Vertex) | OrderCloud Team | ITaxCalculator
| **TaxJar** | [README](./OrderCloud.Integrations.Tax.TaxJar) | [OrderCloud.Integrations.Tax.TaxJar](https://www.nuget.org/packages/OrderCloud.Integrations.Tax.TaxJar) | OrderCloud Team | ITaxCalculator, ITaxCodeProvider

### [User Authentication](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/OrderCloud.Catalyst/Auth/UserAuth)

Use Ordercloud's authentication scheme in your own APIs.

```c#
[HttpGet("hello"), OrderCloudUserAuth(ApiRole.Shopper)]
public string SayHello() {
    return $"Hello {UserContext.Username}";  // UserContext is a property on CatalystController
}
```

### [Webhook Authentication](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/OrderCloud.Catalyst/Auth/WebhookAuth)

Securely receive push notifications of events from the Ordercloud platform.

```c#
[HttpPost("webhook"), OrderCloudWebhookAuth]
public object HandleAddressSave([FromBody] WebhookPayloads.Addresses.Save<MyConfigData> payload) {
    ...
}
```

### [Listing All Pages](./OrderCloud.Catalyst/DataMovement/ListAllAsync)

If OrderCloud's limit of 100 records per page is a pain point.

```c#
var orders = new OrderCloudClient(...).Orders.ListAllAsync();
```

### [Proxying Platform List Calls](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/OrderCloud.Catalyst/Models/ListOptions)

Receive list requests to your API with user defined filters, search, paging, and sorting.
```c#
[HttpGet("orders"), OrderCloudUserAuth(ApiRole.Shopper)]
public async Task<ListPage<Order>> ListOrders(IListArgs args)
{
    var user = await _oc.Me.GetAsync(UserContext.AccessToken); // get user details
    args.Filters.Add(new ListFilter("FromCompanyID", user.MeUser.Buyer.ID)) // filter using the user's buyer organization ID 
    args.Filters.Add(new ListFilter("LineItemCount", ">5"))
    // list orders from an admin endpoint
    var orders = await _oc.Orders.ListAsync(OrderDirection.Incoming, null, null, null, null, args); // apply list args with an extension version of ListAsync()
    return orders;
}
```

### [Caching](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/OrderCloud.Catalyst/DataMovement/Caching)

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

### [Throttler](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/OrderCloud.Catalyst/DataMovement/Throttler) 

A perfomance helper for multiple async function calls.

```c# 
var cars = new List<Car>();

var maxConcurency = 20;
var minPause = 100 // ms
var carOwners = await Throttler.RunAsync(cars, minPause, maxConcurency, car => apiClient.GetCarOwner(car.ID);
```

### [Error Handling](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/OrderCloud.Catalyst/Errors)

Handle API errors, including unexpected ones, with a standard JSON response structure. Define your own errors.

```c#
public class AgeLimit21Exception : CatalystBaseException
{
    public AgeLimit21Exception() : base("AgeLimit21", 403, "You must be 21 years of age or older to buy this product.") { }
}

....

Require.That(user.xp.Age >= 21, new AgeLimit21Exception());
```

### [Model Validation](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst/tree/dev/tests/OrderCloud.Catalyst.TestApi/Controllers/ModelValidation)

Take advantage of DataAnnotation attributes to specify validation requirements for your own custom models.

```c#
[Required(ErrorMessage = "This field is required, please try again.")]
public string RequiredField { get; set; }
```

### Testing helpers

When writing integration tests that hit an endpoint marked with `[OrderCloudUserAuth]`, you'll need to pass a properly formatted JWT token in the Authorization header, otherwise the call will fail. Fake tokens are a bit tedious to create, so `OrderCloud.Catalyst` provides a helper: 

```c#
var token = FakeOrderCloudToken.Create(
    clientID: "my-client-id", 
    roles: new List<string> { "Shopper" },
    expiresUTC: DateTime.UtcNow + TimeSpan.FromHours(1),
    notValidBeforeUTC: DateTime.UtcNow - TimeSpan.FromHours(1)
);
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);
```
