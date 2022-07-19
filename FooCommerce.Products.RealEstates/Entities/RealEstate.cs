using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;
using FooCommerce.Products.Products.Interfaces;

using NetTopologySuite.Geometries;

namespace FooCommerce.Products.RealEstates.Entities;

public class RealEstate : Entity, IEntityProduct<RealEstateAd>, IEntityCoordinate
{
    public Point? Coordinate { get; set; }
    public Guid? CategoryId { get; set; }
    public ICollection<RealEstateAd> Ads { get; set; }
}