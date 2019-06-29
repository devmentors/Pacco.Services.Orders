using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Orders.Application.Commands
{
    public class AssignVehicleToOrder : ICommand
    {
        public Guid OrderId { get; }
        public Guid VehicleId { get; }

        public AssignVehicleToOrder(Guid orderId, Guid vehicleId)
        {
            OrderId = orderId;
            VehicleId = vehicleId;
        }
    }
}