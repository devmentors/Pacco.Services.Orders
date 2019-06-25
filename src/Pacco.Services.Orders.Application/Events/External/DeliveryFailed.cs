using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events.External
{
    public class DeliveryFailed : IEvent
    {
        public Guid OrderId { get; }
        public string Reason { get; }

        public DeliveryFailed(Guid orderId, string reason)
        {
            OrderId = orderId;
            Reason = reason;
        }
    }
}