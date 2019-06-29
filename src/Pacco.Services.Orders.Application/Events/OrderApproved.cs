using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    [Contract]
    public class OrderApproved : IEvent
    {
        public Guid Id { get; }

        public OrderApproved(Guid id)
        {
            Id = id;
        }
    }
}