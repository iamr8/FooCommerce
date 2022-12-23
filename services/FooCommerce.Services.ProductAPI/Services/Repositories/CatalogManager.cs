using FooCommerce.CatalogService.DbProvider;
using FooCommerce.CatalogService.DbProvider.Entities.Products;
using FooCommerce.CatalogService.Exceptions;
using FooCommerce.CatalogService.Services.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace FooCommerce.CatalogService.Services.Repositories;

public class CatalogManager : ICatalogManager
{
    private readonly ProductDbContext _dbContext;

    public CatalogManager(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task DeleteAsync(int catalogId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var catalog = await _dbContext.Catalogs
            .SingleOrDefaultAsync(x => x.ExternalId == catalogId, cancellationToken);
        if (catalog == null)
            throw new CatalogNotFoundException(catalogId);

        try
        {
            catalog.IsDeleted = true;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task CreateAsync(CreateCatalog model, CancellationToken cancellationToken = default)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        Guid? _parentId = null;
        if (model.ParentId is > 0)
        {
            var parent = await _dbContext.Catalogs
                .AsNoTracking()
                .Where(x => x.ExternalId == model.ParentId)
                .Select(x => new { x.Id })
                .SingleOrDefaultAsync(cancellationToken);
            if (parent == null)
                throw new CatalogNotFoundException(model.ParentId.Value);

            _parentId = parent.Id;
        }

        var duplicate = await _dbContext.Catalogs
            .AsNoTracking()
            .AnyAsync(x => x.Name == model.Name && x.ParentId == _parentId, cancellationToken);
        if (duplicate)
            throw new DuplicateCatalogFoundException(model.Name);

        try
        {
            var category = _dbContext.Catalogs.Add(new Catalog
            {
                Name = model.Name,
                ParentId = _parentId,
                Icon = model.IconPath,
                Description = model.Description,
                Slug = model.Slug
            }).Entity;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateAsync(UpdateCatalog model, CancellationToken cancellationToken = default)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var catalog = await _dbContext.Catalogs
            .SingleOrDefaultAsync(x => x.ExternalId == model.Id, cancellationToken);
        if (catalog == null)
            throw new CatalogNotFoundException(model.Id);

        Guid? _parentId = null;
        if (model.ParentId is > 0)
        {
            var parent = await _dbContext.Catalogs
                .AsNoTracking()
                .Where(x => x.ExternalId == model.ParentId)
                .Select(x => new { x.Id })
                .SingleOrDefaultAsync(cancellationToken);
            if (parent == null)
                throw new CatalogNotFoundException(model.ParentId.Value);

            _parentId = parent.Id;
        }

        var duplicate = await _dbContext.Catalogs
            .AsNoTracking()
            .AnyAsync(x => x.ExternalId != model.Id && x.Name == model.Name && x.ParentId == _parentId, cancellationToken);
        if (duplicate)
            throw new DuplicateCatalogFoundException(model.Name);

        try
        {
            catalog.Name = model.Name;
            catalog.ParentId = _parentId;
            catalog.Icon = model.IconPath;
            catalog.Description = model.Description;
            catalog.Slug = model.Slug;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateOrderAsync(int id, int order, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var catalog = await _dbContext.Catalogs
            .SingleOrDefaultAsync(x => x.ExternalId == id, cancellationToken);
        if (catalog == null)
            throw new CatalogNotFoundException(id);

        try
        {
            catalog.Order = order;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateVisibilityAsync(int id, bool visible, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var catalog = await _dbContext.Catalogs
            .SingleOrDefaultAsync(x => x.ExternalId == id && x.IsInvisible == !visible, cancellationToken);
        if (catalog == null)
            throw new CatalogNotFoundException(id);

        try
        {
            catalog.IsInvisible = visible;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task MakePublicAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var catalog = await _dbContext.Catalogs
            .SingleOrDefaultAsync(x => x.ExternalId == id && x.IsInvisible, cancellationToken);
        if (catalog == null)
            throw new CatalogNotFoundException(id);

        try
        {
            catalog.IsInvisible = false;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}