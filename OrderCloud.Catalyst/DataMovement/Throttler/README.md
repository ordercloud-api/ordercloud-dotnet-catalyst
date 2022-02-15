## Throttler function

#### Purpose 
The throttler function provides a method for running many asynchronous tasks in concurrency. The function takes parameters for limiting the number of concurrent threads and enforcing a minimum wait time before starting each task. This allows for efficient performance and for avoiding an unnecessary load on the OrderCloud API.

#### Parameters
The throttler function takes in four parameters...
- items: A list of items to iterate over. This list can serve as an input into your asynchronous function.
- minPause: A minimum pause time to enforce before beginning a new task (in milliseconds).
- maxConcurrent: the maximum number of tasks to be running at once.
- op: Your function you would like to implement

#### Interpretation
The throttler will start after every **minPuase** interval. However, if the **maxConcurrent** number of tasks are still pending the function will also wait for one or more tasks to finish. 

#### Example 
```c#
// Make a bunch of async function calls sequentially. Downside is, it could be slow.
foreach (var prod in products) {
    await ocClient.Products.Create(prod);
}

// Make a bunch of async function calls concurrently. Downside is, could strain resources. 
var requests = products.Select(prod => ocClient.Products.Create(prod));
await Task.WhenAll(requests);


// A happy medium - some speed up, but a bounded degree of concurrency. 
var maxConcurency = 20;
var minPause = 100 // ms
await Throttler.RunAsync(cars, minPause, maxConcurency, prod => ocClient.Products.Create(prod));

// Example of returning results 
var products = await Throttler.RunAsync(cars, minPause, maxConcurency, prod => ocClient.Products.Get(prod.ID));

```

## Best practices
Throttler is not recomended for scenarios where sequential requests are fast enough. For example, it often does not matter if an after hours job takes 1 hour or 2. Try making your requests in series first and only add Throttler if you need it.

It is not always obvious what the perfect combination of parameters are. This function will be highly dependent on your specific scenario and the endpoints you are hitting. We recommend a minPause of 100 and a maxConcurrent of 20 as a starting point. You can then tweak these parameters from here to find out what works best for your project.
