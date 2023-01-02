using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.CatalogService.DbProvider.Entities.Listings.Configurations;

public class ListingCommentConfiguration : IEntityTypeConfiguration<ListingComment>
{
    public void Configure(EntityTypeBuilder<ListingComment> builder)
    {
        builder.ToTable("ListingComments");
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
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.ListingId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ParentComment)
            .WithMany(x => x.ChildComments)
            .HasForeignKey(x => x.ParentCommentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}