using Microsoft.AspNetCore.Html;

namespace FooCommerce.Application.Models.Notifications.Types;

public record NotificationEmailModel(IHtmlContent Html);