using FooCommerce.Application.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Membership.Entities.Configurations;

public class AuthTokenConfiguration : EntityConfiguration<AuthToken>
{
    public override void Configure(EntityTypeBuilder<AuthToken> builder)
    {
        base.Configure(builder);
        builder.ToTable("AuthTokens");
        builder.HasOne(x => x.UserContact)
            .WithMany(x => x.Tokens)
            .HasForeignKey(x => x.UserContactId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}