using System;

namespace Pacco.Services.Orders.Core.Entities
{
    public class Parcel : IEquatable<Parcel>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Variant { get; private set; }
        public string Size { get; private set; }
        public decimal Price { get; private set; }

        public Parcel(Guid id, string name, string variant, string size, decimal price = 0)
        {
            Id = id;
            Name = name;
            Variant = variant;
            Size = size;
            SetPrice(price);
        }

        public void SetPrice(decimal price)
        {
            Price = price;
        }

        public bool Equals(Parcel other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Parcel) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}