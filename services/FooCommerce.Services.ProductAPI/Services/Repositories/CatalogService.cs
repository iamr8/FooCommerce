using FooCommerce.CatalogService.DbProvider;
using FooCommerce.CatalogService.Exceptions;
using FooCommerce.CatalogService.Models;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.CatalogService.Services.Repositories;

public class CatalogService : ICatalogService
{
    private readonly ProductDbContext _dbContext;

    public CatalogService(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CatalogOverviewModel> GetAsync(string slug, CancellationToken cancellationToken = default)
    {
        if (slug == null)
            throw new ArgumentNullException(nameof(slug));

        var entity = await _dbContext.Catalogs
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Slug == slug)
            .Select(x => new
            {
                x.ExternalId,
                x.Name,
                x.Slug,
                x.Description
            })
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);
        if (entity == null)
            throw new CatalogNotFoundException(slug);

        var model = new CatalogOverviewModel
        {
            Name = entity.Name,
            Slug = entity.Slug,
            Description = entity.Description,
            Id = (int)entity.ExternalId
        };
        return model;
    }
}