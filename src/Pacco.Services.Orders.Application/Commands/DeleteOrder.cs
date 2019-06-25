using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Orders.Application.Commands
{
    public class DeleteOrder : ICommand
    {
        public Guid Id { get; }

        public DeleteOrder(Guid id)
        {
            Id = id;
        }
    }
}