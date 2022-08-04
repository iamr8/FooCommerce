using FooCommerce.Products.Interfaces;
using FooCommerce.Products.PurchasableItems.Models;

using MassTransit;

namespace FooCommerce.Products.PurchasableItems.Commands;

public class CreatePurchasableItemConsumer : IConsumer<NewPurchasableItemRequest>
{
    public async Task Consume(ConsumeContext<NewPurchasableItemRequest> context)
    {
        await context.RespondAsync<IAdRequestResult>(new
        {
            IsSuccess = true
        });
    }
}