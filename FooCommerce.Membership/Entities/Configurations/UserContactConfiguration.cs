using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Membership.Entities.Configurations;

public class UserContactConfiguration : EntityConfiguration<UserContact>
{
    public override void Configure(EntityTypeBuilder<UserContact> builder)
    {
        builder.ToTable("UserContacts");
        builder.HasOne(x => x.User)
            .WithMany(x => x.Contacts)
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}