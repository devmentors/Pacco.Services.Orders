using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Orders.Application.Events.External
{
    [MessageNamespace("availability")]
    public class ResourceReservationCanceled : IEvent
    {
        public Guid Id { get; }
        public DateTime Date { get; }

        public ResourceReservationCanceled(Guid id, DateTime date)
        {
            Id = id;
            Date = date;
        }
    }
}