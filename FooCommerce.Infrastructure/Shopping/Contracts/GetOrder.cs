﻿using System.Runtime.CompilerServices;

using MassTransit;

namespace FooCommerce.Infrastructure.Shopping.Contracts;

public record GetOrder
{
    public Guid OrderId { get; init; }

    [ModuleInitializer]
    internal static void Init()
    {
        GlobalTopology.Send.UseCorrelationId<GetOrder>(x => x.OrderId);
    }
}