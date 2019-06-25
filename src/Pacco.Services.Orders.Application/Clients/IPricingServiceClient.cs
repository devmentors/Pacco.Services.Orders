using System;
using System.Threading.Tasks;

namespace Pacco.Services.Orders.Application.Clients
{
    public interface IPricingServiceClient
    {
        Task<decimal> GetTotalPriceAsync(Guid orderId);
    }
}