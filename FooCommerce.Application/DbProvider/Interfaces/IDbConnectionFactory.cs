using System.Data;

namespace FooCommerce.Application.DbProvider.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}