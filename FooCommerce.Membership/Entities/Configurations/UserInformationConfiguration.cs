using FooCommerce.Domain.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Membership.Entities.Configurations;

public class UserInformationConfiguration : EntityConfiguration<UserInformation>
{
    public override void Configure(EntityTypeBuilder<UserInformation> builder)
    {
        base.Configure(builder);
        builder.ToTable("UserInformation");
        builder.HasOne(x => x.User)
            .WithMany(x => x.Information)
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}