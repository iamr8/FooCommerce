using FooCommerce.Services.TokenAPI.Interfaces;

using MassTransit;

namespace FooCommerce.Services.TokenAPI.Sagas;

public class TokenState
    : SagaStateMachineInstance, IIdentifier
{
    public const int MaxRetryCount = 3;

    public int CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid IdentifierId { get; set; }

    public Guid? TokenTimeoutTokenId { get; set; }
    public string Code { get; set; }
    public int IntervalSeconds { get; set; }

    public int FalseTries { get; set; }

    public DateTime? GeneratedOn { get; set; }
    public DateTime? ValidatedOn { get; set; }
    public DateTime? InvalidatedOn { get; set; }
    public Dictionary<DateTime, string> NotValidatedOn { get; set; } = new();
    public DateTime? ExpiresOn { get; set; }
}