using Respawn;

namespace FooCommerce.Tests.Base;

public class DatabaseCheckpoint
{
    public static Checkpoint checkpoint = new()
    {
        WithReseed = true,
        CheckTemporalTables = true,
    };
}