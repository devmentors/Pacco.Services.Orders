using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Orders.Application.Commands
{
    [Contract]
    public class AddParcelToOrder : ICommand
    {
        public Guid OrderId { get; }
        public Guid ParcelId { get; }
        public Guid? CustomerId { get; }

        public AddParcelToOrder(Guid orderId, Guid parcelId, Guid? customerId)
        {
            OrderId = orderId;
            ParcelId = parcelId;
            CustomerId = customerId;
        }
    }
}