using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Services.ProductAPI.DbProvider.Entities.Products.Configurations;

public class SpecificationConfiguration : IEntityTypeConfiguration<Specification>
{
    public void Configure(EntityTypeBuilder<Specification> builder)
    {
        builder.ToTable("Specifications");
        builder.Property(x => x.Id)
            .HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL");
        builder.HasIndex(x => x.Id)
            .IsUnique();
        builder.Property(x => x.Created)
            .HasColumnType("DATETIME2")
            .HasPrecision(7)
            .HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.RowVersion)
            .HasColumnType("ROWVERSION")
            .IsRequired()
            .IsRowVersion()
            .IsConcurrencyToken();
    }
}