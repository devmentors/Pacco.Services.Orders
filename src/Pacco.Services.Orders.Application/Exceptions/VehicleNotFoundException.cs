using System;
using Pacco.Services.Orders.Core.Exceptions;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class VehicleNotFoundException : ExceptionBase
    {
        public override string Code => "vehicle_not_found";
        public Guid Id { get; }

        public VehicleNotFoundException(Guid id) : base($"Vehicle with id: {id} was not found.")
        {
            Id = id;
        }
    }
}