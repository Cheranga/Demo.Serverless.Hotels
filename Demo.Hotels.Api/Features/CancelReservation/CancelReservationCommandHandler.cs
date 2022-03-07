using System.Threading.Tasks;
using Demo.Hotels.Api.Core;
using Demo.Hotels.Api.Infrastructure.DataAccess;
using Microsoft.Extensions.Logging;

namespace Demo.Hotels.Api.Features.CancelReservation
{
    public class CancelReservationCommandHandler : ICommandHandler<CancelReservationCommand>
    {
        private readonly ILogger<CancelReservationCommandHandler> _logger;

        public CancelReservationCommandHandler(ILogger<CancelReservationCommandHandler> logger)
        {
            _logger = logger;
        }
        
        public Task<Result> ExecuteAsync(CancelReservationCommand command)
        {
            return Task.FromResult(Result.Success());
        }
    }
}