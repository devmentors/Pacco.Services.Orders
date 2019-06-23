using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    public class OrderCanceled : IEvent
    {
        public Guid Id { get; }
        
        public OrderCanceled(Guid id)
        {
            Id = id;
        }
    }
}