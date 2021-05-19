using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderCloud.Catalyst.Models.OrderForwarding;
using OrderCloud.SDK;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Jobs
{
    //  ForwardOrdersJob is an example of a custom job that could forward orders to a third party order management system
    public class ForwardOrdersJob : BaseJob
    {
        private int RetryTemporaryErrorsAfterMinutes = 10;
        private const int MaxDeadLetterDescriptionLength = 4096;
        private Random jitterer = new Random();


        public async Task Run(ILogger logger, Message message, MessageReceiver messageReceiver, Microsoft.Azure.ServiceBus.Core.MessageSender messageSender)
        {
            _logger = logger;
            var messageString = Encoding.UTF8.GetString(message.Body);
            var jsonMessage = JsonConvert.DeserializeObject<ExampleMessageType>(messageString);

            //  Process our job and forward the order
            var result = await TryProcessJobAsync(jsonMessage);

            //  Examine results. Log information and Retry job if necessary
            var lockToken = message.SystemProperties.LockToken;
            switch (result)
            {
                case ResultCode.Success:
                    logger.LogInformation($"Completed the message {message.MessageId} due to successful handling.");
                    break;
                case ResultCode.TemporaryFailure:
                    int resubmitCount = message.UserProperties.ContainsKey("ResubmitCount") ? (int)message.UserProperties["ResubmitCount"] : 0;
                    if (resubmitCount > 5)
                    {
                        await messageReceiver.DeadLetterAsync(lockToken, "Exceeded max retries", GetDeadLetterDescription());
                        logger.LogInformation("$Dead lettered the message due to exceeding max retries, this will need to be retried manually.");
                    }
                    else
                    {
                        await ResubmitMessageAsync(message, messageSender, resubmitCount);
                        logger.LogInformation($"Resubmitted the message {message.MessageId} to be tried again in {RetryTemporaryErrorsAfterMinutes} minutes");
                    }
                    break;
                case ResultCode.PermanentFailure:
                    await messageReceiver.DeadLetterAsync(lockToken, "Permanent failure", GetDeadLetterDescription());
                    logger.LogInformation("$Dead lettered the message due to a permanent failure, this will need to be retried manually.");
                    break;
                default:
                    break;
            }
            LogProgress();
            if (result != ResultCode.Success)
            {
                // throw an error so the function fails and we can see it as an error in azure functions monitor logs
                throw new Exception("There were one or more errors during job");
            }
        }

        private async Task ResubmitMessageAsync(Message message, Microsoft.Azure.ServiceBus.Core.MessageSender sender, int resubmitCount)
        {
            // https://markheath.net/post/defer-processing-azure-service-bus-message
            var clone = message.Clone();
            clone.UserProperties["ResubmitCount"] = resubmitCount + 1;
            clone.ScheduledEnqueueTimeUtc = DateTime.UtcNow
                .AddMinutes(RetryTemporaryErrorsAfterMinutes)
                .AddSeconds(jitterer.Next(0, 120)); // plus some jitter up to 2 minutes
            await sender.SendAsync(clone);
        }

        private string GetDeadLetterDescription()
        {
            // truncate the description if it exceeds max
            var message = Failed.LastOrDefault();
            if (message.Length < MaxDeadLetterDescriptionLength)
            {
                return message;
            }
            var truncatedSuffix = "...MESSAGE WAS TRUNCATED";
            var truncatedMessage = message.Substring(0, MaxDeadLetterDescriptionLength - truncatedSuffix.Length);
            return truncatedMessage + truncatedSuffix;
        }

        private async Task<ResultCode> TryProcessJobAsync(ExampleMessageType message)
        {
            try
            {
                return await ProcessJobAsync(message);
            }
            catch (Exception ex)
            {
                LogFailure($"Unhandled exception in ProcessJobAsync - {ex.Message} {ex.InnerException.Message} {ex.StackTrace}");
                return ResultCode.PermanentFailure;
            }
        }

        private async Task<ResultCode> ProcessJobAsync(ExampleMessageType message)
        {
            try
            {
                _logger.LogInformation($"Processing OrderID: {message.OrderID}");
                // POST order to third party system here
                return ResultCode.Success;
            }
            catch (CustomException ex)
            {
                //  Handle and log exception from your third party System
                LogFailure($"Third Party Error: {ex.Message} Third Party Code: {ex.ErrorCode} {ex.StackTrace}");
                return IsTransientError(ex.HttpStatus) ? ResultCode.TemporaryFailure : ResultCode.PermanentFailure;
            }
            catch (OrderCloudException ex)
            {
                //  Handle and log exception from OrderCloud
                LogFailure($"{ex.InnerException?.Message} {ex?.InnerException?.InnerException?.Message} { JsonConvert.SerializeObject(ex.Errors) } {ex.StackTrace }");
                return IsTransientError(ex.HttpStatus) ? ResultCode.TemporaryFailure : ResultCode.PermanentFailure;
            }
            catch (Exception ex)
            {
                //  Catch all for other exceptions
                LogFailure($"{ex.Message} {ex?.InnerException?.Message} {ex.StackTrace}");
                return ResultCode.PermanentFailure;
            }
        }

        private bool IsTransientError(HttpStatusCode? status)
        {
            return
                status == null ||
                status == HttpStatusCode.InternalServerError ||
                status == HttpStatusCode.RequestTimeout ||
                status == HttpStatusCode.TooManyRequests;
        }
    }
}
