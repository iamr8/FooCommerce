using FooCommerce.IdentityAPI.Worker.Contracts.Responses;

using MassTransit;

namespace FooCommerce.IdentityAPI.Worker.Consumers;

public class TokenNotValidatedConsumer :
    IConsumer<TokenNotValidated>,
    IConsumer<Fault<TokenNotValidated>>
{
    private readonly ILogger<TokenNotValidatedConsumer> _logger;

    public TokenNotValidatedConsumer(ILogger<TokenNotValidatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TokenNotValidated> context)
    {
        // need to perform some works...
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<Fault<TokenNotValidated>> context)
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