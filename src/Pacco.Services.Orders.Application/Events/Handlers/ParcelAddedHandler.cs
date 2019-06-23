using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events.Handlers
{
    public class ParcelAddedHandler : IEventHandler<ParcelAdded>
    {
        public async Task HandleAsync(ParcelAdded @event)
        {
            await Task.CompletedTask;
        }
    }
}