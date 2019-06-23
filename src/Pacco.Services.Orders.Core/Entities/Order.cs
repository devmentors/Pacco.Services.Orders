using System;
using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Orders.Core.Events;

namespace Pacco.Services.Orders.Core.Entities
{
    public class Order : AggregateRoot
    {
        private ISet<Parcel> _parcels = new HashSet<Parcel>();
        public Guid CustomerId { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public decimal TotalPrice { get; private set; }

        public IEnumerable<Parcel> Parcels
        {
            get => _parcels;
            private set => _parcels = new HashSet<Parcel>(value);
        }

        public Order(AggregateId id, Guid customerId, OrderStatus status, DateTime createdAt,
            IEnumerable<Parcel> parcels = null)
        {
            Id = id;
            CustomerId = customerId;
            Status = status;
            CreatedAt = createdAt;
            Parcels = parcels ?? Enumerable.Empty<Parcel>();
            CalculateTotalPrice();
            AddEvent(new OrderCreated(this));
        }

        public void AddParcel(Parcel parcel)
        {
            _parcels.Add(parcel);
            CalculateTotalPrice();
        }

        public void RemoveParcel(Guid id)
        {
            var parcel = _parcels.SingleOrDefault(p => p.Id == id);
            if (parcel is null)
            {
                return;
            }

            _parcels.Remove(parcel);
            CalculateTotalPrice();
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = _parcels.Sum(p => p.Price);
        }
    }
}