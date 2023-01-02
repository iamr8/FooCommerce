using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.CatalogService.DbProvider.Entities.Listings.Configurations;

public class ListingPriceConfiguration : IEntityTypeConfiguration<ListingPrice>
{
    public void Configure(EntityTypeBuilder<ListingPrice> builder)
    {
        builder.ToTable("ListingPrices");
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

        builder.HasOne(x => x.Listing)
            .WithMany(x => x.Prices)
            .HasForeignKey(x => x.ListingId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}