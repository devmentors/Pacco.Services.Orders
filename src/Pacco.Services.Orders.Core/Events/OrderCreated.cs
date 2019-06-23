using Pacco.Services.Orders.Core.Entities;

namespace Pacco.Services.Orders.Core.Events
{
    public class OrderCreated : IDomainEvent
    {
        public Order Order { get; }

        public OrderCreated(Order order)
        {
            Order = order;
        }
    }
}