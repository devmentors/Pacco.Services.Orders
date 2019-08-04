using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Events.External.Handlers
{
    public class DeliveryFailedHandler : IEventHandler<DeliveryFailed>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly ILogger<DeliveryFailedHandler> _logger;

        public DeliveryFailedHandler(IOrderRepository orderRepository, IMessageBroker messageBroker,
            IEventMapper eventMapper, ILogger<DeliveryFailedHandler> logger)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _logger = logger;
        }

        public async Task HandleAsync(DeliveryFailed @event)
        {
            var order = await _orderRepository.GetAsync(@event.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(@event.OrderId);
            }

            order.Cancel(@event.Reason);
            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
            _logger.LogInformation($"Order with id: {@event.OrderId} has been canceled due to the " +
                                   $"failed delivery, reason: {@event.Reason}");
        }
    }
}