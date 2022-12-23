using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Services.ProductAPI.DbProvider.Entities.Products.Configurations;

public class ProductSpecificationConfiguration : IEntityTypeConfiguration<ProductSpecification>
{
    public void Configure(EntityTypeBuilder<ProductSpecification> builder)
    {
        builder.ToTable("ProductSpecifications");
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

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Specifications)
            .HasForeignKey(x => x.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Specification)
            .WithMany(x => x.ProductSpecifications)
            .HasForeignKey(x => x.SpecificationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}