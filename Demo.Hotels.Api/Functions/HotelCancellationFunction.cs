using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Demo.Hotels.Api.Functions
{
    public class HotelCancellationFunction
    {
        [FunctionName("HotelCancellationFunction")]
        public void Run([QueueTrigger("%HotelConfig:CancellationQueue%", Connection = "Hotel")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
