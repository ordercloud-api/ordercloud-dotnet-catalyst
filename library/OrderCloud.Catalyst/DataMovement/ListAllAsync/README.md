## ListAll Extension Methods

#### Purpose

List endpoints in OrderCloud are paginated with a maximum of 100 records per request. ListAll methods defined in Catalyst will return a list of all records. 

#### Details

For every List function in the [OrderCloud SDK](https://github.com/ordercloud-api/ordercloud-dotnet-sdk/blob/master/src/OrderCloud.SDK/Generated/Resources.cs), Catalyst defines a ListAll [extension method](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods). ListAll methods iteratively call the underlying paginated API in order to get all records. 
The return type is a simple List<T> as opposed to a ListPage<T>. Function inputs also differ between ListAll and the normal List functions. Paging, Sorting, and Searching are not supported.
However, all other inputs should remain unchanged, including [filters](https://ordercloud.io/features/advanced-querying#filtering).


#### Examples

```c#
// get all products visible to the user
var products = _oc.Me.ListAllProductsAsync();
```
```c#
// function-specific inputs are the same as ListAsync().
var orderID = "xxxxxxxx";
var lineItems = _oc.LineItems.ListAllAsync(OrderDirection.Incomming, orderID);
Console.Log(lineItems.Count)
```
```c#
// optionally, apply filters
var expensiveLineItems = _oc.LineItems.ListAllAsync(OrderDirection.Incomming, orderID, filters: "LineTotal=>100");
```
```c#
// Imagine you want to write line items to a comma separated value file.
// use a call-back function that will be triggered for each list page that's found. This saves memory because only one list page is stored at a given time. 
CloudAppendBlob csvFile = client.GetContainerReference("...").GetAppendBlobReference("..."); // A reference to an append blob in Azure storage
// Request Line Items
_oc.LineItems.ListAllAsync(OrderDirection.Incomming, orderID, listPage => {		
	// Write the lineItems in batches of 100, the default pageSize for ListAll
	var lineItems = listPage.Items;
	foreach(var lineItem in lineItems) {
		string csvLine = ConvertToCSV(lineItem);
		await csvFile.AppendTextAsync(csvLine); // save that batch of lineItems in the cloud. Allows them to be garbage collected locally. 
	}
});
```

## Get multiple resources By ID 

There is a reasonably efficient technique for getting mulitple resources by ID from the OrderCloud API. It uses an ID filter like so `/products?ID=my-product-id|my-2-product-id|my-3-product-id`. There are some minor gotchas with using this method directly. What if the list contains more that 100 ids and paging is necessary? Or, what if the ids have many characters and exceed the OrderCloud limit of 2086 chars in a url? Catalyst includes helper methods to abstract out these concerns. Just provide a list of ids. 

Note that this method will not error if an id doesn't exist. It will simply omit it from the results.   

```c#
// get a list of specific products
var ids = new List<string> { "my-product-id", "my-2-product-id", "my-3-product-id" };
var products = _oc.Me.ListProductsByIDAsync(ids);
Console.log(products.Count); // 3 if all ids are found. 0 if none are found.
```

## Best Practices

These are not methods to throw around lightly! If there are many records, they can be very "expensive" both in time and memory. Avoid listing all records when you can.
Be aware of roughly how many records you expect. Over 3000, list all is less recomended. For larger data sets and for applications where speed is not critical like a nightly sync, we recomend a batch approach where you repeatedly list one page of data, apply it somewhere, and then allow it to be garbage collected.  
