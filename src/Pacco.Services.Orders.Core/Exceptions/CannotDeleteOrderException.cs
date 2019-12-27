using System;

namespace Pacco.Services.Orders.Core.Exceptions
{
    public class CannotDeleteOrderException : ExceptionBase
    {
        public override string Code => "cannot_delete_order";
        public Guid OrderId { get; }

        public CannotDeleteOrderException(Guid orderId) : base($"Order with id: '{orderId}' cannot be deleted.")
        {
            OrderId = orderId;
        }
    }
}