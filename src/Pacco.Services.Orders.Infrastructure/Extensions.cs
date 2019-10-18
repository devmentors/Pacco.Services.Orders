using System;
using System.Linq;
using Convey;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Pacco.Services.Orders.Application;
using Pacco.Services.Orders.Application.Commands;
using Pacco.Services.Orders.Application.Events.External;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Application.Services.Clients;
using Pacco.Services.Orders.Core.Repositories;
using Pacco.Services.Orders.Infrastructure.Contexts;
using Pacco.Services.Orders.Infrastructure.Exceptions;
using Pacco.Services.Orders.Infrastructure.Logging;
using Pacco.Services.Orders.Infrastructure.Mongo.Documents;
using Pacco.Services.Orders.Infrastructure.Mongo.Repositories;
using Pacco.Services.Orders.Infrastructure.Services;
using Pacco.Services.Orders.Infrastructure.Services.Clients;

namespace Pacco.Services.Orders.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<ICustomerRepository, CustomerMongoRepository>();
            builder.Services.AddTransient<IOrderRepository, OrderMongoRepository>();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddTransient<IParcelsServiceClient, ParcelsServiceClient>();
            builder.Services.AddTransient<IPricingServiceClient, PricingServiceClient>();
            builder.Services.AddTransient<IVehiclesServiceClient, VehiclesServiceClient>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());

            return builder
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddHttpClient()
                .AddConsul()
                .AddFabio()
                .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
                .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                .AddMongo()
                .AddMetrics()
                .AddJaeger()
                .AddHandlersLogging()
                .AddMongoRepository<CustomerDocument, Guid>("Customers")
                .AddMongoRepository<OrderDocument, Guid>("Orders");
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseErrorHandler()
                .UseJaeger()
                .UseInitializers()
                .UsePublicContracts<ContractAttribute>()
                .UseConsul()
                .UseMetrics()
                .UseRabbitMq()
                .SubscribeCommand<ApproveOrder>()
                .SubscribeCommand<CreateOrder>()
                .SubscribeCommand<CancelOrder>()
                .SubscribeCommand<DeleteOrder>()
                .SubscribeCommand<AddParcelToOrder>()
                .SubscribeCommand<DeleteParcelFromOrder>()
                .SubscribeCommand<AssignVehicleToOrder>()
                .SubscribeEvent<CustomerCreated>()
                .SubscribeEvent<DeliveryCompleted>()
                .SubscribeEvent<DeliveryFailed>()
                .SubscribeEvent<DeliveryStarted>()
                .SubscribeEvent<ParcelDeleted>()
                .SubscribeEvent<ResourceReserved>()
                .SubscribeEvent<ResourceReservationCanceled>();

            return app;
        }

        internal static CorrelationContext GetCorrelationContext(this IHttpContextAccessor accessor)
            => accessor.HttpContext.Request.Headers.TryGetValue("Correlation-Context", out var json)
                ? JsonConvert.DeserializeObject<CorrelationContext>(json.FirstOrDefault())
                : null;
    }
}