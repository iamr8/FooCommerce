using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.CatalogService.DbProvider.Entities.Products.Configurations;

public class ProductMultimediaConfiguration : IEntityTypeConfiguration<ProductMultimedia>
{
    public void Configure(EntityTypeBuilder<ProductMultimedia> builder)
    {
        builder.ToTable("ProductMultimedia");
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
            .WithMany(x => x.Multimedias)
            .HasForeignKey(x => x.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}