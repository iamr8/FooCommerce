using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Application.Entities.Listings.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Location>
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
            builder.Property(x => x.IsDeleted)
                .HasColumnType("BIT");
            builder.Property(x => x.PublicId)
                .HasColumnType("INT");
            builder.Property(x => x.IsHidden)
                .HasColumnType("BIT");
            builder.Property(x => x.Division)
                .HasColumnType("TINYINT");
            builder.Property(x => x.Name)
                .HasColumnType("NVARCHAR")
                .HasPrecision(100)
                .IsUnicode();
        }
    }
}