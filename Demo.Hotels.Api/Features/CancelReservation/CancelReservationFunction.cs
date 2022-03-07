using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Demo.Hotels.Api.Features.CancelReservation
{
    public class CancelReservationFunction
    {
        private readonly IValidator<CancelHotelReservationMessage> _validator;
        private readonly ICancelHotelReservationService _service;
        private readonly ILogger<CancelReservationFunction> _logger;

        public CancelReservationFunction(IValidator<CancelHotelReservationMessage> validator, ICancelHotelReservationService service, ILogger<CancelReservationFunction> logger)
        {
            _validator = validator;
            _service = service;
            _logger = logger;
        }
        
        [FunctionName(nameof(CancelReservationFunction))]
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
