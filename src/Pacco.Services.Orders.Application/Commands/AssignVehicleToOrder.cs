using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Orders.Application.Commands
{
    public class AssignVehicleToOrder : ICommand
    {
        public Guid OrderId { get; }
        public Guid VehicleId { get; }
        public DateTime DeliveryDate { get; }
        public Guid? CustomerId { get; }

        public AssignVehicleToOrder(Guid orderId, Guid vehicleId, DateTime deliveryDate, Guid? customerId)
        {
            OrderId = orderId;
            VehicleId = vehicleId;
            DeliveryDate = deliveryDate;
            CustomerId = customerId;
        }
    }
}