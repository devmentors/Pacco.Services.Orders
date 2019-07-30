using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Orders.Application.Commands
{
    [Contract]
    public class DeleteOrder : ICommand
    {
        public Guid OrderId { get; }
        public Guid? CustomerId { get; }

        public DeleteOrder(Guid orderId, Guid? customerId)
        {
            OrderId = orderId;
            CustomerId = customerId;
        }
    }
}