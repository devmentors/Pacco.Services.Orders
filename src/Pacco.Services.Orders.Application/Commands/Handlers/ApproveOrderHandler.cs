using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Application.Services.Clients;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Commands.Handlers
{
    public class ApproveOrderHandler : ICommandHandler<ApproveOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPricingServiceClient _pricingServiceClient;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public ApproveOrderHandler(IOrderRepository orderRepository,
            IPricingServiceClient pricingServiceClient,
            IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _orderRepository = orderRepository;
            _pricingServiceClient = pricingServiceClient;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(ApproveOrder command)
        {
            var order = await _orderRepository.GetAsync(command.Id);
            if (order is null)
            {
                throw new OrderNotFoundException(command.Id);
            }

            var totalPrice = await _pricingServiceClient.GetTotalPriceAsync(command.Id);
            order.Approve(totalPrice);
            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}