using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events
{
    public class CreateOrderRejected : IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        public CreateOrderRejected(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }
    }
}