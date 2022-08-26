using Microsoft.AspNetCore.Html;

namespace FooCommerce.NotificationAPI.Models.Types;

public record NotificationEmailModel(IHtmlContent Html);