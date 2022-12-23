using FooCommerce.Domain.Enums;

namespace FooCommerce.NotificationService.Interfaces;

public interface ITemplate
{
    CommType Communication { get; }
    IDictionary<string, string> Values { get; }
}