using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Orders.Application.Commands
{
    public class CreateOrder : ICommand
    {
        public Guid Id { get; }
        public Guid CustomerId { get; }

        public CreateOrder(Guid id, Guid customerId)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            CustomerId = customerId;
        }
    }
}