## OrderCloud User Authentication

When a user authenticates and acquires an access token from OrderCloud.io, typically in a front-end web or mobile app, that token can be used in your custom endpoints to verify the user's identity and roles. Here are the steps involved:

#### 1. Register OrderCloud user authentication and register user context in your [`Startup`](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup) class.

```c#
public virtual void ConfigureServices(IServiceCollection services) {
    services.AddOrderCloudUserAuth();
    services.AddScoped<VerifiedUserContext>(); // instances are scoped to the reqest
}
```
#### 2. In your front-end app, anywhere you call one of your custom endpoints, pass the OrderCloud.io access token in a request header.

```
Authorization: Bearer my-ordercloud-token
```

#### 3. Mark any of your controllers or action  methods with `[OrderCloudUserAuth]`. Inject the `VerifiedUserContext`.

Optionally, You may provide one or more required roles in this attribute, **any one of which** the user must be assigned in order for authorization to succeed.

```c#
public class MyThingController 
{
    private readonly VerifiedUserContext _user;

    // Inject user context, which is scoped to a single request. Fields will only be defined if [OrderCloudUserAuth] is defined on the route.
    public MyThingController(VerifiedUserContext user) 
    {
        _user = user;
    }

    // Without access, requestor recieves a 401 Unauthorized or 403 InsufficientRoles error.
    [HttpGet, Route("thing")] 
    [OrderCloudUserAuth(ApiRole.Shopper, ApiRole.OrderReader, ApiRole.OrderAdmin)] // Any one of these threee roles gives access the endpoint 
    public Thing Get(string id) {
        var username = _user.Username;
        ...
    }
}
```

Define custom roles that are meaningful in your app's context.
Give users access to these roles through https://ordercloud.io/api-reference/authentication-and-authorization/security-profiles/create
```c#
    [HttpPut, Route("thing")]
    [OrderCloudUserAuth("ThingAdmin")] // The role "ThingAdmin" is a custom developer-defined role
    public void Edit([FromBody] Thing thing) {
        ...
    }
```

Access data in the claims of the OrderCloud token used in the request.
```c#
    [HttpPut, Route("hello")]
    [OrderCloudUserAuth] // No roles are defined, so any valid Ordercloud Token gives access.
    public string Hello([FromBody] Thing thing) {
        return $"Hello {_user.Username}, your role is {_user.CommerceRole}";.
    }
```

Get the full user details such as FirstName, LastName and xp from the GET /me endpoint
```c#
    [HttpPut, Route("hello")]
    [OrderCloudUserAuth] // No roles are defined, so any valid Ordercloud Token gives access.
    public async Task<string> Hello([FromBody] Thing thing) {
        var first = _user.MeUser.FirstName; // throws error    

        await _user.RequestMeUserAsync(); // Sets _user.MeUser

        return $"Hello {_user.MeUser.FirstName} {_user.MeUser.LastName}"; // now no error thrown
    }
```

In a C# context that is not a request to a Controller, for example a serverless function, set the user context via token.
```c#
    string token = "...";
    await _user.VerifyAsync(token); // will throw same errors if the there is any problem with the token
```

Proxy the Ordercloud API, adding your own permission logic
```c#
    [HttpGet, Route("orders/mystore")]
    [OrderCloudUserAuth(ApiRole.Shopper)] 
    public async Task<ListPage<Order>> ListOrdersFromMyStore(ListArgs args) {
        // A different way to get the user details on MeUser. Make any request from OcClient as the authenticated user.
        var me = await this._userContext.OcClient.Me.Get();
        // Access a developer-defined extended property (xp) on the user called "StoreID".
        var storeID = me.xp.StoreID 
        // Create a filter. Only return orders where Order.BillingAddress.ID equals the user's storeID.   
        var billingAddressFilter = new ListFilter("BillingAddress.ID", storeID);
        // Add the filter on top of any additional api user-defined filters. 
        args.Filters.Add(billingAddressFilter);
        // request orders from an admin endpoint
        return new OrderCloudClient(...).Orders.ListAsync(OrderDirection.Outgoing, page: args.Page, pageSize: args.PageSize, filters: args.ToFilterString()) 
    }
}
```

Inject the User Context into a command class. Within a method, an OrderCloud request can be made using that user's token. 

```c#
public class OrderSubmitCommand 
{
    private readonly IOrderCloudClient _oc;      // Injected with Integration Client ID context. FullAccess "super user".
    private readonly VerifiedUserContext _user; 
    private readonly ICreditCardCommand _card;   // Details of card processing left unopinionated
    
    public OrderSubmitCommand(IOrderCloudClient oc, VerifiedUserContext user, ICreditCardCommand card)
    {
        _oc = oc;
        _user = user;
        _card = card;
    }

    public async Task<Order> SubmitOrderAsync(string orderID, OrderDirection direction, OrderCloudIntegrationsCreditCardPayment payment) 
    {
        // Order details, inlcuding all line items. Requested with "super user" client context.
        var worksheet = await _oc.IntegrationEvents.GetWorksheetAsync<HSOrderWorksheet>(OrderDirection.Incoming, orderID);
        // Perform validation with full "super user" data
        await ValidateOrderAsync(worksheet, payment);
        await _card.AuthorizePayment(payment);  // do this in middleware for security.
        try
        {
            // Make the submit order request with the original user's token.
            return await _user.OcClient.Orders.SubmitAsync(direction, orderID); 
        }
        catch (Exception)
        {
            await _card.VoidPaymentAsync(orderID);
            throw;
        }
    }
}
```     

    

