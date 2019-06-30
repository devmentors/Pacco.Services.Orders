using System;
using Pacco.Services.Orders.Core.Exceptions;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class ParcelAlreadyAddedToOrderException : ExceptionBase
    {
        public override string Code => "parcel_already_added_to_order";
        public Guid OrderId { get; }
        public Guid ParcelId { get; }

        public ParcelAlreadyAddedToOrderException(Guid orderId, Guid parcelId)
            : base($"Parcel with id: {parcelId} was already added to order: {orderId}.")
        {
            OrderId = orderId;
            ParcelId = parcelId;
        }
    }
}