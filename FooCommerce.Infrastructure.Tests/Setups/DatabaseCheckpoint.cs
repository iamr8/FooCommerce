using Respawn;

namespace FooCommerce.Infrastructure.Tests.Setups
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