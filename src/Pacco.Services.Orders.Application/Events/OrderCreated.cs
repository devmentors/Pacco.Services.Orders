using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    public class OrderCreated : IEvent
    {
        public Guid Id { get; }

        public OrderCreated(Guid id)
        {
            Id = id;
        }
    }
}