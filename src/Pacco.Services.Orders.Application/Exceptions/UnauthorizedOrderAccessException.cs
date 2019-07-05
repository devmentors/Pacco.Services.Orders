using System;
using Pacco.Services.Orders.Core.Exceptions;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class UnauthorizedOrderAccessException : ExceptionBase
    {
        public override string Code => "unauthorized_order_access";

        public UnauthorizedOrderAccessException(Guid id, Guid customerId) 
            : base($"Unauthorized access to order: '{id}' by customer: '{customerId}'")
        {
        }
    }
}