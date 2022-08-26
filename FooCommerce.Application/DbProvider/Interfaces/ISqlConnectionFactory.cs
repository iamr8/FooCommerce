using System.Data;

namespace FooCommerce.Application.DbProvider.Interfaces;

public interface ISqlConnectionFactory
{
    IDbConnection GetOpenConnection();

    IDbConnection CreateNewConnection();

    string GetConnectionString();
}