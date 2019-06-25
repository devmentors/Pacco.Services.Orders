using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    public class OrderDelivering : IEvent
    {
        public Guid Id { get; }

        public OrderDelivering(Guid id)
        {
            Id = id;
        }
    }
}