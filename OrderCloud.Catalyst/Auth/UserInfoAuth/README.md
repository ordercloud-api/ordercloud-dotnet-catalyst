## OrderCloud UserInfo Authentication

When a user authenticates and acquires a userinfo token from OrderCloud, typically in a front-end web or mobile app, that token can be used in your custom endpoints to verify the user's identity and roles. Here are the steps involved:

#### 1. Register OrderCloud user authentication and register user context in your [`Startup`](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup) class.

```c#
// In Startup.cs
public virtual void ConfigureServices(IServiceCollection services) {
    services.AddOrderCloudUserInfoAuth();
}
```
#### 2. In your front-end app, anywhere you call one of your custom endpoints, pass the OrderCloud userinfo token in a request header.

```
Authorization: Bearer my-ordercloud-userinfo-token
```

#### 3. Mark any of your controllers or action  methods with `[OrderCloudUserInfoAuth]`. Use the CatalystBaseController property `UserInfoContext`.

Optionally, You may provide one or more required roles in this attribute, **any one of which** the user must be assigned in order for authorization to succeed.

```c#
public class MyThingController : CatalystController
{
    // Without access, requestor recieves a 401 Unauthorized or 403 InsufficientRoles error.
    [HttpGet, Route("thing")] 
    [OrderCloudUserInfoAuth(ApiRole.Shopper, ApiRole.OrderReader, ApiRole.OrderAdmin)] // Any one of these threee roles gives access the endpoint 
    public Thing Get(string id) {
        var username = UserInfoContext.Username; // UserInfoContext is a property on CatalystController
        ...
    }
}
```

Define custom roles that are meaningful in your app's context.
Give users access to these roles through https://ordercloud.io/api-reference/authentication-and-authorization/security-profiles/create
```c#
    [HttpPut, Route("thing")]
    [OrderCloudUserInfoAuth("ThingAdmin")] // The role "ThingAdmin" is a custom developer-defined role
    public void Edit([FromBody] Thing thing) {
        ...
    }
```

Access data in the claims of the OrderCloud token used in the request.
```c#
    [HttpPut, Route("hello")]
    [OrderCloudUserInfoAuth] // No roles are defined, so any valid OrderCloud Token gives access.
    public string Hello([FromBody] Thing thing) {
        return $"Hello {UserInfoContext.Username}, you belong to groups {string.Join(",", UserInfoContext.Groups)}";.
    }
```

### DecodedUserInfoToken and IRequestAuthenticationService

The primary way to enforce authentication in controllers is by using the [OrderCloudUserInfoAuth] attribute on your controller actions. This automatically validates tokens and injects user info context for you.

However, if you prefer to handle authentication inside a service (rather than directly in the controller), IRequestAuthenticationService offers an alternative approach. It allows you to verify tokens and retrieve user details programmatically.

> **Important:**
IRequestAuthenticationService depends on HttpContext and should only be used within the ASP.NET Core pipeline (e.g., controllers, middleware, or services called from them).
For environments without HttpContext (such as Azure Functions, background services, or console apps), use ITokenValidator instead. ITokenValidator works independently of the current request


#### Using IRequestAuthenticationService (ASP.NET Core)
```c#
    string rawToken = "...";
    // Parses the token, but does not verify it. 
    DecodedUserInfoToken context = new DecodedUserInfoToken(rawToken);
    // Only data on the token is available. user.FirstName and user.xp are not, for example.
    console.log(user.Username)

    // Inject a IRequestAuthenticationService to verify. [OrderCloudUserInfoAuth] uses this method under the hood.
    DecodedUserInfoToken verified = await _requestAuthenticationService.VerifyUserInfoTokenAsync(rawToken); 

    // IRequestAuthenticationService can also get a DecodedUserInfoToken from the current HttpContext.
    // Only use this after calling VerifyUserInfoTokenAsync, either directly or through [OrderCloudUserInfoAuth].
    DecodedUserInfoToken unverified = await _requestAuthenticationService.GetDecodedUserInfoToken(); 

```
   

### Using ITokenValidator (Non-ASP.NET Contexts like Azure Functions)

ITokenValidator does not depend on HttpContext and works anywhere you have the raw token:


```c#
public class TokenValidationExample
{
    private readonly ITokenValidator _tokenValidator;

    public TokenValidationExample(ITokenValidator tokenValidator)
    {
        _tokenValidator = tokenValidator;
    }

    public async Task ValidateTokenAsync(string rawToken)
    {
        // Validate token and enforce roles/user types if needed
        DecodedUserInfoToken decoded = await _tokenValidator.ValidateUserInfoTokenAsync(rawToken);

        Console.WriteLine($"Token is valid for user: {decoded.Username}");
    }
}
```