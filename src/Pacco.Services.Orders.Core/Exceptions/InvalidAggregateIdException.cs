using System;

namespace Pacco.Services.Orders.Core.Exceptions
{
    public class InvalidAggregateIdException : ExceptionBase
    {
        public override string Code => "invalid_aggregate_id";
        public Guid Id { get; }
        
        public InvalidAggregateIdException(Guid id) : base($"Invalid aggregate id.")
        {
            Id = id;
        }
    }
}