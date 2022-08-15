using Respawn;

namespace FooCommerce.Infrastructure.Tests
{
    internal class DatabaseCheckpoint
    {
        public static Checkpoint checkpoint = new()
        {
            WithReseed = true,
            CheckTemporalTables = true,
        };
    }
}