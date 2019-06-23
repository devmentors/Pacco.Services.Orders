using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Pacco.Services.Orders.Core.Entities;
using Pacco.Services.Orders.Core.Repositories;
using Pacco.Services.Orders.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Orders.Infrastructure.Mongo.Repositories
{
    public class OrderMongoRepository : IOrderRepository
    {
        private readonly IMongoRepository<OrderDocument, Guid> _repository;

        public OrderMongoRepository(IMongoRepository<OrderDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task AddAsync(Order order) => _repository.AddAsync(order.AsDocument());
        
        public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);
    }
}