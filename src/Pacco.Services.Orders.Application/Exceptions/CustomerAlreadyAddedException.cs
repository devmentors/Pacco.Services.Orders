using System;
using Pacco.Services.Orders.Core.Exceptions;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class CustomerAlreadyAddedException : ExceptionBase
    {
        public override string Code => "customer_already_added";
        public Guid CustomerId { get; }

        public CustomerAlreadyAddedException(Guid customerId)
            : base($"Customer with id: {customerId} was already added.")
        {
            CustomerId = customerId;
        }
    }
}