using FooCommerce.Application.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.PurchasableItems.Entities.Configurations;

public class PurchasableItemConfiguration : EntityConfiguration<PurchasableItem>
{
    public override void Configure(EntityTypeBuilder<PurchasableItem> builder)
    {
        base.Configure(builder);
        builder.ToTable("PurchasableItems");
        builder.HasMany(x => x.Extensions)
            .WithOne(x => x.Base)
            .HasForeignKey(x => x.BaseId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}