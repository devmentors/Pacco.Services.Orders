using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Pacco.Services.Orders.Application.Services.Clients;

namespace Pacco.Services.Orders.Infrastructure.Services.Clients
{
    public class PricingServiceClient : IPricingServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public PricingServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["pricing"];
        }

        public Task<decimal> GetTotalPriceAsync(Guid orderId)
            => _httpClient.GetAsync<decimal>($"{_url}/pricing/{orderId}");
    }
}