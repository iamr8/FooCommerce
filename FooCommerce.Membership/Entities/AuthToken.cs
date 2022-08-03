using System.Net;

using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;
using FooCommerce.Membership.Enums;

namespace FooCommerce.Membership.Entities
{
    public record AuthToken : Entity, IEntityRequestLog
    {
        public TokenAction Action { get; set; }
        public string Token { get; set; }
        public IPAddress IPAddress { get; set; }
        public string UserAgent { get; set; }
        public DateTime? Sent { get; set; }
        public DateTime? Delivered { get; set; }
        public DateTime? Received { get; set; }
        public DateTime? Authorized { get; set; }
        public Guid UserContactId { get; set; }
        public virtual UserContact UserContact { get; set; }
    }
}