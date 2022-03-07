using System.Threading.Tasks;
using Demo.Hotels.Api.Core;
using Microsoft.Extensions.Logging;

namespace Demo.Hotels.Api.DataAccess
{
    public class UpsertCustomerCommandHandler : ICommandHandler<UpsertCustomerCommand>
    {
        private readonly ILogger<UpsertCustomerCommandHandler> _logger;

        public UpsertCustomerCommandHandler(ILogger<UpsertCustomerCommandHandler> logger)
        {
            _logger = logger;
        }
        
        public Task<Result> ExecuteAsync(UpsertCustomerCommand command)
        {
            return Task.FromResult(Result.Success());
        }
    }
}