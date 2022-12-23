using FooCommerce.TokenService.Contracts;
using FooCommerce.TokensService.Tests.Setup;
using MassTransit;
using Xunit.Abstractions;

namespace FooCommerce.TokensService.Tests;

public class When_A_Token_Generated :
    FixturePerTest
{
    public When_A_Token_Generated(FixturePerTestClass fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public async Task Should_Validate_Stored_Token_In_Instance()
    {
        var identifier = NewId.NextGuid();

        var request = Harness.Bus.CreateRequestClient<GenerateCode>();

        var response = await request.GetResponse<TokenGenerated>(new
        {
            IdentifierId = identifier,
            LifetimeInSeconds = (int)TimeSpan.FromMinutes(5).TotalSeconds,
        });

        await Task.Delay(500);

        Assert.NotNull(response);
        Assert.NotNull(response.Message);
        Assert.NotEqual(DateTime.MinValue, response.Message.ExpiresOn);
    }
}