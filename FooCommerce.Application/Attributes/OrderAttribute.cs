namespace FooCommerce.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Enum | AttributeTargets.Field)]
    public class OrderAttribute : Attribute
    {
        private readonly int _priority;

        public OrderAttribute(int priority)
        {
            _priority = priority;
        }

        public int Priority => _priority;
    }
}