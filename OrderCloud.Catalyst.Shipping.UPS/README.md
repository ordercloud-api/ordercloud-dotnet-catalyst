# OrderCloud.Catalyst.Shipping.UPS

This project brings shipping rate calculation to your ecommerce app using the [UPS RESTful API](https://www.ups.com/upsdeveloperkit?loc=en_US). It will be published as a nuget code library and conforms to a standard shipping interface published in the base library OrderCloud.Catalyst.

## Basics and Installation

1. If you haven't, please review [Order Checkout Integration Event](https://ordercloud.io/knowledge-base/order-checkout-integration) focusing on the ShippingRates event. In short, a webhook from the platform makes a request to a solution-custom API route that contains logic for estimating shipping rates. 
2. This library can be installed in the context of a .NET API project that responds to those webhooks. If you already have a .NET API project, great. If not, you can [follow this guide](https://ordercloud.io/knowledge-base/start-dotnet-middleware-from-scratch). After you have published your API, you will need to configure OrderCloud to point its Integration Event webhooks at your API. 
3. In your .NET project, add the OrderCloud.Catalyst.Shipping.UPS nuget package with either the Visual Studio UI or the dotnet CLI.

```dotnet add package OrderCloud.Catalyst.Shipping.UPS```

## Authentication and Injection

You will need these configuration data points to authneticate to the UPS API - *ApiKey*, and *BaseUrl*. Create a UPS account to get an ApiKey.

```c#
var upsCommand = new UPSCommand(new UPSConfig()
{
	BaseUrl = "https://onlinetools.ups.com/ship/v1",
	ApiKey = "...",
});
```

For efficient use of compute resources and clean code, create 1 UPSCommand object and make it available throughout your project using inversion of control dependency injection. 

```c#
services.AddSingleton<IShipMethodCalculator>(upsCommand);
```

Notice that the interface IShipMethodCalculator is not specific to UPS. It is general across providers and comes from the upstream OrderCloud.Catalyst package. 

## Usage 

Create routes that respond to the OrderCloud platform's Integration Event webhooks. Inject the shipping interface IShipMethodCalculator and use it within the logic of the route. It is not recommended to rely directly on UPSCommand anywhere. The layer of abstraction that IShipMethodCalculator provides decouples your code from UPS as a specific provider and hides some internal complexity.

```c#
public class CheckoutIntegrationEventController : CatalystController
{
	private readonly IShipMethodCalculator _shipMethodCalculator;

	public CheckoutIntegrationEventController(IShipMethodCalculator shipMethodCalculator)
	{
		// Inject interface. Implementation will depend on how services were registered, UPSCommand in this case.
		_shipMethodCalculator = shipMethodCalculator; 
	}

	....

	[HttpPost, Route("shippingrates")] // route and method specified by OrderCloud platform
	[OrderCloudWebhookAuth] // Security feature to verifiy request came from Ordercloud.
	public async Task<OrderCalculateResponse> EstimateShippingRates([FromBody] OrderCalculatePayload<CheckoutConfig> payload)
	{
		var response = new ShipEstimateResponse();

		// containerization logic - how should lineItem quantities be boxed into a set of shipped packages?

		response.ShipEstimates = new List<ShipEstimate> { ... }

		// Each ShipEstimate should be convertable into a ShipPackage, which has all the data shippers need to provide a rate.
		// Your mapping here

		var packages = new List<ShipPackages>() { ... }
		List<List<ShipMethod> rates = await _shipMethodCalculator.CalculateShipMethodsAsync(packages);
		for (var i = 0; i<response.ShipEstimates.Count; i++) 
		{
			response.ShipEstimates[0].ShipMethods = rates[0]
		}

		...
	}

}
```

This library also supports more complex cases that require mulitple shipping accounts with different credentials. For example, in a franchise business model where each location is independent but all sell on one ecommerce solution. In that case, still inject one instance of UPSCommand exactly as above. You can provide empty strings for the fields. However, when you call methods on the interfaces, provide the optional `configOverride` parameter. 

```c#
UPSConfig configOverride = await FetchShippingAccountCredentials(supplierID);
var packages = new List<ShipPackage>() { ... }
List<List<ShipMethods> rates = await _shipMethodCalculator.CalculateShipMethodsAsync(packages, configOverride);
```
