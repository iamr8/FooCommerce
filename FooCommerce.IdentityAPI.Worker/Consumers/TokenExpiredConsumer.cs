using FooCommerce.IdentityAPI.Worker.Contracts.Responses;

using MassTransit;

namespace FooCommerce.IdentityAPI.Worker.Consumers;

public class TokenExpiredConsumer :
    IConsumer<TokenExpired>,
    IConsumer<Fault<TokenExpired>>
{
    private readonly ILogger<TokenExpiredConsumer> _logger;

    public TokenExpiredConsumer(ILogger<TokenExpiredConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TokenExpired> context)
    {
        // need to perform some works...
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<Fault<TokenExpired>> context)
    {
        if (context.Message.FaultId == Guid.Empty)
            return Task.CompletedTask;

        foreach (var exception in context.Message.Exceptions)
        {
            _logger.LogError(exception.Message);
        }

        return Task.CompletedTask;
    }
}