## Webhook Authentication Attribute

## Purpose 
The Webhook authentication attribute ensures that the webhook request came from ordercloud. This prevents other applications from calling your webhook route directly.

## How it works
When you create a webhook in OrderCloud you are required to enter a "HashKey" (or "Secret"). 
Then OrderCloud will use this secret key to create a hash of the request body and send it in the X-oc-hash header. When you instantiate your Webhook Authentication in your application you will need to pass in this HashKey so it can be used as a comparison to what is received in the "X-oc-hash" header in webhook requests. If the values match then the request is authenticated and allowed to proceed. If they do not (or the header is not present) the request will be rejected.

## Example 
```c#
    //  Here we can configure webhook authentication and declare our webhook hash key
    public static IServiceCollection ConfigureServices<TSettings>(
        this IServiceCollection services, 
        OrderCloudWebhookAuthOptions webhookConfig = null
    )
        where TSettings : class, new()
    {
        services
            .AddAuthentication()
            .AddScheme<OrderCloudWebhookAuthOptions, OrderCloudWebhookAuthHandler>("OrderCloudWebhook", null, opts => opts.HashKey = webhookConfig.HashKey);

        return services;
    }

    //  Then in our webhook controller we can define a route and implement the OrderCloudWebhookAuth attribute.
    public class WebhookController : BaseController
	{
		[Route("webhook/saveaddress"), OrderCloudWebhookAuth]
		public object HandleAddressSave([FromBody] WebhookPayloads.Addresses.Save payload)
		{
			return new
			{
				Action = "HandleAddressSave",
				City = payload.Request.Body.City,
				Foo = payload.ConfigData.Foo
			};
		}
	}
```

## Best Practices
For security reasons it is important to keep your webhook hash key confidential and never store the raw value in your codebase or anywehre else where developers outside your organization may access it. Make sure you store this key somewhere secure and pass it into your application as a constant. A good solution for this is to store your hash key with an Azure App Configuration.