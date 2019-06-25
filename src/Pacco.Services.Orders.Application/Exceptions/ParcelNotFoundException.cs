using System;
using Pacco.Services.Orders.Core.Exceptions;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class ParcelNotFoundException : ExceptionBase
    {
        public override string Code => "parcel_not_found";
        
        public ParcelNotFoundException(Guid orderId) : base($"Parcel with id: {orderId} was not found.")
        {
        }
    }
}