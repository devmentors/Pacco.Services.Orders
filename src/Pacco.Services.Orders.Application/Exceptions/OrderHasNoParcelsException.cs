using System;
using Pacco.Services.Orders.Core.Exceptions;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class OrderHasNoParcelsException : ExceptionBase
    {
        public override string Code => "order_has_no_parcels";
        public Guid Id { get; }
        
        public OrderHasNoParcelsException(Guid id) : base($"Order with id: {id} has no parcels.")
        {
            Id = id;
        }
    }
}