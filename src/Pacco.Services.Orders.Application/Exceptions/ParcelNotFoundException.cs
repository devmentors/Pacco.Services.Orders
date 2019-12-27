using System;
using Pacco.Services.Orders.Core.Exceptions;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class ParcelNotFoundException : ExceptionBase
    {
        public override string Code => "parcel_not_found";
        public Guid ParcelId { get; }

        public ParcelNotFoundException(Guid parcelId) : base($"Parcel with id: {parcelId} was not found.")
        {
            ParcelId = parcelId;
        }
    }
}