using System;
using Convey.MessageBrokers.RabbitMQ;
using Pacco.Services.Orders.Application.Commands;
using Pacco.Services.Orders.Application.Events.External;
using Pacco.Services.Orders.Application.Events.Rejected;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Core.Exceptions;

namespace Pacco.Services.Orders.Infrastructure.Exceptions
{
    public class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
        {
            switch (exception)
            {
                case CannotDeleteOrderException ex: return new DeleteOrderRejected(ex.Id, ex.Message, ex.Code);

                case CustomerNotFoundException ex: return new CreateOrderRejected(ex.Id, ex.Message, ex.Code);

                case OrderForReservedVehicleNotFoundException ex:
                    return new OrderForReservedVehicleNotFound(ex.VehicleId, ex.Date, ex.Message, ex.Code);

                case OrderNotFoundException ex:
                    switch (message)
                    {
                        case AddParcelToOrder m:
                            return new AddParcelToOrderRejected(m.OrderId, m.ParcelId, ex.Message, ex.Code);
                        case ApproveOrder m: return new ApproveOrderRejected(m.Id, ex.Message, ex.Code);
                        case AssignVehicleToOrder m:
                            return new AssignVehicleToOrderRejected(m.OrderId, m.VehicleId, ex.Message, ex.Code);
                        case CancelOrder m: return new CancelOrderRejected(m.Id, ex.Message, ex.Code);
                        case DeleteOrder m: return new DeleteOrderRejected(m.Id, ex.Message, ex.Code);
                        case DeleteParcelFromOrder m:
                            return new DeleteParcelFromOrderRejected(m.OrderId, m.ParcelId, ex.Message, ex.Code);
                        case DeliveryCompleted _: return new OrderForDeliveryNotFound(ex.Id, ex.Message, ex.Code);
                        case DeliveryFailed _: return new OrderForDeliveryNotFound(ex.Id, ex.Message, ex.Code);
                        case DeliveryStarted _: return new OrderForDeliveryNotFound(ex.Id, ex.Message, ex.Code);
                    }

                    break;

                case OrderHasNoParcelsException ex:
                    switch (message)
                    {
                        case AddParcelToOrder m:
                            return new AssignVehicleToOrderRejected(m.OrderId, m.ParcelId, ex.Message, ex.Code);
                    }

                    break;

                case ParcelNotFoundException ex:
                    switch (message)
                    {
                        case AddParcelToOrder m:
                            return new AddParcelToOrderRejected(m.OrderId, m.ParcelId, ex.Message, ex.Code);
                    }

                    break;

                case ParcelAlreadyAddedToOrderException ex:
                    return new AddParcelToOrderRejected(ex.OrderId, ex.ParcelId, ex.Message, ex.Code);

                case UnauthorizedOrderAccessException ex:
                    switch (message)
                    {
                        case AddParcelToOrder m:
                            return new AddParcelToOrderRejected(m.OrderId, m.ParcelId, ex.Message, ex.Code);
                        case AssignVehicleToOrder m:
                            return new AssignVehicleToOrderRejected(m.OrderId, m.VehicleId, ex.Message, ex.Code);
                        case DeleteOrder m: return new DeleteOrderRejected(m.Id, ex.Message, ex.Code);
                        case DeleteParcelFromOrder m:
                            return new DeleteParcelFromOrderRejected(m.OrderId, m.ParcelId, ex.Message, ex.Code);
                    }

                    break;
                case VehicleNotFoundException ex:
                    switch (message)
                    {
                        case AssignVehicleToOrder m:
                            return new AssignVehicleToOrderRejected(m.OrderId, m.VehicleId, ex.Message, ex.Code);
                    }

                    break;
            }

            return null;
        }
    }
}