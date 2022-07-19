using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.RealEstates.Entities.Configurations;

public class RealEstateAdConfiguration : EntityConfiguration<RealEstateAd>
{
    public override void Configure(EntityTypeBuilder<RealEstateAd> builder)
    {
        builder.ToTable("RealEstateAds");
    }
}