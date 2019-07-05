using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Commands.Handlers
{
    public class DeleteParcelFromOrderHandler : ICommandHandler<DeleteParcelFromOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public DeleteParcelFromOrderHandler(IOrderRepository orderRepository,
            IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(DeleteParcelFromOrder command)
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

            if (!order.DeleteParcel(command.ParcelId))
            {
                return;
            }

            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}