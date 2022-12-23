using FooCommerce.CatalogService.DbProvider;

namespace FooCommerce.CatalogService.Services.Repositories;

public class ProductManager : IProductManager
{
    private readonly ProductDbContext _dbContext;

    public ProductManager(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}