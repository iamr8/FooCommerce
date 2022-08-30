using System.Data;

namespace FooCommerce.Core.DbProvider.Interfaces;

public interface ISqlConnectionFactory
{
    IDbConnection GetOpenConnection();

    IDbConnection CreateNewConnection();

    string GetConnectionString();
}