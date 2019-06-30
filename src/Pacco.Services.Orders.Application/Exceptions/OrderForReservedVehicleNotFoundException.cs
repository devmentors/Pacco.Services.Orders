using System;
using Pacco.Services.Orders.Core.Exceptions;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class OrderForReservedVehicleNotFoundException : ExceptionBase
    {
        public override string Code => "order_for_reserved_vehicle_not_found";
        public Guid VehicleId { get; }
        public DateTime Date { get; }

        public OrderForReservedVehicleNotFoundException(Guid vehicleId, DateTime date) : base(
            $"Order for reserved vehicle: {vehicleId} for date: {date} was not found.")
        {
            VehicleId = vehicleId;
            Date = date;
        }
    }
}