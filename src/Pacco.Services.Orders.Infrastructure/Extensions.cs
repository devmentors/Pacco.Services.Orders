using System;
using Convey;
using Convey.CQRS.Queries;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Orders.Application.Commands;
using Pacco.Services.Orders.Application.Events;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Core.Repositories;
using Pacco.Services.Orders.Infrastructure.Mongo.Documents;
using Pacco.Services.Orders.Infrastructure.Mongo.Repositories;
using Pacco.Services.Orders.Infrastructure.Services;

namespace Pacco.Services.Orders.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IOrderRepository, OrderMongoRepository>();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            return builder
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddRabbitMq()
                .AddMongo()
                .AddMongoRepository<OrderDocument, Guid>("Orders");
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseInitializers().UseRabbitMq()
                .SubscribeCommand<CreateOrder>()
                .SubscribeCommand<CancelOrder>()
                .SubscribeEvent<ParcelAdded>();

            return app;
        }
    }
}