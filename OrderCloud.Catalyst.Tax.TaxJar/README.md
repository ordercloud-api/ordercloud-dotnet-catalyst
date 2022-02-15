# TaxJar Integration 

## Scope of this integration
This .NET integration calculates sales tax for an Order using the TaxJar API. It can be used during checkout to provide a tax cost to the buyer or after submit to create a TarJar transaction for filling. 

Use Cases:
- Sales Tax Estimate
- Finialized Order Forwarding 

## Taxjar Basics 
[TaxJar](https://www.taxjar.com/) is reimagining how businesses manage sales tax compliance. Our cloud-based platform automates the entire sales tax life cycle across all of your sales channels — from calculations and nexus tracking to reporting and filing. With innovative technology and award-winning support, we simplify sales tax compliance so you can grow with ease.

## Sales Tax Estimate
The sales tax cost on an Order is first calculated in checkout after shipping selections are made and before payment. Following that, they are updated whenever the order is changed. 

**TaxJar Side -** Multiple requests are made to Taxjar's [calculate tax endpoint](https://developers.taxjar.com/api/reference/#post-calculate-sales-tax-for-an-order). Multiple because Taxjar's order model is limited to 1 shipping address and an OrderCloud order can contain LineItems shipping to different addresses. A TaxJar order request is made for each OrderCloud LineItem and each ShipEstimate.

**OrderCloud Side -** This integration should be triggered by the **`OrderCalculate`** Checkout Integration Event. Learn more about [checkout integration events](https://ordercloud.io/knowledge-base/order-checkout-integration); 

## Order Forwarding 
A taxable transaction is committed to taxjar asynchronously shortly following order submit. This enables businesses to easily file sales tax returns. OrderCloud guarantees the submitted order details provided will be unchanged since the most recent tax estimate displayed to the user.

**TaxJar Side -** Multiple requests are made to Taxjar's [create order endpoint](https://developers.taxjar.com/api/reference/#post-create-an-order-transaction). The TaxJar transactionId will look like `OrderID:|{orderID}|LineItemID:|{lineItemID}` or `OrderID:|{orderID}|ShipEstimateID:|{shipEstimateID}`.

**OrderCloud Side -** This integration should be triggered by the **`PostOrderSubmit`** Checkout Integration Event. Learn more about [checkout integration events](https://ordercloud.io/knowledge-base/order-checkout-integration); 

## Set up steps

- You should set up a .NET middleware project using the Catalyst library and starter project. [See guide](https://ordercloud.io/knowledge-base/start-dotnet-middleware-from-scratch).
- Using the OrderCloud API Portal, configure an Order Chekout IntegrationEvent object to point to your new middleware. [See guide](https://ordercloud.io/knowledge-base/order-checkout-integration)
- Create a taxjar account online and retrieve all the configuration variables required in [TaxJarConfig.cs](./TaxJarConfig.cs); 
	- BaseUrl
		- Likely "https://api.sandbox.taxjar.com" or "https://api.taxjar.com")
    - APIToken 
		- Find at https://app.taxjar.com/ under Account -> TaxJar API -> Generate Token
- Within your .NET code project, create an instance of [TaxJarCommand.cs](./TaxJarCommand.cs). Use the method `CalculateEstimateAsync` within the **`OrderCalculate`** Checkout Integration Event.  Use the method `CommitTransactionAsync` within the **`PostOrderSubmit`** Checkout Integration Event. 

## Interfaces

- It conforms to the [ITaxCalculator](../../Interfaces/ITaxCalculator.cs) interface.
