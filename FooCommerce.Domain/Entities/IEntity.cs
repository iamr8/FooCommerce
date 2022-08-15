namespace FooCommerce.Domain.Entities;

public interface IEntity
{
    /// <summary>
    /// Internal Id of the entity.
    /// </summary>
    /// <remarks>
    /// It will be generated automatically, when entity saved to the database.
    /// </remarks>
    Guid Id { get; init; }

    /// <summary>
    /// Creation date and time of the entity.
    /// </summary>
    /// <remarks>
    /// It will be generated automatically, when entity saved to the database.
    /// </remarks>
    DateTime Created { get; init; }

    /// <summary>
    /// A byte[] timestamp which helps concurrency in the database.
    /// </summary>
    /// <remarks>
    /// It will be generated automatically, when entity saved to the database.
    /// </remarks>
    byte[] RowVersion { get; init; }
}