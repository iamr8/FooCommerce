using MassTransit;

namespace FooCommerce.TokenService.Sagas;

public class TokenState
    : SagaStateMachineInstance
{
    public const int MaxRetryCount = 3;

    public int CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid IdentifierId { get; set; }

    public Guid? TokenTimeoutTokenId { get; set; }
    public string Code { get; set; }
    public int LifetimeInSeconds { get; set; }

    public int FalseTries { get; set; }

    public DateTime? GeneratedOn { get; set; }
    public DateTime? ValidatedOn { get; set; }
    public DateTime? InvalidatedOn { get; set; }
    public Dictionary<DateTime, string> NotValidatedOn { get; set; } = new();
    public DateTime? ExpiresOn { get; set; }
}