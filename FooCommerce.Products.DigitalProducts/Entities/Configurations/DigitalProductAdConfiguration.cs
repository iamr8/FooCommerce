using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.DigitalProducts.Entities.Configurations;

public class DigitalProductAdConfiguration : EntityConfiguration<DigitalProductAd>
{
    public override void Configure(EntityTypeBuilder<DigitalProductAd> builder)
    {
        base.Configure(builder);
        builder.ToTable("DigitalProductAds");
    }
}