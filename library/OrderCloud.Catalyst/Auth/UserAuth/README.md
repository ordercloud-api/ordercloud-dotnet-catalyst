## OrderCloud User Authentication

When a user authenticates and acquires an access token from OrderCloud.io, typically in a front-end web or mobile app, that token can be used in your custom endpoints to verify the user's identity and roles. Here are the steps involved:

#### 1. Register OrderCloud user authentication in your [`Startup`](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup) class.

```c#
public virtual void ConfigureServices(IServiceCollection services) {
    services.AddOrderCloudUserAuth();
}
```

##### 2. Mark any of your controllers or action  methods with `[OrderCloudUserAuth]`. Inject the `VerifiedUserContext`.

Optionally, You may provide one or more required roles in this attribute, **any one of which** the user must be assigned in order for authorization to succeed.

```c#
public class MyThingController : BaseController {}

    private readonly VerifiedUserContext _userContext;

    // Inject user context, which is scoped to a single request. Fields will only be defined if [OrderCloudUserAuth] is defined on the route.
    public MyThingController(VerifiedUserContext userContext) 
    {
        _userContext = userContext;
    }

    // Require one of mulitple roles to access an endpoint
    [HttpGet, Route("thing")] 
    [OrderCloudUserAuth(ApiRole.Shopper, ApiRole.OrderReader, ApiRole.OrderAdmin)] 
    public Thing Get(string id) {
        ...
    }

    // Define custom roles that are meaningful in your app's context.
    // Give user's access to these roles through https://ordercloud.io/api-reference/authentication-and-authorization/security-profiles/create
    [HttpPut, Route("thing")]
    [OrderCloudUserAuth("ThingAdmin")] // The role "ThingAdmin" is a custom developer-defined role
    public void Edit([FromBody] Thing thing) {
        ...
    }

    // Have access to the UserContext of the token making requests to your api.
    [HttpPut, Route("hello")]
    [OrderCloudUserAuth] // No roles are defined, so any valid Ordercloud Token gives access.
    public string Hello([FromBody] Thing thing) {
        return $"Hello {this._userContext.FirstName} {this._userContext.LastName}";  // UserContext defined on the BaseContoller class and determined by the token.
    }

    // Proxy the Ordercloud API, adding your own permission logic
    [HttpGet, Route("orders/mystore")]
    [OrderCloudUserAuth(ApiRole.Shopper)] 
    public ListPage<Order> ListOrdersFromMyStore(ListArgs args) {
        // Get the details of the current user from OrderCloud.
        var me = await this._userContext.OcClient.Me.Get();
        // Access a developer-defined extended property (xp) on the user called "StoreID".
        var storeID = me.xp.StoreID 
        // Create a filter. Only return orders where Order.BillingAddress.ID equals the user's storeID.   
        var billingAddressFilter = new ListFilter() { PropertyName = "BillingAddress.ID", FilterExpression = storeID };
        // Add the filter on top of any additional api user-defined filters. 
        args.Filters.Add(billingAddressFilter);
        // request orders from an admin endpoint
        return new OrderCloudClient(...).Orders.ListAsync(OrderDirection.Outgoing, page: args.Page, pageSize: args.PageSize, filters: args.ToFilterString()) 
    }
}
```

#### 3. In your front-end app, anywhere you call one of your custom endpoints, pass the OrderCloud.io access token in a request header.

```
Authorization: Bearer my-ordercloud-token
```

