using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using Pacco.Services.Orders.Core.Entities;
using Pacco.Services.Orders.Core.Repositories;
namespace Pacco.Services.Orders.Application.Events.External.Handlers
{
    public class CustomerCreatedHandler : IEventHandler<CustomerCreated>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerCreatedHandler> _logger;

        public CustomerCreatedHandler(ICustomerRepository customerRepository, ILogger<CustomerCreatedHandler> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task HandleAsync(CustomerCreated @event)
        {
            if (await _customerRepository.ExistsAsync(@event.CustomerId))
            {
                _logger.LogWarning($"Customer with id: {@event.CustomerId} was already added.");
                return;
            }

            await _customerRepository.AddAsync(new Customer(@event.CustomerId));
            _logger.LogInformation($"Added a customer with id: {@event.CustomerId}");
        }
    }
}