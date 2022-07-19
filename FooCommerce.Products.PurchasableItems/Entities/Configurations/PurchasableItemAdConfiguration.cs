using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.PurchasableItems.Entities.Configurations;

public class PurchasableItemAdConfiguration : EntityConfiguration<PurchasableItemAd>
{
    public override void Configure(EntityTypeBuilder<PurchasableItemAd> builder)
    {
        builder.ToTable("PurchasableItemAds");
    }
}