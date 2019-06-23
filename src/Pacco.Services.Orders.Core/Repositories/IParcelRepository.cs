using System;
using System.Threading.Tasks;
using Pacco.Services.Orders.Core.Entities;

namespace Pacco.Services.Orders.Core.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task DeleteAsync(Guid id);
    }
}