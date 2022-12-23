using FooCommerce.Domain.Enums;
using FooCommerce.Localization.Models;
using FooCommerce.NotificationService.Interfaces;

namespace FooCommerce.NotificationService.Dtos;

public sealed record SmsTemplateModel : ITemplate
{
    public CommType Communication => CommType.Sms;
    public LocalizerValueCollection Text { get; init; }
    public IDictionary<string, string> Values { get; }
}