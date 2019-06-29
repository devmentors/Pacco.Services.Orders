using System.Threading.Tasks;
using Convey.CQRS.Events;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Events.External.Handlers
{
    public class ResourceReservedHandler : IEventHandler<ResourceReserved>
    {
        private readonly IOrderRepository _orderRepository;

        public ResourceReservedHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(ResourceReserved @event)
        {
        }
    }
}