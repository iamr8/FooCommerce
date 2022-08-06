using FooCommerce.Application.Enums.Membership;

namespace FooCommerce.Infrastructure.Locations.Dtos;

public class LocationModel
{
    public LocationModel(Guid? parentId = null)
    {
        ParentId = parentId;
    }

    public LocationDivisions Division { get; init; }

    public string Name { get; init; }
    public uint PublicId { get; init; }
    public Guid? ParentId { get; }

    public LocationModel Parent { get; set; }
    public IEnumerable<LocationModel> Children { get; set; } = new List<LocationModel>();

    public void SetParent(LocationModel parent)
    {
        Parent = parent;
        if (parent.Children.All(c => c.PublicId != PublicId))
        {
            parent.Children = parent.Children.Concat(new[] { this });
        }
    }
}