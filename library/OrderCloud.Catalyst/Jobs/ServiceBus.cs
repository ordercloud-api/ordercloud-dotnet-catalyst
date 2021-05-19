using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Jobs
{
    public class ServiceBus
    {
        private readonly ConcurrentDictionary<string, ServiceBusSender> senders = new ConcurrentDictionary<string, ServiceBusSender>();
        private readonly ServiceBusClient _client;

        //  Call this method to send a message via ServiceBus and trigger your job.
        public async Task SendMessage<T>(string queueName, T message, double? afterMinutes = null)
        {
            var sender = senders.GetOrAdd(queueName, _client.CreateSender(queueName));
            var messageString = JsonConvert.SerializeObject(message);
            var messageBytes = Encoding.UTF8.GetBytes(messageString);
            if (afterMinutes == null)
            {
                // send message immediately
                await sender.SendMessageAsync(new ServiceBusMessage(messageBytes));
            }
            else
            {
                // send message after x minutes
                var afterMinutesUtc = DateTime.UtcNow.AddMinutes((double)afterMinutes);
                await sender.SendMessageAsync(new ServiceBusMessage(messageBytes) { ScheduledEnqueueTime = afterMinutesUtc });
            }

        }
    }
}
