using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Orders.Application.Events.External
{
    [MessageNamespace("deliveries")]
    public class ParcelDeleted : IEvent
    {
        public Guid Id { get; }

        public ParcelDeleted(Guid id)
        {
            Id = id;
        }
    }
}