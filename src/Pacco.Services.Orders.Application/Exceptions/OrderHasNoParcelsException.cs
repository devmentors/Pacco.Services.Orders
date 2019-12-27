using System;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class OrderHasNoParcelsException : AppException
    {
        public override string Code => "order_has_no_parcels";
        public Guid Id { get; }
        
        public OrderHasNoParcelsException(Guid id) : base($"Order with id: {id} has no parcels.")
        {
            Id = id;
        }
    }
}