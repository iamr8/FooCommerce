using FooCommerce.Domain.Enums;
using FooCommerce.Services.NotificationAPI.Enums;

namespace FooCommerce.Services.NotificationAPI.Exceptions;

public class TemplateModelNotFoundException : Exception
{
    public TemplateModelNotFoundException(CommType type, NotificationPurpose purpose) : base($"Action {type} needs to send notification via {purpose}, but unable to find appropriate {purpose} template for it.")
    {
    }
}