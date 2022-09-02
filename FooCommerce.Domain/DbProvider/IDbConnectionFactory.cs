using System.Data;

namespace FooCommerce.Domain.DbProvider;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}