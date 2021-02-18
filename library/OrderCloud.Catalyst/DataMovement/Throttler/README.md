# Throttler function

## Purpose 
The throttler function provides a method for running many asynchronous tasks in concurrency. The function takes parameters for limiting the number of concurrent threads and enforcing a minimum wait time before starting each task. This allows for efficient performance and for avoiding an unnesecary load on the ordercloud API.

## Parameters
The throttler function takes in four parameters...
- items: A list of items to iterate over. This list can serve as an input into your asynchonous function.
- minPause: A minimum pause time to enforce before beginning a new task (in milliseconds).
- maxConcurrent: the maximum number of tasks to be running at once.
- op: Your function you would like to implement

## Interpretation
The throttler will start after every **minPuase** interval. However, if the **maxConcurrent** number of tasks are still pending the function will also wait for one or more tasks to finish. 

## Example 
```c#
//allSuppliers is a list of all the suppliers in our OrderCloud organization
var allSuppliers = await ListAllAsync.List(page => orderCloudClient.Suppliers.ListAsync<HSSupplier>(page: page, pageSize: 100));

//We can use the throttler to iterate over all the suppliers ino our list and patch each supplier with an xp value for a notification recipient email.
//This will send a new patch request every 500 milliseconds and allow for a maximum of 20 concurrent requests.
await Throttler.RunAsync(allSuppliers, 100, 20, supplier =>
    orderCloudClient.Suppliers.PatchAsync(supplier.ID, new PartialSupplier { xp = new { NotificationRcpt: 'test@noreply.com' } }));

```

## Best practices
It is not always obvious what the perfect combination of parameters are. This function will be highly dependent on your specific scenario and the endpoints you are hitting. We recommend a minPause of 100 and a maxConcurrent of 20 as a starting point. You can then tweak these parameters from here to find out what works best for your project.