namespace FooCommerce.Products.Ads.Interfaces
{
    public interface IAd
    {
        DateTime EndDate { get; set; }
        Guid ProductId { get; set; }
        Guid? ParentAdId { get; set; }
        Guid UserSubscriptionId { get; set; }
    }
}