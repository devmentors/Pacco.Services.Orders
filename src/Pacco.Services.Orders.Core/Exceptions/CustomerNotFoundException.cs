using System;

namespace Pacco.Services.Orders.Core.Exceptions
{
    public class CustomerNotFoundException : ExceptionBase
    {
        public override string Code => "customer_not_found";
        public Guid CustomerId { get; }

        public CustomerNotFoundException(Guid customerId) : base($"Customer with id: {customerId} was not found.")
        {
            CustomerId = customerId;
        }
    }
}