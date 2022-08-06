using FooCommerce.Infrastructure.Shopping.Components;
using FooCommerce.Infrastructure.Shopping.Contracts;

using MassTransit.EntityFrameworkCoreIntegration;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Infrastructure.Shopping.DatabaseProvider
{
    public class OrderDbContext : SagaDbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new OrderStateMap(); }
        }
    }
}