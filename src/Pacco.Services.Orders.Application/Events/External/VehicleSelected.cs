using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Orders.Application.Events.External
{
    [MessageNamespace("vehicles")]
    public class VehicleSelected : IEvent
    {
        public Guid OrderId { get; }
        public Guid VehicleId { get; }
        public decimal Price { get; }

        public VehicleSelected(Guid orderId, Guid vehicleId, decimal price)
        {
            OrderId = orderId;
            VehicleId = vehicleId;
            Price = price;
        }
    }
}