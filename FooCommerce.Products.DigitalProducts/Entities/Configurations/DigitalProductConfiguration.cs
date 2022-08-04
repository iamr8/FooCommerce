using FooCommerce.Application.DbProvider;

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
        builder.HasMany(x => x.Extensions)
            .WithOne(x => x.Base)
            .HasForeignKey(x => x.BaseId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}