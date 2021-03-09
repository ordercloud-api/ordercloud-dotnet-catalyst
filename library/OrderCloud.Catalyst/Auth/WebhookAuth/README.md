## OrderCloud Webhook Authentication

One of the most common ways to integrate with OrderCloud.io is via webhooks, where your custom endpoints are called directly by OrderCloud, rather than a user app, when some event occurs within the platform. When you configure a webhook, you provide a secret key that is used by OrderCloud to create a hash of the request body and send it in the `X-oc-hash` header. Your custom endpoint can then check this hash to ensure the authenticity of the call. Here are the steps involved:

#### 1. Register OrderCloud webhook authentication in your [`Startup`](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup) class.

You must include your secret key here.

```c#
public virtual void ConfigureServices(IServiceCollection services) {
    service.AddOrderCloudWebhookAuth(opts => opts.HashKey = "my-secret-key");
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
## Best Practices
For security reasons it is important to keep your webhook hash key confidential and never store the raw value in your codebase or anywehre else where developers outside your organization may access it. Make sure you store this key somewhere secure and pass it into your application as a constant. A good solution for this is to store your hash key with an Azure App Configuration.

Webhook payload types (such as `WebhookPayloads.Addresses.Save` above) are defined in the [OrderCloud.io .NET SDK](https://github.com/ordercloud-api/ordercloud-dotnet-sdk).
