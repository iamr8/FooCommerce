using System.Data;

namespace FooCommerce.Core.DbProvider
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}