using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.DigitalProducts.Entities.Configurations;

public class DigitalProductConfiguration : EntityConfiguration<DigitalProduct>
{
    public override void Configure(EntityTypeBuilder<DigitalProduct> builder)
    {
        base.Configure(builder);
        builder.ToTable("DigitalProducts");
        builder.Property(x => x.Name).IsRequired();
        builder.HasMany(x => x.Ads)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}