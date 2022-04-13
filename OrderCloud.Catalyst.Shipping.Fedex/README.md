# OrderCloud.Catalyst.Shipping.Fedex

This project brings shipping rate calculation to your ecommerce app using the [Fedex](https://developer.fedex.com/api/en-us/get-started.html) API. It will be published as a nuget code library and conforms to a standard shipping interface published in the base library OrderCloud.Catalyst.

## Basics and Installation

1. If you haven't, please review [Order Checkout Integration Event](https://ordercloud.io/knowledge-base/order-checkout-integration) focusing on the ShippingRates event. In short, a webhook from the platform makes a request to a solution-custom API route that contains logic for estimating shipping rates. 
2. This library can be installed in the context of a .NET API project that responds to those webhooks. If you already have a .NET API project, great. If not, you can [follow this guide](https://ordercloud.io/knowledge-base/start-dotnet-middleware-from-scratch). After you have published your API, you will need to configure OrderCloud to point its Integration Event webhooks at your API. 
3. In your .NET project, add the OrderCloud.Catalyst.Shipping.Fedex nuget package with either the Visual Studio UI or the dotnet CLI.

```dotnet add package OrderCloud.Catalyst.Shipping.Fedex```

## Authentication and Injection

You will need these configuration data points to authneticate to the Fedex API - *BaseUrl*, *ClientID*, *ClientSecret* and *AccountNumber*. Create an account with Fedex and get these from the admin portal.

```c#
var fedexCommand = new FedexCommand(new FedexConfig()
{
	BaseUrl = "https://apis-sandbox.fedex.com", // or https://apis.fedex.com
	ClientID = "...",
	ClientSecret = "...",
	AccountNumber =  "..."
});
```

For efficient use of compute resources and clean code, create 1 FedexCommand object and make it available throughout your project using inversion of control dependency injection. 

```c#
services.AddSingleton<IShipMethodCalculator>(fedexCommand);
```

Notice that IShipMethodCalculator is not specific to Fedex. It is general to shipping rates and comes from the upstream OrderCloud.Catalyst package. 


## Usage 

Create routes that respond to the OrderCloud platform's Integration Event webhooks. Inject the shipping interface IShipMethodCalculator and use it within the logic of the route. It is not recommended to rely directly on FedexCommand anywhere. The layer of abstraction that IShipMethodCalculator provides decouples your code from Fedex as a specific provider and hides some internal complexity.

```c#
public class CheckoutIntegrationEventController : CatalystController
{
	private readonly IShipMethodCalculator _shipMethodCalculator;

	public CheckoutIntegrationEventController(IShipMethodCalculator shipMethodCalculator)
	{
		// Inject interface. Implementation will depend on how services were registered, FedexCommand in this case.
		_shipMethodCalculator = shipMethodCalculator; 
	}

	....

	[HttpPost, Route("shippingrates")] // route and method specified by OrderCloud platform
	[OrderCloudWebhookAuth] // Security feature to verifiy request came from Ordercloud.
	public async Task<ShipEstimateResponse> EstimateShippingRates([FromBody] OrderCalculatePayload<CheckoutConfig> payload)
	{
		var response = new ShipEstimateResponse();

		// containerization logic - how should lineItem quantities be boxed into a set of shipped packages?
		response.ShipEstimates = new List<ShipEstimate> { ... }
		var packages = response.ShipEstimates.Select(se => MapToPackages(response.ShipEstimates));
		
		// use the interface
		List<List<ShipMethod> rates = await _shipMethodCalculator.CalculateShipMethodsAsync(packages);
		
		for (var i = 0; i<response.ShipEstimates.Count; i++) 
		{
			response.ShipEstimates[0].ShipMethods = rates[0]
		}

		return response;
	}

}
```

This library also supports more complex cases that require mulitple shipping accounts with different credentials. For example, in a franchise business model where each location is independent but all sell on one ecommerce solution. In that case, still inject one instance of FedexCommand exactly as above. You can provide empty strings for the fields. However, when you call methods on the interfaces, provide the optional `configOverride` parameter. 

```c#
FedexConfig configOverride = await FetchShippingAccountCredentials(supplierID)
var packages = new List<ShipPackage>() { ... }
List<List<ShipMethods> rates = await _shipMethodCalculator.CalculateShipMethodsAsync(packages, configOverride);
```
