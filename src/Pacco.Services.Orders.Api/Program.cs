using System.Collections.Generic;
using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Orders.Application;
using Pacco.Services.Orders.Application.Commands;
using Pacco.Services.Orders.Application.DTO;
using Pacco.Services.Orders.Application.Queries;
using Pacco.Services.Orders.Infrastructure;

namespace Pacco.Services.Orders.Api 
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddOpenTracing()
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync("Welcome to Pacco Orders Service!"))
                        .Get<GetOrder, OrderDto>("orders/{id}")
                        .Get<GetOrders, IEnumerable<OrderDto>>("orders")
                        .Delete<DeleteOrder>("orders/{id}")
                        .Post<CreateOrder>("orders",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"orders/{cmd.Id}"))
                        .Post<AddParcelToOrder>("orders/{orderId}/parcels/{parcelId}")
                        .Delete<DeleteParcelFromOrder>("orders/{orderId}/parcels/{parcelId}")
                        .Post<AssignVehicleToOrder>("orders/{orderId}/vehicles/{vehicleId}")))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}