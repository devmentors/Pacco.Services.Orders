using System.Threading.Tasks;
using Convey.CQRS.Events;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services.Clients;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Events.External.Handlers
{
    public class VehicleSelectedHandler : IEventHandler<VehicleSelected>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPricingServiceClient _pricingServiceClient;

        public VehicleSelectedHandler(IOrderRepository orderRepository, IPricingServiceClient pricingServiceClient)
        {
            _orderRepository = orderRepository;
            _pricingServiceClient = pricingServiceClient;
        }

        public async Task HandleAsync(VehicleSelected @event)
        {
            var order = await _orderRepository.GetAsync(@event.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(@event.OrderId);
            }

            order.SetVehicle(@event.VehicleId);
            var orderPrice = await _pricingServiceClient.GetOrderPriceAsync(@event.OrderId, @event.Price);
            order.SetTotalPrice(orderPrice);
            await _orderRepository.UpdateAsync(order);
        }
    }
}