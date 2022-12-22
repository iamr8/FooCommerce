using FooCommerce.IdentityAPI.Worker.Contracts.Responses;

using MassTransit;

namespace FooCommerce.IdentityAPI.Worker.Consumers;

public class TokenGeneratedConsumer :
    IConsumer<TokenGenerated>,
    IConsumer<Fault<TokenGenerated>>
{
    private readonly ILogger<TokenGeneratedConsumer> _logger;

    public TokenGeneratedConsumer(ILogger<TokenGeneratedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TokenGenerated> context)
    {
        // need to perform some works...
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<Fault<TokenGenerated>> context)
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