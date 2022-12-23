using System.Diagnostics;
using FooCommerce.TokenService.Sagas;
using MassTransit.Testing;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace FooCommerce.TokensService.Tests.Setup;

public class FixturePerTest : IClassFixture<FixturePerTestClass>, IDisposable
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly Stopwatch _stopwatch;

    public FixturePerTest(FixturePerTestClass fixture, ITestOutputHelper outputHelper)
    {
        _stopwatch = Stopwatch.StartNew();
        _outputHelper = outputHelper;

        Fixture = fixture;
        Fixture.OnLogging += Log;
    }

    protected FixturePerTestClass Fixture { get; }
    protected IServiceProvider ServiceProvider => this.Fixture.ServiceProvider;

    protected ISagaStateMachineTestHarness<TokenStateMachine, TokenState> SagaHarness =>
        this.Fixture.Harness.GetSagaStateMachineHarness<TokenStateMachine, TokenState>();

    protected ITestHarness Harness => this.Fixture.Harness;

    private void Log(LogLevel logLevel, string fullQualifiedName, EventId eventId, string message, Exception? exception)
    {
        var logLvl = logLevel.ToString().ToUpper()[..3];
        var time = DateTime.UtcNow.Add(_stopwatch.Elapsed);
        var log = $"[{time:HH:mm:ss} {logLvl} TH{Thread.CurrentThread.ManagedThreadId}] {fullQualifiedName}";
        if (!string.IsNullOrEmpty(message))
            log += $"{Environment.NewLine}      {message}";

        if (exception != null)
            log += Environment.NewLine + exception;
        _outputHelper.WriteLine(log);
    }

    public void Dispose()
    {
        Fixture.OnLogging -= Log;
    }
}