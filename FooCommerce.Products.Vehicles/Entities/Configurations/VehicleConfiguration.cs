using FooCommerce.Application.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.Vehicles.Entities.Configurations;

public class VehicleConfiguration : EntityConfiguration<Vehicle>
{
    public override void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        base.Configure(builder);
        builder.ToTable("Vehicles");
        builder.HasMany(x => x.Extensions)
            .WithOne(x => x.Base)
            .HasForeignKey(x => x.BaseId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}