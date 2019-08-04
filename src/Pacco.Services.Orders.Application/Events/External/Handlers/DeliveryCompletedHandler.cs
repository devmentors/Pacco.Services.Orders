using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Events.External.Handlers
{
    public class DeliveryCompletedHandler : IEventHandler<DeliveryCompleted>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly ILogger<DeliveryCompletedHandler> _logger;

        public DeliveryCompletedHandler(IOrderRepository orderRepository, IMessageBroker messageBroker,
            IEventMapper eventMapper, ILogger<DeliveryCompletedHandler> logger)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _logger = logger;
        }

        public async Task HandleAsync(DeliveryCompleted @event)
        {
            var order = await _orderRepository.GetAsync(@event.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(@event.OrderId);
            }

            order.Complete();
            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
            _logger.LogInformation($"Delivery with id: {@event.DeliveryId} for the order with id: " +
                                   $"{@event.OrderId} has been completed.");
        }
    }
}