namespace FooCommerce.Core.Configurations;

public class RabbitMQConfiguration
{
    public static string Host => ContainerSettings.IsRunningInContainer ? "rabbitmq" : "localhost";
    public const string VirtualHost = "/";
    public const string Username = "guest";
    public const string Password = "guest";
}