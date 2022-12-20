using FooCommerce.Services.TokenAPI.Contracts;
using FooCommerce.Services.TokenAPI.Sagas;
using FooCommerce.Services.TokenAPI.Sagas.Contracts;
using FooCommerce.Services.TokenAPI.Tests.Setup;

using MassTransit;
using MassTransit.Testing;

using Xunit.Abstractions;

namespace FooCommerce.Services.TokenAPI.Tests.TokenSagaStateMachineTests;

public class When_A_Token_Validated_OnLastTry :
    FixturePerTest
{
    public When_A_Token_Validated_OnLastTry(FixturePerTestClass fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public async Task Should_Validate_Stored_Token_In_Instance()
    {
        var identifier = NewId.NextGuid();

        await this.Harness.Bus.Publish<GenerateCode>(new
        {
            IdentifierId = identifier,
            Seconds = (int)TimeSpan.FromMinutes(5).TotalSeconds,
        });

        await Task.Delay(500);

        // Assert
        Assert.True(await this.SagaHarness.Created.Any(x => x.CorrelationId == identifier));
        Assert.True(await this.SagaHarness.Consumed.Any<SagaTokenGenerated>(), "Message not consumed");

        var instance = this.SagaHarness.Created.ContainsInState(identifier, this.SagaHarness.StateMachine, this.SagaHarness.StateMachine.Active);
        Assert.True(instance != null, "Saga instance not found");

        Assert.NotNull(instance.GeneratedOn);
        Assert.Equal(identifier, instance.IdentifierId);
        Assert.NotNull(instance.Code);

        for (int i = 0; i < TokenState.MaxRetryCount; i++)
        {
            await this.Harness.Bus.Publish<ValidateCode>(new
            {
                IdentifierId = identifier,
                Code = i == 2
                    ? instance.Code
                    : new Random(Guid.NewGuid().GetHashCode()).NextInt64(10_000, 99_999).ToString()
            });

            await Task.Delay(500);
        }

        instance = this.SagaHarness.Created.ContainsInState(identifier, this.SagaHarness.StateMachine, this.SagaHarness.StateMachine.Final);
        Assert.True(instance != null, "Saga instance not found");

        Assert.NotNull(instance.ValidatedOn);
        Assert.Equal(2, instance.NotValidatedOn.Count);
    }
}