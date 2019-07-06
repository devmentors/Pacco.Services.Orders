using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Orders.Application.Events.External
{
    [MessageNamespace("availability")]
    public class ResourceReserved : IEvent
    {
        public Guid Id { get; }
        public DateTime DateTime { get; }

        public ResourceReserved(Guid id, DateTime dateTime)
        {
            Id = id;
            DateTime = dateTime;
        }
    }
}