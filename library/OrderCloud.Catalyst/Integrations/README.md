# Contributing Guide For New Integrations 

Guidelines for adding a new integration to the Catalyst library. 

## Basics 

Creating integrations as part of this .NET Catalyst project means that they will be delivered as a code library through a Nuget package. Each integration should expose functionality to interact with 1 external service and should not depend any other integrations. There is a tension between providing too little functionality (creating a generic API client) and too much (limiting future flexibility). The key to this balance is the methods of the contract your integration exposes.

## Exposed Contracts 

All integrations should include two classes designed to be exposed and consumed by solutions - an `OCIntegrationConfig` and an `OCIntegrationCommand`. The config is a POCO which contains properties for all the environment variables and secrets needed to authenticate to the service. The command exposes the functionality of your integration in methods. So, for an example service called "Mississippi" you would create the classes below. 

```c#
public class MississippiOCIntegrationConfig : OCIntegrationConfig
{
	public string ApiKey { get; set;}
	... ect
}

public class MississippiOCIntegrationCommand : OCIntegrationCommand, IRiver
{
	protected MississippiOCIntegrationConfig _config;

	public MississippiOCIntegrationCommand(MississippiOCIntegrationConfig config) 
	{
		_config = config; // used to auth to service
	}

	public async Task GetRiverLength() 
	{

	}

	... ect
}
```

Your integration can contain other classes, even public ones, but these two manditory classes form the exposed surface of your integration - designed for use in other projects. 

## Interfaces 

A key goal of these integrations is interopability. In other words, if two services solve roughly the same problem (e.g. calculating tax), they should expose the same contract. To facilitate that, there are interfaces like ITaxCalculator. Please check the existing interfaces and see if any apply to your integration's problem domain. If they do, make sure your OCIntegrationCommand implements those interfaces. Feel free open issues recommending changes to the interfaces. 

## Other Guidelines

 - Keep the number of properties and methods on your exposed contracts to the minimum required. Do a small amount well. 
 - Handle error scenarios within your integration by throwing a CatalystBaseException.
 - Avoid adding a nuget package for your service's SDK. This will lead to bloat as many projects may use this library without using your service. Instead, use the Flurl library for RESTful requests. This will also keep testing consistient. 
 - Write unit tests against the Command methods. Mock API reponses from your service using Flurl (https://flurl.dev/docs/testable-http/) or something simuliar. 
 - Please make sure to include a README.md file with your integration. It should match the format of the existing integrations.
 - When you want to make methods or properties `private`, consider using `protected` instead so that client projects can extend your functionality. 
 - Generally follow the folder structure and code patterns of existing integrations. 