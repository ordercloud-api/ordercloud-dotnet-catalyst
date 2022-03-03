# Contributing Guide For Integrations 

Thank you for investing your time in contributing! These are guidelines for adding a new C# integration to OrderCloud Catalyst. See a list of [existing integrations](https://github.com/ordercloud-api/ordercloud-dotnet-catalyst#3rd-party-integrations).

## Basics 

Creating an integration in this project means it will be published as its own Nuget code library. Each library should expose functionality to interact with 1 external service and should not depend on any other integrations. All integrations will depend on the [OrderCloud.Catalyst](https://www.nuget.org/packages/ordercloud-dotnet-catalyst/) base library to enforce standard contracts. There is a natural tension between providing too little "wrapper" functionality (creating a generic API client) and too much "wrapper" (an opinionated solution that limits use cases). The key to this balance are the details of the contract your integration exposes.

## Exposed Contracts 

All integrations should include two classes designed to be used by downstream projects - an `OCIntegrationConfig` and an `OCIntegrationCommand`. The config is a plain old c# object which contains properties for all the environment variables needed to authenticate to the service. The command exposes the functionality of your integration through methods. There should be two ways to provide a config to the command. First, a default config which is a constructor parameter. Second, every method should take an optional override config which only applies to that request's scope. For an example service called "Mississippi" you would create the classes below. 

```c#
public class MississippiConfig : OCIntegrationConfig
{
    public override string ServiceName { get; } = "Mississippi";

	[RequiredIntegrationField]
	public string ApiKey { get; set;}

	... more environment variables.
}
```
```c#
public class MississippiCommand : OCIntegrationCommand
{
	public MississippiOCIntegrationCommand(MississippiConfig configDefault) : base(configDefault) { }

	public async Task<decimal> GetRiverLength(OCIntegrationConfig configOverride = null) 
	{
		var configToUse = GetValidatedConfig<MississippiConfig>(configOverride);
		// Make request here 
	}

	... more methods.
}
```

Your integration will likely contain other public classes but these two mandatory classes form the exposed surface of your integration - designed for use in other projects. 

## Interfaces 

A key goal is *interoperability*. In other words, if two services solve roughly the same problem (e.g. calculating tax), they should expose the same contract. To facilitate that, there are interfaces like [ITaxCalculator](./Interfaces/ITaxCalculator.cs). Please check under [./Interfaces](./Interfaces) to see if any apply to your integration's problem domain. If some do, make sure your OCIntegrationCommand implements those interfaces.

Feel free open issues recommending changes or additions to the interfaces. 

## Guidelines

 - General
	- Keep the number of properties and methods on your exposed contracts to the minimum required. Do a small amount well. 
	- Aim to follow the code patterns of existing integrations. 
 - Project Structure
    - Create a new Visual Studio project under /libraries. It should be a .NET Standard 2.0 code library project called `OrderCloud.Catalyst.[Category].[ServiceName]`. For example, `OrderCloud.Catalyst.Tax.Avalara`.
	- Your new project should have a project dependency on OrderCloud.Catalyst. Also, OrderCloud.Catalyst.TestApi should depend on your new project. 
	- At the root of your new folder include your Command, Config and a README.md with instructions. Copy the README format of existing integrations.
	- All files and class names in the project should begin with your service name to avoid collisions.
	- Use the OrderCloud.Catalyst.[Category].[ServiceName] namespace for all classes.
 - Publishing on Nuget
	- Your .csproj file contains details of how your integration will be published. Make sure the target framework is `netstandard2.0` and the package Id is `OrderCloud.Catalyst.[Category].[ServiceName]`.
	- Versioning 
		- Refer to https://semver.org/. 
		- Start at version x.0.1 where x is the most recent Major version of OrderCloud.Catalyst. Increment the Patch version for bug fixes and Minor version for new functionality.  
		- Major version changes (API breaking changes) should only be made in response to major version changes in the core OrderCloud.Catalyst library. This should only happen when the interfaces in [/Integrations/Interfaces](./Interfaces) are changed. If the two libraries start with the same major version number, they should have matching interface definitions and be compatible. 
	- For now, the OrderCloud team will handle publishing packages in order to maintain high quality.
	- Versions marked `alpha` are early releases that are not ready for production. Use them only to provide feedback or get a sneak peak.
 - Errors 
	- Handle error scenarios within your integration by throwing one of the exceptions in [/Integrations/Exceptions](./Exceptions). 
	- Every integration should handle cases like missing configs, invalid authentication, error response, and no response.
 - Tests 
	- Write unit tests against your Command methods and put them in the OrderCloud.Catalyst.Tests project under a new folder like so `/IntegrationTests/[ServiceName]/[ServiceName]Tests.cs`. 
	- Mock API reponses from your service using [Flurl test practices](https://flurl.dev/docs/testable-http/) or something similar. 
	- Test error scenarios as well.
	- See [VertexTests](../../../tests.OrderCloud.Catalyst.Tests/IntegrationTests/Vertex/VertexTests.cs).
 - Code Style
    - For every public method on the Command class use the `GetValidatedConfig()` method to authenticate. It will default to the config provided in the constructor, but safely give priority to any config specified in the method call. This supports use cases like different suppliers using different credentials.
 	- Many of the existing integrations also have a Client class. For these integrations the Client class is a pure API wrapper, handling HTTP requests and exceptions. Avoid code patterns that lead to creating multiple Http Client objects in memory.  
	- Avoid adding a nuget package for your service's SDK (or nuget packages in general). Instead, use the Flurl library for RESTful requests. This will also keep testing consistient. 
    - When you want to make methods or properties `private`, consider using `protected` instead so that client projects can extend your functionality. 
	- Use `DateTime.Utc` explicitly to avoid the confusion of local timezones.
	- Always use the `decimal` type for anything money related.

## Approval

A disclaimer, whether a pull request with a new integration is accepted and published will ultimately depend on the approval of the OrderCloud team.

