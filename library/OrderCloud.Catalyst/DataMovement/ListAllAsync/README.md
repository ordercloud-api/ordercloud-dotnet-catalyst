# ListAllAsync Function

## Purpose

The List function provides a way to retirieve all elements of a specific type from OrderCloud. Since data is returned paginated in OrderCloud, this function allows calling out for multiple pages of data without any more implementation or looping logic.

## Parameters

The List and ListWithFacets function require the same parameter.

-listFunc: This parameter is the function you intend to get data from. Within your function call, you are able to set a few different parameters to decide how many items you want to retrieve. Those parameters are listed below. Also noting these properties are deptermined by the OrderCloud SDK depending on the resource.

- page: page sigifies what page is currently being retrieved. This will dynamically update as the function continues to loop through.
- pageSize: this determines how many records you want to return with each page.
- filter: optional parameter to filter results based on a specific property.

## Examples

```c#
// retrieves all related price schedules for the intial id. Notice the function signature that passes in page to the 'PriceSchedules.ListAsync' method. That is the parameter that updates until all pages are retrieved. The other parameters are optional
List<PriceSchedule> relatedPriceSchedules = await ListAllAsync.List((page) => _oc.PriceSchedules.ListAsync(search: initial.ID, page: page, pageSize: 100));

//To get all products with facets.
List<Product> allProducts = await ListAllAsync.ListWithFacets(page => _oc.Products.ListAsync(page: page, pageSize: 100, accessToken: token));

//A little longer example to show how ListAllAsync allows for any OrderCloud call, regardless of the parameters needed.
List<HSLineItem> existingLineItems = await ListAllAsync.List((page) => _oc.LineItems.ListAsync<HSLineItem>(OrderDirection.Outgoing, orderID, page: page, pageSize: 100, filters: $"Product.ID={lineItem.ProductID}", accessToken: verifiedUser.AccessToken));

```

## Best Practices

While OrderCloud handles these List calls pretty quick, it's still reccomended to only use this when needed. Along with that, if it's ever possible to save previous ListAll responses or sparingly call out, it's reccomended to do so to reduce chatter and raise available bandwidth.
