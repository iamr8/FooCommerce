using System.Data;

using Microsoft.Data.SqlClient;

namespace FooCommerce.Domain.DbProvider;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}