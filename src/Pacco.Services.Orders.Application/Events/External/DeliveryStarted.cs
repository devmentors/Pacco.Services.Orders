using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events.External
{
    public class DeliveryStarted : IEvent
    {
        public Guid OrderId { get; }

        public DeliveryStarted(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}