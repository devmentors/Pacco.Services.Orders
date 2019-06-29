using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Orders.Application.Commands
{
    [Contract]
    public class CancelOrder : ICommand
    {
        public Guid Id { get; }
        public string Reason { get; }

        public CancelOrder(Guid id, string reason)
        {
            Id = id;
            Reason = reason;
        }
    }
}