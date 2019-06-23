using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    public class ParcelAdded : IEvent
    {
        public Guid Id { get; }

        public ParcelAdded(Guid id)
        {
            Id = id;
        }
    }
}