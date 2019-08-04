using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Events.External.Handlers
{
    public class DeliveryStartedHandler : IEventHandler<DeliveryStarted>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly ILogger<DeliveryStartedHandler> _logger;

        public DeliveryStartedHandler(IOrderRepository orderRepository, IMessageBroker messageBroker,
            IEventMapper eventMapper, ILogger<DeliveryStartedHandler> logger)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _logger = logger;
        }

        public async Task HandleAsync(DeliveryStarted @event)
        {
            var order = await _orderRepository.GetAsync(@event.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(@event.OrderId);
            }

            order.SetDelivering();
            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
            _logger.LogInformation($"Delivery for the order with id: {@event.OrderId} has started");
        }
    }
}