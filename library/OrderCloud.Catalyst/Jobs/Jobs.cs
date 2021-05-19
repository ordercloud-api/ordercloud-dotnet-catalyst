using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Jobs
{
    public class Jobs
    {
        // Example Job to forward an order to a third party order managememt system
        private readonly ForwardOrdersJob _forwardJob;
        [FunctionName("ThirdPartyOrderProcessing")]
        public async Task Run(
        [ServiceBusTrigger(
            queueName: "%ServiceBusSettings:OrderProcessingQueueName%",
            Connection = "ServiceBusSettings:ConnectionString")]
        Message message,
        MessageReceiver messageReceiver,
        [ServiceBus(
            queueOrTopicName: "%ServiceBusSettings:OrderProcessingQueueName%",
            Connection = "ServiceBusSettings:ConnectionString" )]
        MessageSender messageSender,
        ILogger logger) => await _forwardJob.Run(logger, message, messageReceiver, messageSender);
    }
}
