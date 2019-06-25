using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events.External
{
    public class ParcelDeleted : IEvent
    {
        public Guid Id { get; }

        public ParcelDeleted(Guid id)
        {
            Id = id;
        }
    }
}