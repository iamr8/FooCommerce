using FooCommerce.Products.Domain.Interfaces;

using MassTransit;

namespace FooCommerce.Products.Domain.Models;

public class CreateAdConsumer<TRequest> : IConsumer<TRequest> where TRequest : class, IAdRequest, new()
{
    private readonly TRequest _model;

    public CreateAdConsumer(TRequest model)
    {
        _model = model;
    }

    public async Task Consume(ConsumeContext<TRequest> context)
    {
        await context.Send(new TRequest());
    }
}