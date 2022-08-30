using System.Data;

namespace FooCommerce.Core.DbProvider.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}