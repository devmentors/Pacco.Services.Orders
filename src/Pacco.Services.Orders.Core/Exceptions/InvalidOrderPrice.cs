using System;

namespace Pacco.Services.Orders.Core.Exceptions
{
    public class InvalidOrderPrice : ExceptionBase
    {
        public override string Code => "invalid_order_price";

        public InvalidOrderPrice(Guid id, decimal price) : base($"Order with id: '{id}' has an invalid price: {price}")
        {
        }
    }
}