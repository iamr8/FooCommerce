using FooCommerce.Services.NotificationAPI.Contracts;

using MassTransit;

namespace FooCommerce.Services.NotificationAPI.Consumers;

public class EnqueueSmsConsumer :
    IConsumer<EnqueueSms>,
    IConsumer<Fault<EnqueueSms>>
{
    private readonly ILogger<EnqueueSmsConsumer> _logger;

    public EnqueueSmsConsumer(ILogger<EnqueueSmsConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<EnqueueSms> context)
    {
        // TODO: SDK must be implemented
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<Fault<EnqueueSms>> context)
    {
        for (int i = 0; i < context.Message.Exceptions.Length; i++)
        {
            _logger.LogError(context.Message.Exceptions[i].Message);
        }

        return Task.CompletedTask;
    }
}