using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Orders.Application.Clients;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Entities;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Commands.Handlers
{
    public class AddParcelToOrderHandler : ICommandHandler<AddParcelToOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IParcelsServiceClient _parcelsServiceClient;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public AddParcelToOrderHandler(IOrderRepository orderRepository,
            IParcelsServiceClient parcelsServiceClient, IMessageBroker messageBroker,
            IEventMapper eventMapper)
        {
            _orderRepository = orderRepository;
            _parcelsServiceClient = parcelsServiceClient;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(AddParcelToOrder command)
        {
            var order = await _orderRepository.GetAsync(command.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }

            var parcel = await _parcelsServiceClient.GetAsync(command.ParcelId);
            if (parcel is null)
            {
                throw new ParcelNotFoundException(command.ParcelId);
            }

            order.AddParcel(new Parcel(parcel.Id, parcel.Name, parcel.Variant, parcel.Size));
            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}