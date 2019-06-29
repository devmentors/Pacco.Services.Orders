using System;

namespace Pacco.Services.Orders.Core.Exceptions
{
    public class CannotChangeOrderPriceException : ExceptionBase
    {
        public override string Code => "cannot_change_order_price";

        public CannotChangeOrderPriceException(Guid id) : base($"Order with id: '{id}' cannot have a changed price.")
        {
        }
    }
}