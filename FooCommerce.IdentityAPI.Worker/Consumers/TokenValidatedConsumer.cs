using FooCommerce.IdentityAPI.Worker.Contracts.Responses;

using MassTransit;

namespace FooCommerce.IdentityAPI.Worker.Consumers;

public class TokenValidatedConsumer :
    IConsumer<TokenValidated>,
    IConsumer<Fault<TokenValidated>>
{
    private readonly ILogger<TokenValidatedConsumer> _logger;

    public TokenValidatedConsumer(ILogger<TokenValidatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TokenValidated> context)
    {
        // need to perform some works...
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<Fault<TokenValidated>> context)
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