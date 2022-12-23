using FooCommerce.Services.ProductAPI.Exceptions;

namespace FooCommerce.Services.ProductAPI.Services;

public interface ICategoryManager
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="iconPath"></param>
    /// <param name="parentId"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ParentCategoryNotFoundException"></exception>
    /// <exception cref="DuplicateCategoryFoundException"></exception>
    /// <exception cref="CategoryGroupNotFoundException"></exception>
    Task CreateAsync(int groupId, string name, string description, string iconPath, int? parentId, CancellationToken cancellationToken = default);

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="slug"></param>
    /// <param name="description"></param>
    /// <param name="iconPath"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="DuplicateCategoryGroupFoundException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task CreateGroupAsync(string name, string slug, string description, string iconPath, CancellationToken cancellationToken = default);
}