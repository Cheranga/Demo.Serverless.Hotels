using System;
using System.Threading.Tasks;
using Demo.Hotels.Api.DTO.Messages;
using Demo.Hotels.Api.Services;
using FluentValidation;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Demo.Hotels.Api.Functions
{
    public class HotelCancellationFunction
    {
        private readonly IValidator<CancelHotelReservationMessage> _validator;
        private readonly ICancelHotelReservationService _service;
        private readonly ILogger<HotelCancellationFunction> _logger;

        public HotelCancellationFunction(IValidator<CancelHotelReservationMessage> validator, ICancelHotelReservationService service, ILogger<HotelCancellationFunction> logger)
        {
            _validator = validator;
            _service = service;
            _logger = logger;
        }
        
        [FunctionName(nameof(HotelCancellationFunction))]
        public async Task Run([QueueTrigger("%HotelCancellationQueue%", Connection = "QueueSource")]CancelHotelReservationMessage message)
        {
            var validationResult = await _validator.ValidateAsync(message);
            if (!validationResult.IsValid)
            {
                throw new Exception("Invalid message received");
            }

            var operation = await _service.CancelAsync(message);
            if (!operation.Status)
            {
                throw new Exception(operation.ErrorMessage);
            }
        }
    }
}
