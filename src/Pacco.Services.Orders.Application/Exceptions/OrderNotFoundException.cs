using System;
using Pacco.Services.Orders.Core.Exceptions;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class OrderNotFoundException : ExceptionBase
    {
        public override string Code => "order_not_found";
        
        public OrderNotFoundException(Guid id) : base($"Order with id: {id} was not found.")
        {
        }
    }
}