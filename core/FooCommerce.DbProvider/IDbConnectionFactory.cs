using System.Data;

namespace FooCommerce.DbProvider;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}