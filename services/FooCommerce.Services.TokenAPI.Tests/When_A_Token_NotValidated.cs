using FooCommerce.TokenService.Contracts;
using FooCommerce.TokenService.Enums;
using FooCommerce.TokensService.Tests.Setup;
using MassTransit;
using MassTransit.Testing;
using Xunit.Abstractions;

namespace FooCommerce.TokensService.Tests;

public class When_A_Token_NotValidated :
    FixturePerTest
{
    public When_A_Token_NotValidated(FixturePerTestClass fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public async Task Should_Validate_Stored_Token_In_Instance()
    {
        var identifier = NewId.NextGuid();

        await Harness.Bus.Publish<GenerateCode>(new
        {
            IdentifierId = identifier,
            LifetimeInSeconds = (int)TimeSpan.FromMinutes(5).TotalSeconds,
        });

        await Task.Delay(500);

        // Assert
        Assert.True(await SagaHarness.Created.Any(x => x.IdentifierId == identifier));
        var correlationId = SagaHarness.Created.Select(x => x.IdentifierId == identifier).ElementAt(0).Saga.CorrelationId;
        Assert.True(await Harness.Published.Any<TokenGenerated>(x => ((TokenGenerated)x.MessageObject).CorrelationId == correlationId));

        var instance = SagaHarness.Created.ContainsInState(correlationId, SagaHarness.StateMachine, SagaHarness.StateMachine.Active);
        Assert.True(instance != null, "Saga instance not found");

        Assert.NotNull(instance.GeneratedOn);
        Assert.Equal(identifier, instance.IdentifierId);
        Assert.NotNull(instance.Code);

        await Harness.Bus.Publish<ValidateCode>(new
        {
            instance.CorrelationId,
            Code = "12345"
        });

        await Task.Delay(500);

        instance = SagaHarness.Created.ContainsInState(correlationId, SagaHarness.StateMachine, SagaHarness.StateMachine.Active);
        Assert.True(instance != null, "Saga instance not found");

        Assert.True(await Harness.Published.Any<TokenFulfilled>(x => ((TokenFulfilled)x.MessageObject).Status == TokenStatus.CodeInvalid));

        Assert.Single(instance.NotValidatedOn);
        Assert.Equal(1, instance.FalseTries);
    }
}