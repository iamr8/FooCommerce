using FooCommerce.Application.Membership.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Core.Membership.Entities.Configurations;

public class UserCommunicationConfiguration : IEntityTypeConfiguration<UserCommunication>
{
    public void Configure(EntityTypeBuilder<UserCommunication> builder)
    {
        builder.ToTable("UserCommunications");
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
        builder.Property(x => x.Type)
            .HasColumnType("TINYINT");
    }
}