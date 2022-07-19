using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.RealEstates.Entities.Configurations;

public class RealEstateConfiguration : EntityConfiguration<RealEstate>
{
    public override void Configure(EntityTypeBuilder<RealEstate> builder)
    {
        base.Configure(builder);
        builder.ToTable("RealEstates");
        builder.HasMany(x => x.Ads)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}