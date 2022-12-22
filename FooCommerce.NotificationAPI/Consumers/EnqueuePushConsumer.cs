using FooCommerce.Services.NotificationAPI.Contracts;

using MassTransit;

namespace FooCommerce.Services.NotificationAPI.Consumers;

public class EnqueuePushConsumer :
    IConsumer<EnqueuePush>,
    IConsumer<Fault<EnqueuePush>>
{
    private readonly ILogger<EnqueuePushConsumer> _logger;

    public EnqueuePushConsumer(ILogger<EnqueuePushConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<EnqueuePush> context)
    {
        // TODO: SDK must be implemented
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<Fault<EnqueuePush>> context)
    {
        for (int i = 0; i < context.Message.Exceptions.Length; i++)
        {
            _logger.LogError(context.Message.Exceptions[i].Message);
        }

        return Task.CompletedTask;
    }
}