using Microsoft.Extensions.Logging;

namespace FooCommerce.Tests;

public delegate void OnLoggingEvent(LogLevel logLevel, string categoryName, EventId eventId, string message, Exception? exception);

public class ForwardingLoggerProvider : ILoggerProvider
{
    private readonly OnLoggingEvent _logAction;

    public ForwardingLoggerProvider(OnLoggingEvent logAction)
    {
        _logAction = logAction;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new ForwardingLogger(categoryName, _logAction);
    }

    public void Dispose()
    {
    }

    internal class ForwardingLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly OnLoggingEvent _logAction;

        public ForwardingLogger(string categoryName, OnLoggingEvent logAction)
        {
            _categoryName = categoryName;
            _logAction = logAction;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null!;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _logAction(logLevel, _categoryName, eventId, formatter(state, exception), exception);
        }
    }
}