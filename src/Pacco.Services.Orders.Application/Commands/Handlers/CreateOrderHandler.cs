using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Entities;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Commands.Handlers
{
    public class CreateOrderHandler : ICommandHandler<CreateOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateOrderHandler(IOrderRepository orderRepository, IMessageBroker messageBroker,
            IEventMapper eventMapper, IDateTimeProvider dateTimeProvider)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task HandleAsync(CreateOrder command)
        {
            var order = new Order(command.Id, command.CustomerId, OrderStatus.New, _dateTimeProvider.Now);
            await _orderRepository.AddAsync(order);
        }
    }
}