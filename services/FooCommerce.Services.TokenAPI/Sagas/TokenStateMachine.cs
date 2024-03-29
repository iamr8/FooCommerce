﻿using System.Security.Cryptography;
using FooCommerce.TokenService.Contracts;
using FooCommerce.TokenService.Enums;
using FooCommerce.TokenService.Sagas.Contracts;
using MassTransit;

namespace FooCommerce.TokenService.Sagas;

// ReSharper disable once ClassNeverInstantiated.Global
public class TokenStateMachine
    : MassTransitStateMachine<TokenState>
{
    public TokenStateMachine()
    {
        this.Event(() => GenerationRequested, x =>
        {
            x.CorrelateById(p => p.Message.IdentifierId)
                .SelectId(c => c.CorrelationId ?? NewId.NextGuid());
        });
        this.Event(() => ValidationRequested, x =>
        {
            x.OnMissingInstance(c =>
                c.ExecuteAsync(v =>
                    v.RespondAsync<TokenFulfilled>(new
                    {
                        Status = TokenStatus.NotFound
                    })));
            x.CorrelateById(p => p.Message.CorrelationId);
        });

        this.InstanceState(x => x.CurrentState, Active);
        Schedule(() => TokenTimeout, x => x.TokenTimeoutTokenId, x =>
        {
            x.DelayProvider = ctx => TimeSpan.FromSeconds(ctx.Saga.LifetimeInSeconds);
            x.Received = p => p.CorrelateById(context => context.Message.CorrelationId);
        });

        this.Initially(
            this.When(GenerationRequested)
                .Then(context =>
                {
                    context.Saga.LifetimeInSeconds = context.Message.LifetimeInSeconds;
                    context.Saga.IdentifierId = context.Message.IdentifierId;
                    context.Saga.Code = RandomNumberGenerator.GetInt32(10_000, 99_999).ToString();
                    context.Saga.GeneratedOn = DateTime.UtcNow;
                    context.Saga.ExpiresOn = DateTime.UtcNow.Add(TimeSpan.FromSeconds(context.Saga.LifetimeInSeconds));

                    Console.WriteLine("Code: " + context.Saga.Code);
                })
                .RespondAsync(context => context.Init<TokenGenerated>(new
                {
                    context.Saga.ExpiresOn,
                    context.Saga.CorrelationId,
                }))
                .Schedule(TokenTimeout, context => context.Init<TokenExpiredInternal>(new
                {
                    context.Saga.CorrelationId,
                }))
                .TransitionTo(Active)
        );

        this.During(Active,
            this.When(TokenTimeout.Received)
                .Then(context => context.Saga.InvalidatedOn = DateTime.UtcNow)
                .RespondAsync(context => context.Init<TokenFulfilled>(new
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

    public Schedule<TokenState, TokenExpiredInternal> TokenTimeout { get; private set; }

    public Event<GenerateCode> GenerationRequested { get; private set; }

    public Event<ValidateCode> ValidationRequested { get; private set; }

    private Func<EventActivityBinder<TokenState, ValidateCode>, EventActivityBinder<TokenState, ValidateCode>> WhenMaxRetryExceeded()
    {
        return ifTrue => ifTrue
            .Then(context => context.Saga.InvalidatedOn = DateTime.UtcNow)
            .RespondAsync(context => context.Init<TokenFulfilled>(new
            {
                Status = TokenStatus.MaxRetryExceeded
            }))
            .Unschedule(TokenTimeout)
            .Finalize();
    }

    private Func<EventActivityBinder<TokenState, ValidateCode>, EventActivityBinder<TokenState, ValidateCode>> WhenMaxRetryNotExceeded()
    {
        return ifFalse => ifFalse
            .RespondAsync(context => context.Init<TokenFulfilled>(new
            {
                Status = TokenStatus.CodeInvalid
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
            .RespondAsync(context => context.Init<TokenFulfilled>(new
            {
                Status = TokenStatus.Validated
            }))
            .Unschedule(TokenTimeout)
            .Finalize();
    }
}