﻿using MassTransit;

namespace FooCommerce.NotificationAPI.Worker;

public class Worker : IHostedService
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
}