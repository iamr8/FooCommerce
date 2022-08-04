using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace FooCommerce.Ordering.Tests.Internals;

public class XUnitLoggerFactory<T> : ILoggerFactory
{
    private ITestOutputHelper _output;

    public XUnitLoggerFactory(ITestOutputHelper output)
    {
        _output = output;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new XUnitLogger<T>(_output);
    }

    public void AddProvider(ILoggerProvider provider)
    {
        throw new NotImplementedException();
    }
}

public class XUnitLogger<T> : ILogger<T>, IDisposable
{
    private ITestOutputHelper _output;

    public XUnitLogger(ITestOutputHelper output)
    {
        _output = output;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        _output.WriteLine(state.ToString());
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return this;
    }

    public void Dispose()
    {
    }
}