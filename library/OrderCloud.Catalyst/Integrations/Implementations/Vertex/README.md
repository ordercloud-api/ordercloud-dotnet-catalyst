# Vertex Integration 

## Scope of this integration
This .NET integration calculates sales tax for an Order using the vertex cloud API. It can be used during checkout to provide a tax cost to the buyer or after submit to create a vertex transaction for filling. 

## Vertex Basics 
[Vertex](https://www.vertexinc.com/) is a cloud or on premise **sales and use tax solution**. Vertex Cloud integrates with leading e-commerce platforms and mid-market ERP systems. Customers can use Vertex Cloud to manage complex sales and use tax across multiple jurisdictions. Vertex Cloud provides tax calculations and signature-ready PDF returns in one comprehensive solution.

## Sales Tax Estimate
The sales tax cost on an Order is first calculated in checkout after shipping selections are made and before payment. Following that, they are updated whenever the order is changed. 

**Vertex Side -** Get a tax estimate in Vertex Cloud by calling the [Tax Calculate for Sellers API endpoint](https://developer.vertexcloud.com/api/docs/#operation/Sale_Post) with `saleMessageType` set to `QUOTATION`.

**OrderCloud Side -** This integration should be triggered by the **`OrderCalculate`** Checkout Integration Event. Learn more about [checkout integration events](https://ordercloud.io/knowledge-base/order-checkout-integration); 

## Order Forwarding
A taxable transaction is committed to vertex asynchronously shortly following order submit. This enables businesses to easily file sales tax returns. OrderCloud guarantees the submitted order details provided will be unchanged since the most recent tax estimate displayed to the user.

**Vertex Side -** Commit a transaction in Vertex Cloud by calling the [Tax Calculate for Sellers API endpoint](https://developer.vertexcloud.com/api/docs/#operation/Sale_Post) with `saleMessageType` set to `INVOICE`.

**OrderCloud Side -** This integration should be triggered by the **`PostOrderSubmit`** Checkout Integration Event. Learn more about [checkout integration events](https://ordercloud.io/knowledge-base/order-checkout-integration); 

## Set up steps

- You should set up a .NET middleware project using the Catalyst library and starter project. [See guide](https://ordercloud.io/knowledge-base/start-dotnet-middleware-from-scratch).
- Using the OrderCloud API Portal, configure an Order Chekout IntegrationEvent object to point to your new middleware. [See guide](https://ordercloud.io/knowledge-base/order-checkout-integration)
- Create a vertex account online and retrieve all the configuration variables required in [VertexOCIntegrationConfig.cs](./VertexOCIntegrationConfig.cs); 
	- CompanyName
	- ClientID
	- ClientSecret
	- Username
	- Password
- Within your .NET code project, create an instance of [VertexOCIntegrationCommand.cs](./VertexOCIntegrationCommand.cs). Use the method `CalculateEstimateAsync` within the **`OrderCalculate`** Checkout Integration Event.  Use the method `CommitTransactionAsync` within the **`PostOrderSubmit`** Checkout Integration Event. 

## Interfaces

- It conforms to the [ITaxCalculator](../../Interfaces/ITaxCalculator.cs) interface.
