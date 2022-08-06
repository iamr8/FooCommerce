using FooCommerce.Infrastructure.Shopping.StateMachines;

using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FooCommerce.Infrastructure.Shopping.Components;

public class OrderStateMap :
    SagaClassMap<OrderState>
{
    protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
    {
        base.Configure(entity, model);

        entity.Property(x => x.CurrentState).HasMaxLength(40);

        entity.Property(x => x.OrderNumber).HasMaxLength(40);
    }
}