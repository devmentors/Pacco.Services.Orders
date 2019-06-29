using System;
using System.Threading.Tasks;

namespace Pacco.Services.Orders.Application.Services.Clients
{
    public interface IPricingServiceClient
    {
        Task<decimal> GetOrderPriceAsync(Guid customerId, decimal orderPrice);
    }
}