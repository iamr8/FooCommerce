using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Membership.Entities.Configurations;

public class UserLockoutConfiguration : EntityConfiguration<UserLockout>
{
    public override void Configure(EntityTypeBuilder<UserLockout> builder)
    {
        base.Configure(builder);
        builder.ToTable("UserLockouts");
        builder.HasOne(x => x.User)
            .WithMany(x => x.Lockouts)
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}