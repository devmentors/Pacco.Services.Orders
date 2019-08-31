using System;
using System.Collections.Generic;
using Convey.Logging.CQRS;
using Pacco.Services.Orders.Application.Commands;

namespace Pacco.Services.Orders.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(AddParcelToOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Added a parcel with id: {ParcelId} to the order with id: {OrderId}."
                    }
                },
                {
                    typeof(ApproveOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Order with id: {OrderId} has been approved."
                    }
                },
                {
                    typeof(AssignVehicleToOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Assigned a vehicle with id: {VehicleId} to the order with id: {OrderId}."
                    }
                },
                {
                    typeof(CancelOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Order with id: {OrderId} has been canceled."
                    }
                },
                {
                    typeof(CreateOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Order with id: {OrderId} has been created."
                    }
                },
                {
                    typeof(DeleteOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Order with id: {OrderId} has been deleted."
                    }
                },
                {
                    typeof(DeleteParcelFromOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Deleted a parcel with id: {ParcelId} from the order with id: {OrderId}."
                    }
                },
            };
        
        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var key = message.GetType();
            return MessageTemplates.TryGetValue(key, out var template) ? template : null;
        }
    }
}