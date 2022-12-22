using FooCommerce.Domain.Enums;
using FooCommerce.Localization.Models;
using FooCommerce.Services.NotificationAPI.Interfaces;

namespace FooCommerce.Services.NotificationAPI.Dtos;

public sealed record PushTemplateModel : ITemplate
{
    public CommType Communication => CommType.Push;
    public LocalizerValueCollection Subject { get; init; }
    public LocalizerValueCollection Message { get; init; }
    public IDictionary<string, string> Values { get; }
}