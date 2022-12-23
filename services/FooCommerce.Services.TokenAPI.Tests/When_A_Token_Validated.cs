using FooCommerce.Services.TokenAPI.Contracts;
using FooCommerce.Services.TokenAPI.Enums;
using FooCommerce.Services.TokenAPI.Tests.Setup;

using MassTransit;
using MassTransit.Testing;

using Xunit.Abstractions;

namespace FooCommerce.Services.TokenAPI.Tests;

public class When_A_Token_Validated :
    FixturePerTest
{
    public When_A_Token_Validated(FixturePerTestClass fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public async Task Should_Validate_Stored_Token_In_Instance()
    {
        var identifier = NewId.NextGuid();

        // Arrange
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
            instance.Code
        });

        await Task.Delay(500);

        instance = SagaHarness.Created.ContainsInState(correlationId, SagaHarness.StateMachine, SagaHarness.StateMachine.Final);
        Assert.True(instance != null, "Saga instance not found");

        Assert.True(await Harness.Published.Any<TokenFulfilled>(x => ((TokenFulfilled)x.MessageObject).Status == TokenStatus.Validated));

        Assert.NotNull(instance.ValidatedOn);
    }
}