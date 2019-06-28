using System;
using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Orders.Core.Events;
using Pacco.Services.Orders.Core.Exceptions;

namespace Pacco.Services.Orders.Core.Entities
{
    public class Order : AggregateRoot
    {
        private ISet<Parcel> _parcels = new HashSet<Parcel>();
        public Guid CustomerId { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string CancellationReason { get; private set; }

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
            AddEvent(new OrderStateChanged(this));
        }

        public bool CanBeDeleted() => Status == OrderStatus.New;

        public bool AddParcel(Parcel parcel)
        {
            if (!_parcels.Add(parcel))
            {
                return false;
            }

            AddEvent(new ParcelAdded(this, parcel));

            return true;
        }

        public bool DeleteParcel(Guid id)
        {
            var parcel = _parcels.SingleOrDefault(p => p.Id == id);
            if (parcel is null)
            {
                return false;
            }

            _parcels.Remove(parcel);
            AddEvent(new ParcelDeleted(this, parcel));

            return true;
        }

        public void Approve(decimal totalPrice)
        {
            if (Status != OrderStatus.New)
            {
                throw new CannotChangeOrderStateException(Id, Status, OrderStatus.Approved);
            }

            SetTotalPrice(totalPrice);
            Status = OrderStatus.Approved;
            AddEvent(new OrderStateChanged(this));
        }

        private void SetTotalPrice(decimal totalPrice)
        {
            if (totalPrice < 0)
            {
                throw new InvalidOrderPriceException(Id, totalPrice);
            }

            TotalPrice = totalPrice;
        }

        public void Cancel(string reason)
        {
            if (Status == OrderStatus.Completed || Status == OrderStatus.Canceled)
            {
                throw new CannotChangeOrderStateException(Id, Status, OrderStatus.Canceled);
            }

            Status = OrderStatus.Canceled;
            CancellationReason = reason;
            AddEvent(new OrderStateChanged(this));
        }

        public void Complete()
        {
            if (Status == OrderStatus.Completed || Status == OrderStatus.Canceled)
            {
                throw new CannotChangeOrderStateException(Id, Status, OrderStatus.Completed);
            }

            Status = OrderStatus.Completed;
            AddEvent(new OrderStateChanged(this));
        }

        public void SetDelivering()
        {
            if (Status != OrderStatus.Approved)
            {
                throw new CannotChangeOrderStateException(Id, Status, OrderStatus.Delivering);
            }

            Status = OrderStatus.Delivering;
            AddEvent(new OrderStateChanged(this));
        }
    }
}