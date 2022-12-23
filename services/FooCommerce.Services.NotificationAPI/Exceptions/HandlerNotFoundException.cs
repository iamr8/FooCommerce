using FooCommerce.Domain.Enums;

namespace FooCommerce.NotificationService.Exceptions;

public class HandlerNotFoundException : Exception
{
    public HandlerNotFoundException(CommType type)
    {
    }
}