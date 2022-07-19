using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.PurchasableItems.Entities.Configurations;

public class PurchasableItemConfiguration : EntityConfiguration<PurchasableItem>
{
    public override void Configure(EntityTypeBuilder<PurchasableItem> builder)
    {
        builder.ToTable("PurchasableItems");
        builder.HasMany(x => x.Ads)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}