# Contributing Guide For Integrations 

Guidelines for adding a new integration to the Catalyst library. 

## Basics 

Creating an integration in this project means it will be published as part of a [Nuget code library](https://www.nuget.org/packages/ordercloud-dotnet-catalyst/). Each integration should expose functionality to interact with 1 external service and should not depend any other integrations. There is a natural tension between providing too little "wrapper" functionality (creating a generic API client) and too much "wrapper" (an opinionated solution that limits use cases). The key to this balance is the method signatures of the contract your integration exposes.

## Exposed Contracts 

All integrations should include two classes designed to be exposed and consumed by solutions - an `OCIntegrationConfig` and an `OCIntegrationCommand`. The config is a POCO which contains properties for all the environment variables and secrets needed to authenticate to the service. The command exposes the functionality of your integration through methods. For an example service called "Mississippi" you would create the classes below. 

```c#
public class MississippiOCIntegrationConfig : OCIntegrationConfig
{
	public string ApiKey { get; set;}
	... ect.
}
```
```c#
public class MississippiOCIntegrationCommand : OCIntegrationCommand, IRiver
{
	protected readonly MississippiOCIntegrationConfig _config;

	public MississippiOCIntegrationCommand(MississippiOCIntegrationConfig config) 
	{
		_config = config; // used to auth to service
	}

	public async Task<decimal> GetRiverLength() 
	{

	}

	... ect.
}
```

Your integration will likely contain other public classes but these two mandatory classes form the exposed surface of your integration - designed for use in other projects. 

## Interfaces 

A key goal of these integrations is *interoperability*. In other words, if two services solve roughly the same problem (e.g. calculating tax), they should expose the same contract. To facilitate that, there are interfaces like [ITaxCalculator](./Interfaces/ITaxCalculator.cs). Please check under [/Integrations/Interfaces](./Interfaces) to see if any apply to your integration's problem domain. If some do, make sure your OCIntegrationCommand implements those interfaces. If none do, still create your integration but be aware we may look to standardize it in the future. Feel free open issues recommending changes or additions to the interfaces. 

## Guidelines

 - Keep the number of properties and methods on your exposed contracts to the minimum required. Do a small amount well. 
 - Under [/Integrations/Implementations](./Implementations) create a folder with your service name (e.g. "Mississippi") to contain your files. At the root of your new folder include your Command, Config and a README.md. Copy the README format of existing integrations.
 - Handle error scenarios like auth errors and bad requests within your integration by throwing a CatalystBaseException.
 - Avoid adding a nuget package for your service's SDK. This will lead to bloat as many projects may use this library without using your service. Instead, use the Flurl library for RESTful requests. This will also keep testing consistient. 
 - Write unit tests against your Command methods and put them in the OrderCloud.Catalyst.Tests project under `/IntegrationTests/[ServiceName]Tests.cs`. Mock API reponses from your service using [Flurl test practices](https://flurl.dev/docs/testable-http/) or something similar. 
 - When you want to make methods or properties `private`, consider using `protected` instead so that client projects can extend your functionality. 
 - Aim to follow the code patterns of existing integrations. 

## Approval

A disclaimer, whether a pull request with a new integration is accepted and published will ultimately depend on the approval of the OrderCloud team.
