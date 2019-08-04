using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Commands.Handlers
{
    public class DeleteParcelFromOrderHandler : ICommandHandler<DeleteParcelFromOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly ILogger<DeleteParcelFromOrderHandler> _logger;

        public DeleteParcelFromOrderHandler(IOrderRepository orderRepository, IAppContext appContext,
            IMessageBroker messageBroker, IEventMapper eventMapper, ILogger<DeleteParcelFromOrderHandler> logger)
        {
            _orderRepository = orderRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _logger = logger;
        }

        public async Task HandleAsync(DeleteParcelFromOrder command)
        {
            var order = await _orderRepository.GetAsync(command.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != order.CustomerId && !identity.IsAdmin)
            {
                _logger.LogWarning($"Customer with id: {identity.Id} tried to delete a parcel from the order " +
                                   $"with id: {order.Id} without the proper access rights.");
                throw new UnauthorizedOrderAccessException(order.Id, identity.Id);
            }

            if (!order.DeleteParcel(command.ParcelId))
            {
                _logger.LogWarning($"Parcel with id: {command.ParcelId} was already deleted from the order" +
                                   $"with id: {order.Id}.");
                return;
            }

            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
            _logger.LogInformation($"Deleted a parcel with id: {command.ParcelId} from the order with id: {order.Id}.");
        }
    }
}