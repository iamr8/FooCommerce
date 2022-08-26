using Respawn;

namespace FooCommerce.Tests;

public class DatabaseCheckpoint
{
    public static Checkpoint checkpoint = new()
    {
        WithReseed = true,
        CheckTemporalTables = true,
    };
}