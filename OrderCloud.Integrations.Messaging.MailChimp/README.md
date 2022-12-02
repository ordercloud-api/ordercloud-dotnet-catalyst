# OrderCloud.Integrations.Messaging.MailChimp

This project brings email to your ecommerce app using the [MailChimp Transactional API](https://mailchimp.com/developer/transactional/api/) (Formerly Mandrill). Messaging can include marketing campaigns, batches, or single transactional messages send via email, SMS, or another channel. `MailChimpService.cs` implements to the [`ISingleEmailSender`](../OrderCloud.Catalyst/Integrations/Interfaces/ISingleEmailSender.cs) interface published in the base library ordercloud-dotnet-catalyst.

## Intro to Transactional Email  
"Transactional" is the term used to describe emails that are sent in response to specific actions the user takes in an app. Think forgot password or order confirmation emails. This is as opposed to batch messages or marketing campaigns. Different regulations apply to these different categories. For example, marketing campaigns should have an unsubscribe feature, whereas that is not required for transactional. Also, most major messaging platforms provide distinct functionality for transactional messages. 

Here are features which the `ISingleEmailSender` supports
- Basic email properties like Email and Name for To and From addresses, Subject, and Content.
- Multiple recipients, with the ability to hide them from each other or put them all on one thread. 
- Pre-saved templates which can be populated by dynamic data. 
- In the case of multiple recipients who are hidden from each other, it supports dynamic data overrides by recipient.  
- Attached Files

Here are features which are *not supported* in the interface. You can find these features with some providers.
- Future Scheduled Emails
- ReplyTo Email and Name

## Transactional Messaging in OrderCloud 
All OrderCloud Ecommerce solutions need transactional messages - at a minimum for events like forgot password and order confirmation. The OrderCloud API has a resource called "Message Senders" to support this. Start by reading [this article about message senders](https://ordercloud.io/knowledge-base/message-senders). It includes a list of supported platform events which trigger messages. OrderCloud includes a built-in messaging integration with MailChimp (Mandrill). This integration allows you to enter a MailChimp API and will send emails based on templates in your account. It includes a set of dynamic data variables for each message type. This is nice because you don't have to write any code to get emails working. However, there are common scenarios which the built-in integration *cannot* handle. 
- If you want to use a provider other than MailChimp Mandrill. Create a custom message sender and connect it to your API routes.
- If you want to send emails triggered by an event which is not included in the supported list. Write your own logic, possibly triggered by an OrderCloud webhook.
- If you want to display dynamic data in an email which is not included in the standard integration's template variables. Create a custom message sender and connect it to your API routes.

In these scenarios this project can help!

## Package Installation 

There is no nuget package for this integration currently.

## Authentication and Injection

You will need these configuration data points to authneticate to the MailChimp API - *ApiKey*. Transactional emails require a extra add-on to the standard MailChimp payment package. Follow [these steps](https://mailchimp.com/help/about-api-keys/) to get API credentials. 

```c#
var mailChimpService = new MailChimpService(new MailChimpConfig()
{
	TransactionalApiKey = "...",
});
```

For efficient use of compute resources and clean code, create 1 MailChimpService object and make it available throughout your project using inversion of control dependency injection. 

```c#
services.AddSingleton<ISingeEmailSender>(mailChimpService);
```

Notice that ISingeEmailSender is not specific to MailChimp. It is general to the problem domain and comes from the upstream ordercloud-dotnet-catalyst package. 

## Usage 

Inject the interfaces and use them within route logic. Rely on the interface whenever you can, not MailChimpService. The layer of abstraction that ISingleEmailSender provides decouples your code from MailChimp as a specific provider and hides some internal complexity.

```c#
public class SendEmailService 
{
	private readonly ISingleEmailSender _sender;

	public SendEmailService(ISingleEmailSender sender)
	{
		// Inject interface. Implementation will depend on how services were registered, MailChimpService in this case.
		_sender = sender; 
	}

	public async Task SendForgotPasswordEmailAsync(SetPasswordMessageSenderPayloadWithXp payload)
	{
		var templateData = new Dictionary<string, object>()
		{
			{ "username", payload.EventBody.Username },
			{ "passwordtoken", payload.EventBody.PasswordRenewalAccessToken },
			{ "passwordverification", payload.EventBody.PasswordRenewalVerificationCode },
			{ "passwordrenewalurl", payload.EventBody.PasswordRenewalUrl }
		};
		var to = payload.Recipient.Email;
		var from = new EmailAddress("noreply@fake.com");
		var subject = "Reset Password For OrderCloud Customer";
		var templateID = "reset-password-template";
		var message = EmailBuilder.BuildTemplateEmail(to, from, subject, templateID, templateData);

		await _sender.SendSingleEmailAsync(message);
	}

	public async Task SendOrderConfirmationEmailAsync(OrderMessageSenderPayloadWithXp payload)
	{
		var templateData = new Dictionary<string, object>()
		{
			{ "firstname", payload.EventBody.Order.FromUser.FirstName },
			{ "lastname", payload.EventBody.Order.FromUser.LastName },
			{ "orderid", payload.EventBody.Order.ID },
			{ "total", payload.EventBody.Order.Total.ToString() },
			// supports arbitary json-like data inside the Dictionary entries. In this case an array of product objects.
			{ "products", payload.EventBody.LineItems.Select(li =>   
				{
					return new
					{
						cost = li.LineTotal.ToString(),
						quantity = li.Quantity.ToString(),
						productdesc = li.Product.Description,
						productname = li.Product.Name,
						shiptopostalcode = li.ShippingAddress.Zip,
					};
				}) 
			}
		}
		...
	}
}
```

Another good engineering pattern for email notifications can be to processes them asynchronously from the main thread so that they do not hold up other work. That can be accomplished by dropping a message with the details on a queue and processing the queue in an Azure Function. The queue service in this example is intentionally a little vauge.


```c#
public class SendEmailService 
{
	private readonly IQueueService _queue;

	public SendEmailService(IQueueService queue)
	{
		_queue = queue; 
	}

	public async void SendForgotPasswordEmail(SetPasswordMessageSenderPayloadWithXp payload)
	{
		...
		EmailMessage message = EmailBuilder.BuildTemplateEmail(...);
		_queue.Drop("email-queue", message)
	}
}
```
```c#
public class Functions 
{
	private readonly IQueueService _queue;
	private readonly ISingleEmailSender _sender;

	public Functions(IQueueService queue, ISingleEmailSender sender) 
	{
		_queue = queue;
		_sender = sender;
	}

	// Will trigger an Azure Serverless Function when a message is placed on the queue
	public async Task RunEmailSendAsync([QueueTrigger("email-queue")] string messageName, TextWriter log)
	{
		string body = _queue.Read(messageName);
		EmailMessage message = JsonConvert.DeserializeObject(body);
		await _sender.SendSingleEmailAsync(message);
	}
}
```

This library also supports more complex cases that require mulitple merchant accounts with different credentials. For example, in a franchise business model where each location is independent but all sell on one ecommerce solution. In that case, still inject one instance of MailChimpService exactly as above. You can provide empty strings for the credentials. However, when you call methods on the interfaces, provide the optional `overrideConfig` parameter. 

```c#
MailChimpConfig overrideConfig = await FetchAccountCredentials(supplierID)
var message = EmailBuilder.BuildEmail(...);
await _sender.SendSingleEmailAsync(message, overrideConfig);
```
