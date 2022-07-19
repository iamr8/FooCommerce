using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.PurchasableItems.SecondHandItems.Entities.Configurations;

public class PurchasableSecondHandItemConfiguration : EntityConfiguration<PurchasableSecondHandItem>
{
    public override void Configure(EntityTypeBuilder<PurchasableSecondHandItem> builder)
    {
        builder.ToTable("PurchasableSecondHandItems");
        builder.HasMany(x => x.Ads)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}