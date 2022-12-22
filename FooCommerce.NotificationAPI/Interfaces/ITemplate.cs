using FooCommerce.Domain.Enums;

namespace FooCommerce.Services.NotificationAPI.Interfaces;

public interface ITemplate
{
    CommType Communication { get; }
    IDictionary<string, string> Values { get; }
}