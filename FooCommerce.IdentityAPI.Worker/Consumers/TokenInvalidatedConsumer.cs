using FooCommerce.IdentityAPI.Worker.Contracts.Responses;

using MassTransit;

namespace FooCommerce.IdentityAPI.Worker.Consumers;

public class TokenInvalidatedConsumer :
    IConsumer<TokenInvalidated>,
    IConsumer<Fault<TokenInvalidated>>
{
    private readonly ILogger<TokenInvalidatedConsumer> _logger;

    public TokenInvalidatedConsumer(ILogger<TokenInvalidatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TokenInvalidated> context)
    {
        // need to perform some works...
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<Fault<TokenInvalidated>> context)
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