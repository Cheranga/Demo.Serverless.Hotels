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
        private readonly ITableStorageFactory _tableStorageFactory;
        private readonly ILogger<CancelReservationCommandHandler> _logger;

        public CancelReservationCommandHandler(ITableStorageFactory tableStorageFactory, ILogger<CancelReservationCommandHandler> logger)
        {
            _tableStorageFactory = tableStorageFactory;
            _logger = logger;
        }
        
        public async Task<Result> ExecuteAsync(CancelReservationCommand command)
        {
            var tableClient = _tableStorageFactory.GetTableClient("cancelledreservations");
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

    public interface ITableStorageFactory
    {
        TableClient GetTableClient(string tableName);
    }

    public class TableStorageFactory : ITableStorageFactory
    {
        private readonly IDictionary<string, TableClient> _tableClientsMappedByTable;
        
        public TableStorageFactory(IDictionary<string, TableClient> tableClientsMappedByTable)
        {
            _tableClientsMappedByTable = tableClientsMappedByTable;
        }
        public TableClient GetTableClient(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return null;
            }

            tableName = tableName.Trim().ToUpper();

            if (_tableClientsMappedByTable.TryGetValue(tableName, out var tableClient))
            {
                return tableClient;
            }

            return null;
        }
    }
}