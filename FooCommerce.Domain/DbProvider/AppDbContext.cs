
using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Domain.DbProvider
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}