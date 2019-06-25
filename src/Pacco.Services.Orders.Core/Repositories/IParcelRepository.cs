using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pacco.Services.Orders.Core.Entities;

namespace Pacco.Services.Orders.Core.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetAsync(Guid id);
        Task<Order> GetContainingParcelAsync(Guid parcelId);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);
    }
}