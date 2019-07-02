using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events.Rejected
{
    [Contract]
    public class CreateOrderRejected : IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }
        public Guid CustomerId { get; }

        public CreateOrderRejected(Guid customerId, string reason, string code)
        {
            CustomerId = customerId;
            Reason = reason;
            Code = code;
        }
    }
}