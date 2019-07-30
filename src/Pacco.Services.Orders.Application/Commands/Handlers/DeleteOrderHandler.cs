using System.Threading.Tasks;
using Convey.CQRS.Commands;
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
        private readonly IMessageBroker _messageBroker;

        public DeleteOrderHandler(IOrderRepository orderRepository, IMessageBroker messageBroker)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeleteOrder command)
        {
            var order = await _orderRepository.GetAsync(command.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }
            
            if (command.CustomerId.HasValue && command.CustomerId != order.CustomerId)
            {
                throw new UnauthorizedOrderAccessException(command.OrderId, command.CustomerId.Value);
            }

            if (!order.CanBeDeleted)
            {
                throw new CannotDeleteOrderException(command.OrderId);
            }

            await _orderRepository.DeleteAsync(command.OrderId);
            await _messageBroker.PublishAsync(new OrderDeleted(command.OrderId));
        }
    }
}