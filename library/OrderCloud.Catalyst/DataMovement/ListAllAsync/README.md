## ListAll Extension Methods

#### Purpose

List endpoints in Ordercloud are paginated with a maximum of 100 records per request. ListAll methods defined in Catalyst will return a list of all records. 

#### Details

For every List function in the [OrderCloud SDK](https://github.com/ordercloud-api/ordercloud-dotnet-sdk/blob/master/src/OrderCloud.SDK/Generated/Resources.cs), Catalyst defines a ListAll [extension method](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods). ListAll methods iteratively call the underlying paginated API in order to get all records. 
The return type is a simple List<T> as opposed to a ListPage<T>. Function inputs also differ between ListAll and the normal List functions. Paging, Sorting, and Searching are not supported.
However, all other inputs should remain unchanged, including [filters](https://ordercloud.io/features/advanced-querying#filtering).


#### Examples

```c#
// get all products visible to the user
var products = _oc.Me.ListAllProductsAsync();

// function-specific inputs are the same as ListAsync().
var orderID = "xxxxxxxx";
var lineItems = _oc.LineItems.ListAllAsync(OrderDirection.Incomming, orderID);
Console.Log(lineItems.Count)

// optionally, apply filters
var expensiveLineItems = _oc.LineItems.ListAllAsync(OrderDirection.Incomming, orderID, filters: "LineTotal=>100");

// Imagine you want to write line items to a comma separated value file.
// use a call-back function that will be triggered for each list page that's found. This saves memory because only one list page is stored at a given time. 
CloudAppendBlob csvFile = client.GetContainerReference("...").GetAppendBlobReference("..."); // A reference to an append blob in Azure storage
// Request Line Items
_oc.LineItems.ListAllAsync(OrderDirection.Incomming, orderID, listPage => {		
	// Write the lineItems in batches of 100, the default pageSize for ListAll
	var lineItems = listPage.Items;
	foreach(var lineItem in lineItems) {
		string csvLine = ConvertToCSV(lineItem);
		csvFile.AppendTextAsync(csvLine); // save that batch of lineItems in the cloud. Allows them to be garbage collected locally. 
	}	
});
```

#### Best Practices

These are not methods to throw around lightly! If there are many records, they can be very "expensive" both in time and memory. Avoid listing all records when you can.
Be aware of roughly how many records you expect. Over 3000, these methods are less recomended. For larger data sets and for applications where speed is not critical like a nightly sync, we recomend a batch approach where you repeatedly list one page of data, apply it somewhere, and then allow it to be garbage collected.  
