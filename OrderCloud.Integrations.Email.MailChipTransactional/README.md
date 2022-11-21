# OrderCloud.Integrations.Payment.BlueSnap

## Payment Processing Basics


## Package Installation 

This nuget library can be installed in the context of a .NET server-side project. If you already have a .NET project, great. If not, you can [follow this guide](https://ordercloud.io/knowledge-base/start-dotnet-middleware-from-scratch).

```dotnet add package OrderCloud.Integrations.Payment.BlueSnap```

## Authentication and Injection

You will need these configuration data points to authneticate to the BlueSnap API - *BaseUrl*, *APIUsername*, and *APIPassword*. Follow [these steps](https://developers.bluesnap.com/docs/api-credentials) to get API credentials. 

```c#
var blueSnapService = new BlueSnapService(new BlueSnapConfig()
{
	BaseUrl = "https://sandbox.bluesnap.com" // or https://ws.bluesnap.com
	APIUsername = "...",
	APIPassword = "...",
});
```

For efficient use of compute resources and clean code, create 1 BlueSnapService object and make it available throughout your project using inversion of control dependency injection. 

```c#
services.AddSingleton<ICreditCardProcessor>(blueSnapService);
services.AddSingleton<ICreditCardSaver>(blueSnapService);
```

Notice that ICreditCardProcessor and ICreditCardSaver are not specific to BlueSnap. They are general to the problem domain and come from the upstream ordercloud-dotnet-catalyst package. 

## Usage 

Inject the interfaces and use them within route logic. Rely on the interfaces whenever you can, not BlueSnapService. The layer of abstraction that ICreditCardProcessor and ICreditCardSaver provide decouples your code from BlueSnap as a specific provider and hides some internal complexity.

```c#
public class CreditCardCommand 
{
	private readonly ICreditCardProcessor _creditCardProcessor;
	private readonly ICreditCardSaver _creditCardSaver;

	public CreditCardCommand(ICreditCardProcessor creditCardProcessor, ICreditCardSaver creditCardSaver)
	{
		// Inject interface. Implementation will depend on how services were registered, BlueSnapService in this case.
		_creditCardProcessor = creditCardProcessor; 
		_creditCardSaver = creditCardSaver;
	}

	...

	// Use in pre-submit webhook or proxy route
	public async Task<PaymentWithXp> AuthorizeCardPayment(OrderWorksheetWithXp worksheet, PaymentWithXp payment)
	{
		var authorizeRequest = new AuthorizeCCTransaction()
		{
			OrderID = worksheet.Order.ID,
			Amount = worksheet.Order.Total,
			Currency = worksheet.Order.Currency,
			AddressVerification = worksheet.Order.BillingAddress,
			CustomerIPAddress = "...",
			CardDetails = new PCISafeCardDetails()
		};
		var payWithSavedCard = payment?.xp?.SafeCardDetails?.SavedCardID != null;
		if (payWithSavedCard)
		{
			authorizeRequest.CardDetails.SavedCardID = payment.xp.SafeCardDetails.SavedCardID;
			authorizeRequest.ProcessorCustomerID = worksheet.Order.FromUser.xp.PaymentProcessorCustomerID;
		}
		else
		{
			authorizeRequest.CardDetails.CardToken = payment?.xp?.SafeCardDetails?.Token;
		}
		
		CCTransactionResult authorizationResult = await _creditCardProcessor.AuthorizeOnlyAsync(authorizeRequest);

		Require.That(authorizationResult.Succeeded, new ErrorCode("Payment.AuthorizeDidNotSucceed", authorizationResult.Message), authorizationResult);

		await _oc.Payments.PatchAsync<PaymentWithXp>(OrderDirection.All, worksheet.Order.ID, payment.ID, new PartialPayment { Accepted = true, Amount = authorizeRequest.Amount });
		var updatedPayment = await _oc.Payments.CreateTransactionAsync<PaymentWithXp>(OrderDirection.All, worksheet.Order.ID, payment.ID, new PaymentTransactionWithXp()
		{
			ID = authorizationResult.TransactionID,
			Amount = payment.Amount,
			DateExecuted = DateTime.Now,
			ResultCode = authorizationResult.AuthorizationCode,
			ResultMessage = authorizationResult.Message,
			Succeeded = authorizationResult.Succeeded,
			Type = PaymentTransactionType.Authorization.ToString(),
			xp = new PaymentTransactionXp
			{
				TransactionDetails = authorizationResult,
			}
		});
		return updatedPayment;
	}
}
```

This library also supports more complex cases that require mulitple merchant accounts with different credentials. For example, in a franchise business model where each location is independent but all sell on one ecommerce solution. In that case, still inject one instance of BlueSnapService exactly as above. You can provide empty strings for the credentials. However, when you call methods on the interfaces, provide the optional `configOverride` parameter. 

```c#
BlueSnapConfig configOverride = await FetchPaymentAccountCredentials(supplierID)
var authorize = new AuthorizeCCTransaction();
List<List<ShipMethods> rates = await _creditCardProcessor.AuthorizeOnlyAsync(authorize, configOverride);
```
