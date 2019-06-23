using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Orders.Application.Commands
{
    public class CancelOrder : ICommand
    {
        public Guid Id { get; }

        public CancelOrder(Guid id)
        {
            Id = id;
        }
    }
}