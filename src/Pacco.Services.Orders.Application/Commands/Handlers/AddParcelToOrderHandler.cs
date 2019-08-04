using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Application.Services.Clients;
using Pacco.Services.Orders.Core.Entities;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Commands.Handlers
{
    public class AddParcelToOrderHandler : ICommandHandler<AddParcelToOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IParcelsServiceClient _parcelsServiceClient;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly ILogger<AddParcelToOrderHandler> _logger;

        public AddParcelToOrderHandler(IOrderRepository orderRepository, IParcelsServiceClient parcelsServiceClient,
            IAppContext appContext, IMessageBroker messageBroker, IEventMapper eventMapper,
            ILogger<AddParcelToOrderHandler> logger)
        {
            _orderRepository = orderRepository;
            _parcelsServiceClient = parcelsServiceClient;
            _appContext = appContext;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _logger = logger;
        }

        public async Task HandleAsync(AddParcelToOrder command)
        {
            var order = await _orderRepository.GetContainingParcelAsync(command.ParcelId);
            if (!(order is null))
            {
                ValidateAccessOrFail(order);
                throw new ParcelAlreadyAddedToOrderException(command.OrderId, command.ParcelId);
            }

            order = await _orderRepository.GetAsync(command.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }

            ValidateAccessOrFail(order);
            var parcel = await _parcelsServiceClient.GetAsync(command.ParcelId);
            if (parcel is null)
            {
                throw new ParcelNotFoundException(command.ParcelId);
            }

            if (!order.AddParcel(new Parcel(parcel.Id, parcel.Name, parcel.Variant, parcel.Size)))
            {
                _logger.LogWarning($"Parcel with id: {parcel.Id} was already added to the order with id: {order.Id}.");
                return;
            }

            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
            _logger.LogInformation($"Added a parcel with id: {parcel.Id} to the order with id: {order.Id}.");
        }

        private void ValidateAccessOrFail(Order order)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != order.CustomerId && !identity.IsAdmin)
            {
                _logger.LogWarning($"Customer with id: {identity.Id} tried to add parcel to the order " +
                                   $"with id: {order.Id} without the proper access rights.");
                throw new UnauthorizedOrderAccessException(order.Id, identity.Id);
            }
        }
    }
}