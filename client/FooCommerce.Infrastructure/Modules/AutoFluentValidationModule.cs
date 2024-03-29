﻿using FluentValidation.AspNetCore;

using FooCommerce.Common.Configurations;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Modules;

public class AutoFluentValidationModule : Module
{
    public void Load(IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
    }
}