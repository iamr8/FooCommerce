using FooCommerce.Domain.DbProvider;

namespace FooCommerce.Products.Domain.Entities
{
    public record AdComment : Entity
    {
        public string Text { get; set; }
        public int ReviewScore { get; set; }
        public Guid? AdId { get; set; }
        public Guid? RepliedToId { get; set; }
        public virtual AdComment RepliedTo { get; set; }
        public virtual ICollection<AdComment> Replies { get; set; }
    }
}