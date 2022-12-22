using Respawn;
using Respawn.Graph;

namespace FooCommerce.Tests;

public class DatabaseCheckpoint
{
    public static Checkpoint checkpoint = new()
    {
        WithReseed = true,
        CheckTemporalTables = true,
        TablesToIgnore = new[] { new Table("__EFMigrationsHistory"), new Table("SchemaVersions") }
    };
}