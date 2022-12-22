using MassTransit;

namespace FooCommerce.IdentityAPI.Worker;

public class Worker : IHostedService, IAsyncDisposable
{
    private readonly ILogger<Worker> _logger;
    private readonly IBusControl _bus;

    public Worker(ILogger<Worker> logger, IBusControl bus)
    {
        _logger = logger;
        _bus = bus;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting the {this.GetType().Assembly.FullName} bus...");
        return _bus.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Stopping the {this.GetType().Assembly.FullName} bus...");
        return _bus.StopAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _bus.StopAsync();
        GC.SuppressFinalize(this);
    }
}