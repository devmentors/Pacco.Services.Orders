using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Events.External.Handlers
{
    public class ParcelDeletedHandler : IEventHandler<ParcelDeleted>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly ILogger<ParcelDeletedHandler> _logger;

        public ParcelDeletedHandler(IOrderRepository orderRepository, IMessageBroker messageBroker,
            IEventMapper eventMapper, ILogger<ParcelDeletedHandler> logger)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _logger = logger;
        }

        public async Task HandleAsync(ParcelDeleted @event)
        {
            var order = await _orderRepository.GetContainingParcelAsync(@event.ParcelId);
            if (order is null)
            {
                return;
            }

            order.DeleteParcel(@event.ParcelId);
            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
            _logger.LogInformation($"Parcel with id: {@event.ParcelId} has been deleted from the order " +
                                   $"with id: {order.Id}");
        }
    }
}