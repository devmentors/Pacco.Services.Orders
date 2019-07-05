using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Orders.Application.Commands
{
    [Contract]
    public class DeleteOrder : ICommand
    {
        public Guid Id { get; }
        public Guid? CustomerId { get; }

        public DeleteOrder(Guid id, Guid? customerId)
        {
            Id = id;
            CustomerId = customerId;
        }
    }
}