using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Pacco.Services.Orders.Application.Events;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Application.Services.Clients;
using Pacco.Services.Orders.Core.Entities;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Commands.Handlers
{
    public class AssignVehicleToOrderHandler : ICommandHandler<AssignVehicleToOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPricingServiceClient _pricingServiceClient;
        private readonly IVehiclesServiceClient _vehiclesServiceClient;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;
        private readonly ILogger<AssignVehicleToOrderHandler> _logger;

        public AssignVehicleToOrderHandler(IOrderRepository orderRepository,
            IPricingServiceClient pricingServiceClient, IVehiclesServiceClient vehiclesServiceClient,
            IMessageBroker messageBroker, IAppContext appContext, ILogger<AssignVehicleToOrderHandler> logger)
        {
            _orderRepository = orderRepository;
            _pricingServiceClient = pricingServiceClient;
            _vehiclesServiceClient = vehiclesServiceClient;
            _messageBroker = messageBroker;
            _appContext = appContext;
            _logger = logger;
        }

        public async Task HandleAsync(AssignVehicleToOrder command)
        {
            var order = await _orderRepository.GetAsync(command.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != order.CustomerId && !identity.IsAdmin)
            {
                _logger.LogWarning($"Customer with id: {identity.Id} tried to assign a vehicle to the order " +
                                   $"with id: {order.Id} without the proper access rights.");
                throw new UnauthorizedOrderAccessException(command.OrderId, identity.Id);
            }

            if (!order.HasParcels)
            {
                throw new OrderHasNoParcelsException(command.OrderId);
            }

            if (order.Status != OrderStatus.New)
            {
                return;
            }

            var vehicle = await _vehiclesServiceClient.GetAsync(command.VehicleId);
            if (vehicle is null)
            {
                throw new VehicleNotFoundException(command.VehicleId);
            }

            _logger.LogInformation($"Fetching an order pricing for customer: {order.CustomerId} and " +
                                   $"service price: {vehicle.PricePerService}");
            var pricing = await _pricingServiceClient.GetOrderPriceAsync(order.CustomerId, vehicle.PricePerService);
            _logger.LogInformation($"Received an order pricing: {pricing.OrderDiscountPrice}");
            order.SetVehicle(command.VehicleId);
            order.SetTotalPrice(pricing.OrderDiscountPrice);
            order.SetDeliveryDate(command.DeliveryDate);
            await _orderRepository.UpdateAsync(order);
            await _messageBroker.PublishAsync(new VehicleAssignedToOrder(command.OrderId, command.VehicleId));
            _logger.LogInformation($"Assigned a vehicle with id: {command.VehicleId} to the order with id: {order.Id}.");
        }
    }
}