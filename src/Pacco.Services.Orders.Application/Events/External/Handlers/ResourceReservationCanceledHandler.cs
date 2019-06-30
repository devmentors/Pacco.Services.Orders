using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Events.External.Handlers
{
    public class ResourceReservationCanceledHandler : IEventHandler<ResourceReservationCanceled>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public ResourceReservationCanceledHandler(IOrderRepository orderRepository, IMessageBroker messageBroker,
            IEventMapper eventMapper)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(ResourceReservationCanceled @event)
        {
            var order = await _orderRepository.GetAsync(@event.Id, @event.Date);
            if (order is null)
            {
                throw new OrderForReservedVehicleNotFoundException(@event.Id, @event.Date);
            }

            order.Cancel($"Reservation canceled at: {@event.Date}");
            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}