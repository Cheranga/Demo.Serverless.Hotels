using System;
using Demo.Hotels.Api.Configs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Demo.Hotels.Api.Functions
{
    public class HotelCancellationFunction
    {
        private readonly ILogger<HotelCancellationFunction> _logger;

        public HotelCancellationFunction(ILogger<HotelCancellationFunction> logger)
        {
            _logger = logger;
        }
        
        [FunctionName("HotelCancellationFunction")]
        public void Run([QueueTrigger("CancellationQueue", Connection = "HotelConfig")]string myQueueItem, ILogger log)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
