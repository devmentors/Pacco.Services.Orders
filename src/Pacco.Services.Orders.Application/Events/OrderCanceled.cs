using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    [Contract]
    public class OrderCanceled : IEvent
    {
        public Guid Id { get; }
        public string Reason { get; }

        public OrderCanceled(Guid id, string reason)
        {
            Id = id;
            Reason = reason;
        }
    }
}