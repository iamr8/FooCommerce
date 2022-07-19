using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.Vehicles.Entities.Configurations;

public class VehicleAdConfiguration : EntityConfiguration<VehicleAd>
{
    public override void Configure(EntityTypeBuilder<VehicleAd> builder)
    {
        base.Configure(builder);
        builder.ToTable("VehicleAds");
    }
}