using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Demo.Hotels.Api.Core;
using Demo.Hotels.Api.Infrastructure.DataAccess;
using Microsoft.Extensions.Logging;

namespace Demo.Hotels.Api.Features.CancelReservation
{
    public class CancelReservationCommandHandler : ICommandHandler<CancelReservationCommand>
    {
        private const string CancelledReservationsTable = "cancelledreservations";
        private readonly TableServiceClient _serviceClient;
        private readonly ILogger<CancelReservationCommandHandler> _logger;

        public CancelReservationCommandHandler(TableServiceClient serviceClient, ILogger<CancelReservationCommandHandler> logger)
        {
            _serviceClient = serviceClient;
            _logger = logger;
        }
        
        public async Task<Result> ExecuteAsync(CancelReservationCommand command)
        {
            var tableClient = _serviceClient.GetTableClient(CancelledReservationsTable);
            if (tableClient == null)
            {
                return Result.Failure(ErrorCodes.TableClientNotFound, ErrorMessages.TableClientNotFound);
            }

            var partitionKey = command.Status.ToString().ToUpper();
            var rowKey = command.ReservationId.ToUpper();
            var entity = new TableEntity(partitionKey, rowKey)
            {
                {nameof(command.CorrelationId), command.CorrelationId},
                {nameof(command.Name), command.Name},
                {nameof(command.City), command.City}
            };

            var response = await tableClient.UpsertEntityAsync(entity);
            if (response.IsError)
            {
                _logger.LogError("{CorrelationId} upsert failed because {FailedReason}", command.CorrelationId, response.ReasonPhrase);
                return Result.Failure(ErrorCodes.FailedUpsert, ErrorMessages.FailedUpsert);
            }

            return Result.Success();
        }
    }
}