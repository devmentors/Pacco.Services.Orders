using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Orders.Application.Events.External
{
    [MessageNamespace("deliveries")]
    public class DeliveryFailed : IEvent
    {
        public Guid DeliveryId { get; }
        public Guid OrderId { get; }
        public string Reason { get; }

        public DeliveryFailed(Guid deliveryId, Guid orderId, string reason)
        {
            DeliveryId = deliveryId;
            OrderId = orderId;
            Reason = reason;
        }
    }
}