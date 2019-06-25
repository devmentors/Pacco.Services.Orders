using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events.External
{
    public class DeliveryCompleted : IEvent
    {
        public Guid OrderId { get; }

        public DeliveryCompleted(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}