using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Services.ProductAPI.DbProvider.Entities.Products.Configurations;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories");
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

        builder.HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Group)
            .WithMany(x => x.Categories)
            .HasForeignKey(x => x.GroupId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}