# Vertex Integration 

## Scope of this integration
This .NET integration calculates sales tax for an Order using the TaxJar API. It can be used during checkout to provide a tax cost to the buyer or after submit to create a vertex transaction for filling. 

Use Cases:
- Sales Tax Estimate
- Finialized Order Forwarding 

## Taxjar Basics 
[TaxJar](https://www.taxjar.com/) is reimagining how businesses manage sales tax compliance. Our cloud-based platform automates the entire sales tax life cycle across all of your sales channels — from calculations and nexus tracking to reporting and filing. With innovative technology and award-winning support, we simplify sales tax compliance so you can grow with ease.

## Sales Tax Estimate
The sales tax cost on an Order is first calculated in checkout after shipping selections are made and before payment. Following that, they are updated whenever the order is changed. 

**Avalara Side -** Get a tax estimate by calling Avalara's [create transaction endpoint](https://developer.avalara.com/api-reference/avatax/rest/v2/methods/Transactions/CreateTransaction) with `type` set to `SalesOrder`.

**OrderCloud Side -** This integration should be triggered by the **`OrderCalculate`** Checkout Integration Event. Learn more about [checkout integration events](https://ordercloud.io/knowledge-base/order-checkout-integration); 

## Committed Transactions
A taxable transaction is committed to avalara asynchronously shortly following order submit. This enables businesses to easily file sales tax returns. OrderCloud guarantees the submitted order details provided will be unchanged since the most recent tax estimate displayed to the user.

**Avalara Side -** Commit a transaction in Avalara by calling the  [create transaction endpoint](https://developer.avalara.com/api-reference/avatax/rest/v2/methods/Transactions/CreateTransaction) with `type` set to `SalesInvoice`.

**OrderCloud Side -** This integration should be triggered by the **`PostOrderSubmit`** Checkout Integration Event. Learn more about [checkout integration events](https://ordercloud.io/knowledge-base/order-checkout-integration); 

## Set up steps

- You should set up a .NET middleware project using the Catalyst library and starter project. [See guide](https://ordercloud.io/knowledge-base/start-dotnet-middleware-from-scratch).
- Using the OrderCloud API Portal, configure an Order Chekout IntegrationEvent object to point to your new middleware. [See guide](https://ordercloud.io/knowledge-base/order-checkout-integration)
- Create an avalara account online and retrieve all the configuration variables required in [AvalaraConfig.cs](./Avalara.cs); 
	- BaseUrl (https://sandbox-rest.avatax.com/api/v2","https://rest.avatax.com/api/v2")
	- AccountID 
	- LicenseKey
	- CompanyCode
- Within your .NET code project, create an instance of [AvalaraCommand.cs](./AvalaraCommand.cs). Use the method `CalculateEstimateAsync` within the **`OrderCalculate`** Checkout Integration Event.  Use the method `CommitTransactionAsync` within the **`PostOrderSubmit`** Checkout Integration Event. 

## Interfaces

- It conforms to the [ITaxCalculator](../OrderCloud.Catalyst/Integrations/Interfaces/ITaxCalculator.cs) interface.
