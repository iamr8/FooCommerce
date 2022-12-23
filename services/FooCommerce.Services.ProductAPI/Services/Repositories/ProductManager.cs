using FooCommerce.Services.ProductAPI.DbProvider;

namespace FooCommerce.Services.ProductAPI.Services.Repositories;

public class ProductManager : IProductManager
{
    private readonly ProductDbContext _dbContext;

    public ProductManager(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}