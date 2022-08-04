using FooCommerce.Application.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.PurchasableItems.SecondHandItems.Entities.Configurations;

public class PurchasableSecondHandItemConfiguration : EntityConfiguration<PurchasableSecondHandItem>
{
    public override void Configure(EntityTypeBuilder<PurchasableSecondHandItem> builder)
    {
        base.Configure(builder);
        builder.ToTable("PurchasableSecondHandItems");
        builder.HasMany(x => x.Extensions)
            .WithOne(x => x.Base)
            .HasForeignKey(x => x.BaseId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}