## Forwarding Orders Job

## Purpose
This folder provides example code for how developers can forward OrderCloud orders to a third-party Order management system. In this example we rely on 
the Microsoft Azure ServiceBus for sending messages to a queue to trigger our forwarding function.
Check out the [Getting Started](https://github.com/Azure/azure-sdk-for-net/tree/Microsoft.Azure.ServiceBus_5.1.2/sdk/servicebus/Azure.Messaging.ServiceBus#getting-started) section of the Service Bus github page 
for more information on using the Azure Service Bus library for .NET.

## Details
Our "ThirdPartyOrderProcessing" job is configured to receive messages that are sent to our queueName: `"%ServiceBusSettings:OrderProcessingQueueName%"`. In this example this field is a reference to a value stored in our azure app configuration. 
Upon receiving a message the the `_forwardJob.Run()` method is called. This function will receive the message and Deserialize it to our `ExampleMessageType` which just consists of a single OrderID field. Then it continues to run the necessary process to forward the order, handle errors and log the results of this process. In this example we have some basic retry logic that will retry the order up to 10 times if necessary.
In order to send the message to our message listener to trigger the job we can call the `SendMessage()` method in the `ServiceBus` class. This can be done following submitting the order to OrderCloud in your applications order submit workflow.

## Best Practices
In this example we use a `BaseJob` class to store generic logic around logging and tracking counts of successful and failed operations. This is a useful pattern to employ when you have many jobs that all may share a subset of common functions. 

