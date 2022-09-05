using FooCommerce.DbProvider.Entities.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.DbProvider.Entities.Listings.Entities.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("Locations");
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
        builder.Property(x => x.Division)
            .HasColumnType("TINYINT");
    }
}