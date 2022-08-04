using FooCommerce.Application.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Membership.Entities.Configurations;

public class UserPasswordConfiguration : EntityConfiguration<UserPassword>
{
    public override void Configure(EntityTypeBuilder<UserPassword> builder)
    {
        base.Configure(builder);
        builder.ToTable("UserPasswords");
        builder.HasOne(x => x.User)
            .WithMany(x => x.Passwords)
            .HasForeignKey(x => x.UserId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);
    }
}