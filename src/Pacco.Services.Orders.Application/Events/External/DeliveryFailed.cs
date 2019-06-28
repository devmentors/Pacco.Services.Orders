using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Orders.Application.Events.External
{
    [MessageNamespace("deliveries")]
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