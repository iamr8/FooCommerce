using Microsoft.AspNetCore.Html;

namespace FooCommerce.Application.Notifications.Models.Types;

public record NotificationEmailModel(IHtmlContent Html);