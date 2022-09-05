﻿using FooCommerce.Common.Configurations;
using FooCommerce.IdentityAPI.Worker.Extensions;

using MassTransit;

namespace FooCommerce.IdentityAPI.Worker.Modules;

public class BusModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddMassTransit(cfg => cfg.ConfigureBus());
    }
}