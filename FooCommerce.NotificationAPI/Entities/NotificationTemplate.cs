using FooCommerce.Application.Membership.Enums;
using FooCommerce.Domain.Interfaces.Database;

namespace FooCommerce.NotificationAPI.Entities
{
    public record NotificationTemplate
        : IEntity
    {
        public Guid Id { get; init; }
        public DateTime Created { get; init; }
        public byte[] RowVersion { get; init; }
        public CommunicationType Type { get; init; }

        public string JsonTemplate { get; init; }
        public bool IncludeRequest { get; init; }
        public Guid NotificationId { get; init; }
    }
}