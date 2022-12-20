using System.Security.Cryptography;

using FooCommerce.Services.TokenAPI.Contracts;
using FooCommerce.Services.TokenAPI.Enums;
using FooCommerce.Services.TokenAPI.Sagas.Contracts;

using MassTransit;

namespace FooCommerce.Services.TokenAPI.Sagas;

// ReSharper disable once ClassNeverInstantiated.Global
public class TokenStateMachine
    : MassTransitStateMachine<TokenState>
{
    public TokenStateMachine()
    {
        this.Event(() => GenerationRequested, x => x.CorrelateById(p => p.Message.IdentifierId));
        this.Event(() => TokenGenerated, x => x.CorrelateById(p => p.Message.IdentifierId));
        this.Event(() => ValidationRequested, x =>
        {
            x.OnMissingInstance(c =>
                c.ExecuteAsync(v =>
                    v.RespondAsync<TokenValidationStatus>(new
                    {
                        Status = TokenStatus.NotFound
                    })));
            x.CorrelateById(p => p.Message.IdentifierId);
        });

        this.InstanceState(x => x.CurrentState, Active);
        Schedule(() => TokenTimeout, x => x.TokenTimeoutTokenId, x =>
        {
            x.DelayProvider = ctx => TimeSpan.FromSeconds(ctx.Saga.IntervalSeconds);
            x.Received = p => p.CorrelateById(context => context.Message.IdentifierId);
        });

        this.Initially(
            this.When(GenerationRequested)
                .Then(context =>
                {
                    context.Saga.IntervalSeconds = context.Message.Seconds;
                    context.Saga.IdentifierId = context.Message.IdentifierId;
                    context.Saga.Code = RandomNumberGenerator.GetInt32(10_000, 99_999).ToString();
                    context.Saga.GeneratedOn = DateTime.UtcNow;
                    context.Saga.ExpiresOn = DateTime.UtcNow.Add(TimeSpan.FromSeconds(context.Saga.IntervalSeconds));
                })
                .PublishAsync(context => context.Init<SagaTokenGenerated>(new
                {
                    context.Saga.IdentifierId,
                    context.Saga.Code,
                    GeneratedOn = DateTime.UtcNow
                }))
                .TransitionTo(Active)

        );

        this.During(Active,
            this.When(TokenGenerated)
                .RespondAsync(context => context.Init<TokenGenerationStatus>(new
                {
                    context.Saga.ExpiresOn
                }))
                .Schedule(TokenTimeout, context => context.Init<SagaTokenExpired>(new
                {
                    context.Saga.IdentifierId,
                })),
            this.When(TokenTimeout.Received)
                .Then(context => context.Saga.InvalidatedOn = DateTime.UtcNow)
                .RespondAsync(context => context.Init<TokenValidationStatus>(new
                {
                    Status = TokenStatus.Expired
                }))
                .Finalize(),
            this.When(ValidationRequested)
                .IfElse(context => context.Message.Code == context.Saga.Code,
                    thenActivityCallback: WhenCodeIsValid(),
                    elseActivityCallback: WhenCodeIsNotValid()
                )
        );

        SetCompletedWhenFinalized();
    }

    public State Active { get; private set; }

    public Schedule<TokenState, SagaTokenExpired> TokenTimeout { get; private set; }

    public Event<GenerateCode> GenerationRequested { get; private set; }
    public Event<SagaTokenGenerated> TokenGenerated { get; private set; }
    public Event<ValidateCode> ValidationRequested { get; private set; }

    private Func<EventActivityBinder<TokenState, ValidateCode>, EventActivityBinder<TokenState, ValidateCode>> WhenMaxRetryExceeded()
    {
        return ifTrue => ifTrue
            .Then(context => context.Saga.InvalidatedOn = DateTime.UtcNow)
            .RespondAsync(context => context.Init<TokenValidationStatus>(new
            {
                Status = TokenStatus.MaxRetryExceeded
            }))
            .Unschedule(TokenTimeout)
            .Finalize();
    }

    private Func<EventActivityBinder<TokenState, ValidateCode>, EventActivityBinder<TokenState, ValidateCode>> WhenMaxRetryNotExceeded()
    {
        return ifFalse => ifFalse
            .RespondAsync(context => context.Init<TokenValidationStatus>(new
            {
                Status = TokenStatus.TokenInvalid
            }));
    }

    private Func<EventActivityBinder<TokenState, ValidateCode>, EventActivityBinder<TokenState, ValidateCode>> WhenCodeIsNotValid()
    {
        return ifFalse => ifFalse
            .Then(context =>
            {
                context.Saga.FalseTries++;
                context.Saga.NotValidatedOn.Add(DateTime.UtcNow, context.Message.Code);
            })
            .IfElse(context => context.Saga.FalseTries == TokenState.MaxRetryCount,
                thenActivityCallback: WhenMaxRetryExceeded(),
                elseActivityCallback: WhenMaxRetryNotExceeded()
                );
    }

    private Func<EventActivityBinder<TokenState, ValidateCode>, EventActivityBinder<TokenState, ValidateCode>> WhenCodeIsValid()
    {
        return ifTrue => ifTrue
            .Then(context => context.Saga.ValidatedOn = DateTime.UtcNow)
            .RespondAsync(context => context.Init<TokenValidationStatus>(new
            {
                Status = TokenStatus.Validated
            }))
            .Unschedule(TokenTimeout)
            .Finalize();
    }
}