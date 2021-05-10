## OrderCloud User Authentication

When a user authenticates and acquires an access token from OrderCloud.io, typically in a front-end web or mobile app, that token can be used in your custom endpoints to verify the user's identity and roles. Here are the steps involved:

#### 1. Register OrderCloud user authentication and register user context in your [`Startup`](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup) class.

```c#
// In Startup.cs
public virtual void ConfigureServices(IServiceCollection services) {
    services.AddOrderCloudUserAuth();
}
```
#### 2. In your front-end app, anywhere you call one of your custom endpoints, pass the OrderCloud.io access token in a request header.

```
Authorization: Bearer my-ordercloud-token
```

#### 3. Mark any of your controllers or action  methods with `[OrderCloudUserAuth]`. Use the CatalystBaseController property `UserContext`.

Optionally, You may provide one or more required roles in this attribute, **any one of which** the user must be assigned in order for authorization to succeed.

```c#
public class MyThingController : CatalystController
{
    // Without access, requestor recieves a 401 Unauthorized or 403 InsufficientRoles error.
    [HttpGet, Route("thing")] 
    [OrderCloudUserAuth(ApiRole.Shopper, ApiRole.OrderReader, ApiRole.OrderAdmin)] // Any one of these threee roles gives access the endpoint 
    public Thing Get(string id) {
        var username = UserContext.Username; // UserContext is a property on CatalystController
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
        return $"Hello {UserContext.Username}, your role is {UserContext.CommerceRole}";.
    }
```

Get the full user details such as FirstName, LastName and xp from the GET /me endpoint
```c#
    [HttpPut, Route("hello")]
    [OrderCloudUserAuth] // No roles are defined, so any valid Ordercloud Token gives access.
    public async Task<string> Hello([FromBody] Thing thing) {
        var user = await _oc.Me.GetAsync(UserContext.AccessToken)

        return $"Hello {user.FirstName} {user.LastName}"; // now no error thrown
    }
```

Proxy the Ordercloud API, adding your own permission logic
```c#
    [HttpGet, Route("orders/mystore")]
    [OrderCloudUserAuth(ApiRole.Shopper)] 
    public async Task<ListPage<Order>> ListOrdersFromMyStore(ListArgs args) {
        // A different way to get the user details on MeUser. Make any request from OcClient as the authenticated user.
        var me = await await UserContext.BuildClient().Me.GetAsync();
        // Access a developer-defined extended property (xp) on the user called "StoreID".
        var storeID = me.xp.StoreID 
        // Create a filter. Only return orders where Order.BillingAddress.ID equals the user's storeID.   
        var billingAddressFilter = new ListFilter("BillingAddress.ID", storeID);
        // Add the filter on top of any additional api user-defined filters. 
        args.Filters.Add(billingAddressFilter);
        // request orders from an admin perspective
        return new OrderCloudClient(...).Orders.ListAsync(OrderDirection.Outgoing, page: args.Page, pageSize: args.PageSize, filters: args.ToFilterString()) 
    }
}
```

### DecodedToken and RequestAuthenticationService
Outside a request to a Controller you can use the injectable `RequestAuthenticationService` to parse and verify a user's token. 
```c#
    string rawToken = "...";
    // Parses the token, but does not verify it. 
    DecodedToken context = new DecodedToken(rawToken);
    // Only data on the token is available. user.FirstName and user.xp are not, for example.
    console.log(user.Username)

    // Inject a RequestAuthenticationService to verify. [OrderCloudUserAuth] uses this method under the hood. 
    DecodedToken verified = await _requestAuthenticationService.VerifyTokenAsync(rawToken); 

    // RequestAuthenticationService can also get a DecodedToken from the current HttpContext.
    // Only use this after calling VerifyTokenAsync, either directly or through [OrderCloudUserAuth].
    DecodedToken unverified = await _requestAuthenticationService.GetDecodedToken(); 

    // A shortcut method for getting the full user details
    MeUser user = await _requestAuthenticationService.GetUserAsync(); 

```

Inject the RequestAuthenticationService into a command class. Within a method, an OrderCloud request can be made using that user's token. 

```c#
public class OrderSubmitCommand 
{
    private readonly IOrderCloudClient _oc;      // Injected with Integration Client ID context. FullAccess "super user".
    private readonly RequestAuthenticationService _auth; // User token that made the request 
    private readonly ICreditCardCommand _card;   // Details of card processing left unopinionated
    
    public OrderSubmitCommand(IOrderCloudClient oc, RequestAuthenticationService auth, ICreditCardCommand card)
    {
        _oc = oc;
        _auth = auth;
        _card = card;
    }

    public async Task<Order> SubmitOrderAsync(string orderID, OrderDirection direction, OrderCloudIntegrationsCreditCardPayment payment) 
    {
        // Order details, inlcuding all line items. Requested with "super user" client context.
        var worksheet = await _oc.IntegrationEvents.GetWorksheetAsync(OrderDirection.Incoming, orderID);
        // Perform validation with full "super user" data
        await ValidateOrderAsync(worksheet, payment);
        await _card.AuthorizePayment(payment);  // do this in middleware for security.
        try
        {
            // Make the submit order request with the original user's token.
            return await _auth.GetClient().Orders.SubmitAsync(direction, orderID); 
        }
        catch (Exception)
        {
            await _card.VoidPaymentAsync(orderID);
            throw;
        }
    }
}
```     

    

