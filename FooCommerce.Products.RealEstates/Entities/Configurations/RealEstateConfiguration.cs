using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.RealEstates.Entities.Configurations;

public class RealEstateConfiguration : EntityConfiguration<RealEstate>
{
    public override void Configure(EntityTypeBuilder<RealEstate> builder)
    {
        base.Configure(builder);
        builder.ToTable("RealEstates");
        builder.Property(x => x.PublicId).UseHiLo("realestateadsseq", "realestate");
        builder.HasMany(x => x.Extensions)
            .WithOne(x => x.Base)
            .HasForeignKey(x => x.BaseId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}