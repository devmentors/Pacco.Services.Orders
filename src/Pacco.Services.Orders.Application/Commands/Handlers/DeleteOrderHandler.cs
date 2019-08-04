using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Pacco.Services.Orders.Application.Events;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Exceptions;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Commands.Handlers
{
    public class DeleteOrderHandler : ICommandHandler<DeleteOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<DeleteOrderHandler> _logger;

        public DeleteOrderHandler(IOrderRepository orderRepository, IAppContext appContext,
            IMessageBroker messageBroker, ILogger<DeleteOrderHandler> logger)
        {
            _orderRepository = orderRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
            _logger = logger;
        }

        public async Task HandleAsync(DeleteOrder command)
        {
            var order = await _orderRepository.GetAsync(command.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }
       
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != order.CustomerId && !identity.IsAdmin)
            {
                _logger.LogWarning($"Customer with id: {identity.Id} tried to delete the order " +
                                   $"with id: {order.Id} without the proper access rights.");
                throw new UnauthorizedOrderAccessException(command.OrderId, identity.Id);
            }

            if (!order.CanBeDeleted)
            {
                throw new CannotDeleteOrderException(command.OrderId);
            }

            await _orderRepository.DeleteAsync(command.OrderId);
            await _messageBroker.PublishAsync(new OrderDeleted(command.OrderId));
            _logger.LogInformation($"Order with id: {order.Id} has been deleted.");
        }
    }
}