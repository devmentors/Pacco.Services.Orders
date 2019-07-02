using System.Threading.Tasks;
using Convey.CQRS.Commands;
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

        public AssignVehicleToOrderHandler(IOrderRepository orderRepository,
            IPricingServiceClient pricingServiceClient, IVehiclesServiceClient vehiclesServiceClient,
            IMessageBroker messageBroker)
        {
            _orderRepository = orderRepository;
            _pricingServiceClient = pricingServiceClient;
            _vehiclesServiceClient = vehiclesServiceClient;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(AssignVehicleToOrder command)
        {
            var order = await _orderRepository.GetAsync(command.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
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

            var orderPrice = await _pricingServiceClient.GetOrderPriceAsync(order.CustomerId, vehicle.PricePerService);
            order.SetVehicle(command.VehicleId);
            order.SetTotalPrice(orderPrice);
            await _orderRepository.UpdateAsync(order);
            await _messageBroker.PublishAsync(new VehicleAssignedToOrder(command.OrderId, command.VehicleId));
        }
    }
}