using System;
using Pacco.Services.Orders.Core.Entities;

namespace Pacco.Services.Orders.Core.Exceptions
{
    public class CannotChangeOrderStateException : ExceptionBase
    {
        public override string Code => "cannot_change_order_state";

        public CannotChangeOrderStateException(Guid id, OrderStatus currentStatus, OrderStatus nextStatus) :
            base($"Cannot change state for order with id: '{id}' from {currentStatus} to {nextStatus}'")
        {
        }
    }
}