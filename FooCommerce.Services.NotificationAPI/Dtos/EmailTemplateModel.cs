using FooCommerce.Domain.Enums;
using FooCommerce.Localization.Models;
using FooCommerce.Services.NotificationAPI.Interfaces;

namespace FooCommerce.Services.NotificationAPI.Dtos;

public sealed record EmailTemplateModel : ITemplate
{
    public CommType Communication => CommType.Email;
    public LocalizerValueCollection Html { get; init; }
    public bool ShowRequestData { get; init; }
    public IDictionary<string, string> Values { get; }
}