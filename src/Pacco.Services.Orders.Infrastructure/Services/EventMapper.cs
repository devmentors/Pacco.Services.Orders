using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Events;
using Pacco.Services.Orders.Application.Events;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core;

namespace Pacco.Services.Orders.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);

        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                case Core.Events.OrderCreated e:
                    return new OrderCreated(e.Order.Id);
            }

            return null;
        }
    }
}