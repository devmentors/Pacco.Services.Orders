using System.Linq;
using Pacco.Services.Orders.Application.DTO;
using Pacco.Services.Orders.Core.Entities;

namespace Pacco.Services.Orders.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Order AsEntity(this OrderDocument document)
            => new Order(document.Id, document.CustomerId, document.Status, document.CreatedAt,
                document.Parcels.Select(p => new Parcel(p.Id, p.Name, p.Variant, p.Size)));

        public static OrderDocument AsDocument(this Order entity)
            => new OrderDocument()
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                TotalPrice = entity.TotalPrice,
                Parcels = entity.Parcels.Select(p => new OrderDocument.Parcel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Size = p.Size,
                    Variant = p.Variant
                })
            };

        public static OrderDto AsDto(this OrderDocument document)
            => new OrderDto
            {
                Id = document.Id,
                CustomerId = document.CustomerId,
                Status = document.Status.ToString().ToLowerInvariant(),
                CreatedAt = document.CreatedAt,
                TotalPrice = document.TotalPrice,
                Parcels = document.Parcels.Select(p => new ParcelDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Size = p.Size,
                    Variant = p.Variant
                })
            };
    }
}