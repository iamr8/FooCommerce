using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.PurchasableItems.SecondHandItems.Entities.Configurations;

public class PurchasableSecondHandItemAdConfiguration : EntityConfiguration<PurchasableSecondHandItemAd>
{
    public override void Configure(EntityTypeBuilder<PurchasableSecondHandItemAd> builder)
    {
        base.Configure(builder);
        builder.ToTable("PurchasableSecondHandItemAds");
    }
}