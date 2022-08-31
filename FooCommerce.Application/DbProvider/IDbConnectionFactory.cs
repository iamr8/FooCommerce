using System.Data;

namespace FooCommerce.Application.DbProvider;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}