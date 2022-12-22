namespace FooCommerce.Services.ProductAPI.DbProvider.Entities.Listings.Dtos;

public record LocationModel
{
    public Guid Id { get; init; }
    public byte Division { get; init; } // LocationDivision

    public string Name { get; init; }
    public uint PublicId { get; init; }
    public Guid? ParentId { get; init; }
}