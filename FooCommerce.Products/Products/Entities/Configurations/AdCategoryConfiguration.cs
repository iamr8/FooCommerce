using FooCommerce.Application.DbProvider;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Products.Products.Entities.Configurations
{
    public class AdCategoryConfiguration : EntityConfiguration<AdCategory>
    {
        public override void Configure(EntityTypeBuilder<AdCategory> builder)
        {
            base.Configure(builder);
            builder.ToTable("AdCategories");
            builder.Property(x => x.PublicId).UseHiLo("adcategories_seq");
            builder.HasMany(x => x.Children)
                .WithOne(x => x.Parent)
                .HasForeignKey(x => x.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}