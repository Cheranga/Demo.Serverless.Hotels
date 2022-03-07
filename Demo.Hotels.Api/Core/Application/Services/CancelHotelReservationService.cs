using System.Threading.Tasks;
using Demo.Hotels.Api.Core.Domain.Messages;
using Demo.Hotels.Api.Core.Domain.Requests;
using Demo.Hotels.Api.Core.Domain.Responses;
using Demo.Hotels.Api.Infrastructure.DataAccess;
using Demo.Hotels.Api.Infrastructure.HTTP;
using Microsoft.Extensions.Logging;

namespace Demo.Hotels.Api.Core.Application.Services
{
    public class CancelHotelReservationService : ICancelHotelReservationService
    {
        private readonly ICustomerApiService _customerApiService;
        private readonly ICommandHandler<UpsertCustomerCommand> _commandHandler;
        private readonly IEmailService _emailService;
        private readonly ILogger<CancelHotelReservationService> _logger;

        public CancelHotelReservationService(ICustomerApiService customerApiService, ICommandHandler<UpsertCustomerCommand> commandHandler, IEmailService emailService, ILogger<CancelHotelReservationService> logger)
        {
            _customerApiService = customerApiService;
            _commandHandler = commandHandler;
            _emailService = emailService;
            _logger = logger;
        }
        
        public async Task<Result> CancelAsync(CancelHotelReservationMessage request)
        {
            var getCustomerOperation = await GetCustomerAsync(request);
            if (!getCustomerOperation.Status)
            {
                return Result.Failure(getCustomerOperation.ErrorCode, getCustomerOperation.ErrorMessage);
            }

            var saveCustomerOperation = await SaveCustomerAsync(request.CorrelationId, CancellationStatus.Received, getCustomerOperation.Data);
            if (!saveCustomerOperation.Status)
            {
                return saveCustomerOperation;
            }

            var sendEmailOperation = await SendEmailAsync(request, getCustomerOperation.Data);
            if (!sendEmailOperation.Status)
            {
                return Result.Failure(sendEmailOperation.ErrorCode, sendEmailOperation.ErrorMessage);
            }

            saveCustomerOperation = await SaveCustomerAsync(request.CorrelationId, CancellationStatus.Confirmed, getCustomerOperation.Data);
            return saveCustomerOperation;
        }

        private async Task<Result<GetCustomerResponse>> GetCustomerAsync(CancelHotelReservationMessage request)
        {
            var customerId = request.UserId % 11 == 0 ? 1 : request.UserId % 10;
            
            var getCustomerRequest = new GetCustomerRequest
            {
                CorrelationId = request.CorrelationId,
                CustomerId = customerId.ToString()
            };

            var operation = await _customerApiService.GetCustomerAsync(getCustomerRequest);
            return operation;
        }

        private async Task<Result> SaveCustomerAsync(string correlationId, CancellationStatus status, GetCustomerResponse response)
        {
            var command = new UpsertCustomerCommand
            {
                CorrelationId = correlationId,
                Name = response.Name,
                Email = response.Email,
                UserName = response.UserName,
                City = response.Address?.City,
                Street = response.Address?.Street,
                Suite = response.Address?.Suite,
                ZipCode = response.Address?.ZipCode,
                Status = status
            };

            var operation = await _commandHandler.ExecuteAsync(command);
            return operation;
        }

        private async Task<Result<SendCancelConfirmationEmailResponse>> SendEmailAsync(CancelHotelReservationMessage message, GetCustomerResponse customerData)
        {
            var request = new SendCancelConfirmationEmailRequest
            {
                CorrelationId = message.CorrelationId,
                Name = customerData.Name,
                Email = customerData.Email,
                ReservationId = message.ReservationId
            };

            var operation = await _emailService.SendEmailAsync(request);
            return operation;
        }
    }
}