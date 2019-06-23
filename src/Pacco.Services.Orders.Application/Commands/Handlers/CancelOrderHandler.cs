using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Orders.Application.Events;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Commands.Handlers
{
    public class CancelOrderHandler : ICommandHandler<CancelOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _messageBroker;

        public CancelOrderHandler(IOrderRepository orderRepository, IMessageBroker messageBroker)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CancelOrder command)
        {
            await _orderRepository.DeleteAsync(command.Id);
            await _messageBroker.PublishAsync(new OrderCanceled(command.Id));
        }
    }
}