using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Orders.Application.Events.External
{
    [MessageNamespace("deliveries")]
    public class DeliveryCompleted : IEvent
    {
        public Guid OrderId { get; }

        public DeliveryCompleted(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}