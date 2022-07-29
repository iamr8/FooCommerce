using FooCommerce.Products.Domain.Interfaces;
using FooCommerce.Products.RealEstates.Models;

using MassTransit;

namespace FooCommerce.Products.RealEstates.Commands;

public class CreateRealEstateAdConsumer : IConsumer<NewRealEstateAdRequest>
{
    public async Task Consume(ConsumeContext<NewRealEstateAdRequest> context)
    {
        await context.RespondAsync<IAdRequestResult>(new
        {
            IsSuccess = true
        });
    }
}