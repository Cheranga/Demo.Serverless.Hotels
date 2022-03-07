﻿using System.Threading.Tasks;
using Demo.Hotels.Api.Core;

namespace Demo.Hotels.Api.DataAccess
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task<Result> ExecuteAsync(TCommand command);
    }
}