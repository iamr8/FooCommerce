using FooCommerce.Services.TokenAPI.Contracts;
using FooCommerce.Services.TokenAPI.Sagas;
using FooCommerce.Services.TokenAPI.Sagas.Contracts;
using FooCommerce.Services.TokenAPI.Tests.Setup;

using MassTransit;
using MassTransit.Testing;

using Xunit.Abstractions;

namespace FooCommerce.Services.TokenAPI.Tests;

public class When_A_Token_Invalidated_OnFalseTries :
    FixturePerTest
{
    public When_A_Token_Invalidated_OnFalseTries(FixturePerTestClass fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public async Task Should_Validate_Stored_Token_In_Instance()
    {
        var identifier = NewId.NextGuid();

        await Harness.Bus.Publish<GenerateCode>(new
        {
            IdentifierId = identifier,
            Seconds = (int)TimeSpan.FromMinutes(5).TotalSeconds,
        });

        await Task.Delay(500);

        // Assert
        Assert.True(await SagaHarness.Created.Any(x => x.CorrelationId == identifier));
        Assert.True(await SagaHarness.Consumed.Any<SagaTokenGenerated>(), "Message not consumed");

        var instance = SagaHarness.Created.ContainsInState(identifier, SagaHarness.StateMachine, SagaHarness.StateMachine.Active);
        Assert.True(instance != null, "Saga instance not found");

        Assert.NotNull(instance.GeneratedOn);
        Assert.Equal(identifier, instance.IdentifierId);
        Assert.NotNull(instance.Code);

        for (int i = 0; i < TokenState.MaxRetryCount; i++)
        {
            await Harness.Bus.Publish<ValidateCode>(new
            {
                IdentifierId = identifier,
                Code = new Random(Guid.NewGuid().GetHashCode()).NextInt64(10_000, 99_999).ToString()
            });

            await Task.Delay(500);
        }

        instance = SagaHarness.Created.ContainsInState(identifier, SagaHarness.StateMachine, SagaHarness.StateMachine.Final);
        Assert.True(instance != null, "Saga instance not found");

        Assert.Equal(3, instance.NotValidatedOn.Count);
        Assert.NotNull(instance.InvalidatedOn);
    }
}