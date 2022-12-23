using FooCommerce.Services.ProductAPI.DbProvider;
using FooCommerce.Services.ProductAPI.DbProvider.Entities.Products;
using FooCommerce.Services.ProductAPI.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Services.ProductAPI.Services.Repositories;

public class CategoryManager : ICategoryManager
{
    private readonly ProductDbContext _dbContext;

    public CategoryManager(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateGroupAsync(string name, string slug, string description, string iconPath, CancellationToken cancellationToken = default)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));
        if (slug == null)
            throw new ArgumentNullException(nameof(slug));
        if (description == null)
            throw new ArgumentNullException(nameof(description));

        await using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
        {
            var duplicate = await _dbContext.ProductCategoryGroups
                .AsNoTracking()
                .AnyAsync(x => x.Name == name && x.Slug == slug, cancellationToken);
            if (duplicate)
                throw new DuplicateCategoryGroupFoundException(name);

            try
            {
                var group = _dbContext.ProductCategoryGroups.Add(new ProductCategoryGroup
                {
                    Name = name,
                    Description = description,
                    Slug = slug,
                    Icon = iconPath,
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
    }

    public async Task CreateAsync(int groupId, string name, string description, string iconPath, int? parentId, CancellationToken cancellationToken = default)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));
        if (description == null)
            throw new ArgumentNullException(nameof(description));

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        Guid? _parentId = null;
        if (parentId is > 0)
        {
            var parent = await _dbContext.ProductCategories
                .AsNoTracking()
                .Where(x => x.PublicId == parentId)
                .Select(x => new { x.Id })
                .SingleOrDefaultAsync(cancellationToken);
            if (parent == null)
                throw new ParentCategoryNotFoundException(name, parentId.Value);

            _parentId = parent.Id;
        }

        var group = await _dbContext.ProductCategoryGroups
            .AsNoTracking()
            .Where(x => x.PublicId == groupId)
            .Select(x => new { x.Id })
            .SingleOrDefaultAsync(cancellationToken);
        if (group == null)
            throw new CategoryGroupNotFoundException(groupId);

        var duplicate = await _dbContext.ProductCategories
            .AsNoTracking()
            .AnyAsync(x => x.GroupId == group.Id && x.Name == name && x.ParentId == _parentId, cancellationToken);
        if (duplicate)
            throw new DuplicateCategoryFoundException(name);

        try
        {
            var category = _dbContext.ProductCategories.Add(new ProductCategory
            {
                Name = name,
                ParentId = _parentId,
                Icon = iconPath,
                Description = description,
                GroupId = group.Id
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
}