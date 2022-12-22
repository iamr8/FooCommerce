using FooCommerce.Domain.Enums;

namespace FooCommerce.Services.NotificationAPI.Exceptions;

public class HandlerNotFoundException : Exception
{
    public HandlerNotFoundException(CommType type)
    {
    }
}