# Contributing Guide For Integrations 

Thank you for investing your time in contributing! These are guidelines for adding a new integration to the Catalyst library. See a list of [existing integrations](./Implementations/README.md).

## Basics 

Creating an integration in this project means it will be published as part of a [Nuget code library](https://www.nuget.org/packages/ordercloud-dotnet-catalyst/). Each integration should expose functionality to interact with 1 external service and should not depend on any other integrations. There is a natural tension between providing too little "wrapper" functionality (creating a generic API client) and too much "wrapper" (an opinionated solution that limits use cases). The key to this balance are the details of the contract your integration exposes.

## Exposed Contracts 

All integrations should include two classes designed to be exposed and consumed by solutions - an `OCIntegrationConfig` and an `OCIntegrationCommand`. The config is a POCO which contains properties for all the environment variables and secrets needed to authenticate to the service. The command exposes the functionality of your integration through methods. For an example service called "Mississippi" you would create the classes below. 

```c#
public class MississippiConfig : OCIntegrationConfig
{
    public override string ServiceName { get; } = "Mississippi";

	[RequiredIntegrationField]
	public string ApiKey { get; set;}

	... etc.
}
```
```c#
public class MississippiCommand : OCIntegrationCommand
{
	protected readonly MississippiConfig _config;

	public MississippiOCIntegrationCommand(MississippiConfig config) : base(config)
	{
		_config = config; // used to auth to service
	}

	public async Task<decimal> GetRiverLength() 
	{

	}

	... etc.
}
```

Your integration will likely contain other public classes but these two mandatory classes form the exposed surface of your integration - designed for use in other projects. 

## Interfaces 

A key goal is *interoperability*. In other words, if two services solve roughly the same problem (e.g. calculating tax), they should expose the same contract. To facilitate that, there are interfaces like [ITaxCalculator](./Interfaces/ITaxCalculator.cs). Please check under [/Integrations/Interfaces](./Interfaces) to see if any apply to your integration's problem domain. If some do, make sure your OCIntegrationCommand implements those interfaces.

Feel free open issues recommending changes or additions to the interfaces. 

## Guidelines

 - General
	- Keep the number of properties and methods on your exposed contracts to the minimum required. Do a small amount well. 
	- Aim to follow the code patterns of existing integrations. 
 - Folders and files	
	- Under [/Integrations/Implementations](./Implementations) create a folder with your service name (e.g. "Mississippi") to contain your files. 
	- At the root of your new folder include your Command, Config and a README.md. Copy the README format of existing integrations.
	- All files and class names should begin with your service name.
	- Many of the existing integrations also have a Client class. For these integrations the Client class is a pure API wrapper, handling HTTP requests and exceptions. This leaves mapping as the responsibility of the Command class. 
 - Errors 
	- Handle error scenarios within your integration by throwing one of the exceptions in [/Integrations/Exceptions](./Exceptions). 
	- Every integration should handle cases like missing configs, invalid authentication, error response, and no response.
 - Tests 
	- Write unit tests against your Command methods and put them in the OrderCloud.Catalyst.Tests project under a new folder like so `/IntegrationTests/[ServiceName]/[ServiceName]Tests.cs`. 
	- Mock API reponses from your service using [Flurl test practices](https://flurl.dev/docs/testable-http/) or something similar. 
	- Test error scenarios as well.
	- See [VertexTests](../../../tests.OrderCloud.Catalyst.Tests/IntegrationTests/Vertex/VertexTests.cs).
 - Code Style
	- Avoid adding a nuget package for your service's SDK. This will lead to bloat as many projects may use this library without using your service. Instead, use the Flurl library for RESTful requests. This will also keep testing consistient. 
    - When you want to make methods or properties `private`, consider using `protected` instead so that client projects can extend your functionality. 
	- Use DateTime.Utc explicitly to keep the project time-zone agnostic.
	- Always use the `decimal` type for anything money related.

## Approval

A disclaimer, whether a pull request with a new integration is accepted and published will ultimately depend on the approval of the OrderCloud team.

