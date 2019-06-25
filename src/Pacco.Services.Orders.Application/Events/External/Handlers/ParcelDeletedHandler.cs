using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Events.External.Handlers
{
    public class ParcelDeletedHandler : IEventHandler<ParcelDeleted>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public ParcelDeletedHandler(IOrderRepository orderRepository,
            IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(ParcelDeleted command)
        {
            var order = await _orderRepository.GetContainingParcelAsync(command.Id);
            if (order is null)
            {
                return;
            }

            order.DeleteParcel(command.Id);
            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}