using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    [Contract]
    public class OrderDeleted : IEvent
    {
        public Guid Id { get; }

        public OrderDeleted(Guid id)
        {
            Id = id;
        }
    }
}